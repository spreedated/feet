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
    public class FeetOfTheDay : IDisposable
    {
        internal HttpClient httpClient;

        private bool isRunning;

        public FeetOfTheDay()
        {
            this.httpClient = new();
        }

        public event EventHandler Completed;

        public event EventHandler Started;
        public Models.FeetOfTheDay FeetOfTheDayResponse { get; private set; }
        
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<Models.FeetOfTheDay> RetrieveAsync()
        {
            if (this.isRunning)
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

            this.FeetOfTheDayResponse = new()
            {
                Name = RetrieveName(source)
            };


            this.Completed?.Invoke(this, EventArgs.Empty);
            this.isRunning = false;

            return this.FeetOfTheDayResponse;
        }

        internal static string RetrieveName(HtmlNode htmlNode)
        {
            HtmlNodeCollection fotdboxes = htmlNode.SelectNodes("//div[@class='dash2']");
            HtmlNode fotdbox = null;
            if (fotdboxes != null && fotdboxes.Count > 0)
            {
                fotdbox = fotdboxes.First(x => x.SelectSingleNode(".//div/h3").InnerHtml.ToLower().Contains("feet") && x.SelectSingleNode(".//div/h3").InnerHtml.ToLower().Contains("day"));
            }

            return fotdbox?.SelectSingleNode(".//div[@class='dashdock']")?.InnerText;
        }

        protected virtual void Dispose(bool disposing)
        {
            this.httpClient?.Dispose();
        }

        private async Task<HtmlNode> DownloadSourcecode()
        {
            this.httpClient.Timeout = new TimeSpan(0, 0, 10);
            HttpResponseMessage res = await this.httpClient.GetAsync($"{FEET_URL}");

            HtmlDocument htmlDocument = new();
            htmlDocument.LoadHtml(await res.Content.ReadAsStringAsync());

            return htmlDocument.DocumentNode;
        }
    }
}
