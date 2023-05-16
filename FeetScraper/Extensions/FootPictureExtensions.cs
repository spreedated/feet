using FeetScraper.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using static FeetScraper.Logic.Constants;

namespace FeetScraper
{
    public static class FootPictureExtensions
    {
        public static async Task<byte[]> DownloadFootPictureAsync(this FootPicture footPicture, HttpClient client = null)
        {
            client ??= new();

            using (client)
            {
                client.Timeout = new TimeSpan(0, 0, 10);
                HttpResponseMessage p = await client.GetAsync(string.Format(FEET_PICTURE_URLTEMPLATE, footPicture.Name.Replace(' ', '_'), footPicture.Id));

                if (p == null || p.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return null;
                }

                return await p.Content.ReadAsByteArrayAsync();
            }
        }
    }
}
