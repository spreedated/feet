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
    public class FeetPage : IDisposable
    {
        internal HttpClient httpClient;

        private bool isRunning;

        public event EventHandler Started;
        public event EventHandler Completed;

        public string Name { get; init; }
        public PageResponse PageResponse { get; private set; }

        public FeetPage(string name)
        {
            this.httpClient = new();
            this.Name = name;
        }

        public async Task<PageResponse> RetrieveAsync()
        {
            if (string.IsNullOrEmpty(this.Name) || this.isRunning)
            {
                return null;
            }

            this.isRunning = true;

            HtmlNode source = await this.DownloadSourcecode();

            if (source == null)
            {
                return null;
            }

            this.PageResponse = new()
            {
                
            };

            this.isRunning = false;

            return null;
        }

        internal static string RetrieveBirthplace(HtmlNode htmlNode)
        {
            string s = htmlNode?.SelectSingleNode("//span[@id='nation_label']")?.InnerText?.Replace("edit","")?.Trim();

            if (s == null || s.ToLower().Contains("not set"))
            {
                return null;
            }

            return s;
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

        internal static string RetrieveName(HtmlNode htmlNode)
        {
            return htmlNode?.SelectSingleNode("//div[@class='round10']//h1")?.InnerText;
        }

        private async Task<HtmlNode> DownloadSourcecode()
        {
            this.httpClient.Timeout = new TimeSpan(0, 0, 10);
            HttpResponseMessage res = await this.httpClient.GetAsync($"{FEET_URL}{HtmlString(this.Name)}");

            HtmlDocument htmlDocument = new();
            htmlDocument.LoadHtml(await res.Content.ReadAsStringAsync());

            return htmlDocument.DocumentNode;
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
