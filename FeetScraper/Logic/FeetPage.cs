using FeetScraper.Models;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static FeetScraper.Logic.Constants;
using static FeetScraper.Logic.HelperFunctions;

namespace FeetScraper.Logic
{
    public class FeetPage : IDisposable
    {
        internal HttpClient httpClient;

        private bool isRunning;

        public FeetPage(string name)
        {
            this.httpClient = new();
            this.Name = name;
        }

        public event EventHandler Completed;

        public event EventHandler Started;
        public string Name { get; init; }
        public PageResponse PageResponse { get; private set; }
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<PageResponse> RetrieveAsync()
        {
            if (string.IsNullOrEmpty(this.Name) || this.isRunning)
            {
                return null;
            }

            this.isRunning = true;
            this.Started?.Invoke(this, EventArgs.Empty);

            HtmlNode source = await this.DownloadSourcecode();

            if (source == null)
            {
                this.Completed?.Invoke(this, EventArgs.Empty);
                return null;
            }

            this.PageResponse = new()
            {
                Name = this.Name,
                ShoeSize = RetrieveShoesize(source),
                RatingStats = RetrieveFeetRatingStats(source),
                Birthday = RetrieveBirthday(source),
                Birthplace = RetrieveBirthplace(source),
                Feet = RetrieveFootPictures(source, this.Name),
            };

            this.Completed?.Invoke(this, EventArgs.Empty);
            this.isRunning = false;

            return this.PageResponse;
        }

        internal static DateTime? RetrieveBirthday(HtmlNode htmlNode)
        {
            string s = htmlNode?.SelectSingleNode("//span[@id='bdate_label']")?.InnerText;

            if (s == null)
            {
                return null;
            }

            if (s.Contains(' '))
            {
                s = s.Substring(0, s.IndexOf(' '));
            }

            _ = DateTime.TryParse(s, out DateTime dd);

            return dd == default ? null : dd;
        }

        internal static string RetrieveBirthplace(HtmlNode htmlNode)
        {
            string s = htmlNode?.SelectSingleNode("//span[@id='nation_label']")?.InnerText?.Replace("edit", "")?.Trim();

            if (s == null || s.ToLower().Contains("not set"))
            {
                return null;
            }

            return s;
        }

        internal static int RetrieveFeetPicsCount(HtmlNode htmlNode)
        {
            string script = htmlNode?.SelectSingleNode("//div[@id='thepics']/script")?.InnerHtml;

            if (script == null)
            {
                return default;
            }

            script = script.Substring(script.IndexOf("gdata"));
            script = script.Substring(script.IndexOf("["));
            script = script.Substring(0, script.IndexOf("]") + 1);

            if (!IsValidJson(script, out JToken jToken))
            {
                return default;
            }

            return jToken.Children().Count();
        }

        internal static FeetRatingStats RetrieveFeetRatingStats(HtmlNode htmlNode)
        {
            HtmlNode ratings = htmlNode?.SelectSingleNode(".//div[@class='round10']/div[2]/div[2]/div");

            if (ratings == null || ratings.ChildNodes.Count <= 4)
            {
                return null;
            }

            int[] rates = new int[5];
            int i = 0;

            foreach (HtmlNode n in ratings.ChildNodes.Where(x => x.Name == "div"))
            {
                StringBuilder sint = new();
                foreach (char inner in n.InnerText.Trim())
                {
                    if (!int.TryParse(inner.ToString(), out _))
                    {
                        continue;
                    }
                    sint.Append(inner);
                }

                rates[i] = Convert.ToInt32(sint.ToString());

                i++;

                if (i > rates.Length)
                {
                    continue;
                }
            }

            return new()
            {
                Beautiful = rates[0],
                Nice = rates[1],
                Okay = rates[2],
                Bad = rates[3],
                Ugly = rates[4]
            };
        }

        internal static IEnumerable<FootPicture> RetrieveFootPictures(HtmlNode htmlNode, string name)
        {
            string script = htmlNode?.SelectSingleNode("//div[@id='thepics']/script")?.InnerHtml;

            if (script == null)
            {
                yield break;
            }

            script = script.Substring(script.IndexOf("gdata"));
            script = script.Substring(script.IndexOf("["));
            script = script.Substring(0, script.IndexOf("]") + 1);

            if (!IsValidJson(script, out JToken jToken))
            {
                yield break;
            }

            foreach (JToken c in jToken.Children())
            {
                yield return new()
                {
                    FootTags = GetFoottags(c["tags"]?.ToString()),
                    Name = name,
                    Id = int.TryParse(c["pid"]?.ToString(), out _) ? Convert.ToInt32(c["pid"]?.ToString()) : default,
                    Height = int.TryParse(c["ph"]?.ToString(), out _) ? Convert.ToInt32(c["ph"]?.ToString()) : default,
                    Width = int.TryParse(c["pw"]?.ToString(), out _) ? Convert.ToInt32(c["pw"]?.ToString()) : default
                };
            }
        }

        internal static string RetrieveName(HtmlNode htmlNode)
        {
            return htmlNode?.SelectSingleNode("//div[@class='round10']//h1")?.InnerText;
        }

        internal static float? RetrieveShoesize(HtmlNode htmlNode)
        {
            string s = htmlNode?.SelectSingleNode("//span[@id='ssize_label']")?.InnerText?.Trim();

            if (s == null || s.ToLower().Contains("not set"))
            {
                return null;
            }

            StringBuilder sint = new();

            foreach (char ss in s)
            {
                if (!int.TryParse(ss.ToString(), out _) && ss != '.')
                {
                    continue;
                }
                sint.Append(ss);
            }

            return float.Parse(sint.ToString(), CultureInfo.InvariantCulture);
        }

        protected virtual void Dispose(bool disposing)
        {
            this.httpClient?.Dispose();
        }

        private static IEnumerable<FootTag> GetFoottags(string tags)
        {
            if (tags == null)
            {
                yield break;
            }

            foreach (char tc in tags.ToUpper())
            {
                yield return new(tc);
            }
        }
        private async Task<HtmlNode> DownloadSourcecode()
        {
            this.httpClient.Timeout = new TimeSpan(0, 0, 10);
            HttpResponseMessage res = await this.httpClient.GetAsync($"{FEET_URL}{this.Name.Replace(' ', '_')}");

            HtmlDocument htmlDocument = new();
            htmlDocument.LoadHtml(await res.Content.ReadAsStringAsync());

            return htmlDocument.DocumentNode;
        }
    }
}
