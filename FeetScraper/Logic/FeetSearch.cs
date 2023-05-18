using FeetScraper.EventArguments;
using FeetScraper.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static FeetScraper.Logic.Constants;
using static FeetScraper.Logic.HelperFunctions;

namespace FeetScraper.Logic
{
    public class FeetSearch : IDisposable
    {
        public enum Genders
        {
            Women,
            Men
        }

        internal List<SearchResponse> searchResponses;
        internal HttpClient httpClient;

        private bool isRunning;

        public Genders Gender { get; init; }

        public event EventHandler Started;
        public event EventHandler<SearchCompletedEventArgs> Completed;

        public FeetSearch(Genders gender = Genders.Women)
        {
            this.httpClient = new();
            this.searchResponses = new();
            this.Gender = gender;
        }

        public async Task<IEnumerable<SearchResponse>> SearchAsync(string name)
        {
            if (string.IsNullOrEmpty(name) || this.isRunning)
            {
                return Array.Empty<SearchResponse>();
            }

            this.Started?.Invoke(this, EventArgs.Empty);
            this.isRunning = true;
            this.searchResponses.Clear();

            HtmlNodeCollection nodes = await this.DownloadSourcecode(name);

            if (nodes == null || nodes.Count <= 0)
            {
                this.isRunning = false;
                this.Completed?.Invoke(this, new(this.searchResponses));
                return Array.Empty<SearchResponse>();
            }

            foreach (HtmlNode celebNode in nodes)
            {
                searchResponses.Add(new()
                {
                    Name = RetrieveName(celebNode),
                    Piccount = RetrievePiccount(celebNode),
                    Updated = RetrieveUpdatedDate(celebNode),
                    PageLink = RetrievePagelink(celebNode),
                    Rating = RetrieveRating(celebNode),
                    ThumbnailLinks = RetrieveThumbnails(celebNode)
                });
            }

            this.isRunning = false;
            this.Completed?.Invoke(this, new(this.searchResponses));
            return this.searchResponses;
        }

        internal static string RetrieveName(HtmlNode htmlNode)
        {
            return htmlNode?.SelectSingleNode(".//div[@class='boxtitle']/div/a")?.InnerText;
        }

        internal static IEnumerable<string> RetrieveThumbnails(HtmlNode htmlNode)
        {
            if (htmlNode == null)
            {
                yield break;
            }

            HtmlNodeCollection nodes = htmlNode?.SelectNodes(".//div[@class='boxcont']/a/div");

            if (nodes == null)
            {
                yield break;
            }

            foreach (HtmlNode thumbNode in nodes)
            {
                yield return GetCssBackgroundImage(GetCssAttribute(thumbNode.GetAttributeValue<string>("style", null), "background-image"));
            }
        }

        internal static FeetRating RetrieveRating(HtmlNode htmlNode)
        {
            double width = Convert.ToDouble(GetCssAttribute(htmlNode?.SelectSingleNode(".//div[@class='boxtitle']/div/div/div")?.GetAttributeValue<string>("style", null), "width")?.Replace("px", ""));

            if (width == default)
            {
                var s = htmlNode?.SelectSingleNode(".//div[@class='boxtitle']/div/div/img");
                if (s == null || s.GetAttributeValue<string>("src", null) == null)
                {
                    return null;
                }
                string img = s.GetAttributeValue<string>("src", null)?.ToLower();
                if (img.Contains("stars0"))
                {
                    return new();
                }
                if (img.Contains("stars2"))
                {
                    return new()
                    {
                        Width = FEET_GORGEOUS_RATING_WIDTH
                    };
                }
                return null;
            }

            return new()
            {
                Width = width
            };
        }

        internal static string RetrievePagelink(HtmlNode htmlNode)
        {
            StringBuilder pagelink = new();

            pagelink.Append(htmlNode?.SelectSingleNode(".//div[@class='boxtitle']/div/a")?.GetAttributeValue<string>("href", null)?.TrimStart('/'));

            if (pagelink.Length <= 0)
            {
                return default;
            }

            if (pagelink.Length >= 3)
            {
                pagelink.Insert(0, FEET_URL);
            }

            return pagelink.ToString();
        }

        internal static DateTime RetrieveUpdatedDate(HtmlNode htmlNode)
        {
            string updatedstring = htmlNode?.SelectSingleNode(".//div[@class='boxcont']/div")?.InnerText.Replace("Updated: ", "").Trim();

            if (updatedstring == null || (!updatedstring.Contains('-') && updatedstring.Count(x => x == '-') <= 1))
            {
                return default;
            }

            _ = DateTime.TryParse(updatedstring, out DateTime updated);

            return updated;
        }

        internal static int RetrievePiccount(HtmlNode htmlNode)
        {
            string piccount = htmlNode?.SelectSingleNode(".//div[@class='boxtitle']/div/small")?.InnerText;

            if (piccount == null)
            {
                return default;
            }

            if (piccount.Contains('(') || piccount.Contains(' '))
            {
                piccount = piccount?.Substring(piccount.IndexOf('(') + 1, piccount.IndexOf(' '));
            }

            _ = int.TryParse(piccount.Trim(), out int picc);

            return picc;
        }

        private async Task<HtmlNodeCollection> DownloadSourcecode(string name)
        {
            this.httpClient.Timeout = new TimeSpan(0, 0, 10);
            HttpResponseMessage res = await this.httpClient.GetAsync($"{(this.Gender == Genders.Women ? FEET_URL : FEET_URL_MEN)}search/{HtmlString(name)}");

            HtmlDocument htmlDocument = new();
            htmlDocument.LoadHtml(await res.Content.ReadAsStringAsync());

            return htmlDocument.DocumentNode?.SelectNodes("//div[@class='round8 celebbox']");
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            this.httpClient?.Dispose();
        }
    }
}
