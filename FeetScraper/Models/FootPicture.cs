using Newtonsoft.Json;
using System.Collections.Generic;
using static FeetScraper.Logic.Constants;

namespace FeetScraper.Models
{
    public class FootPicture
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("width")]
        public int Width { get; set; }
        [JsonProperty("height")]
        public int Height { get; set; }
        [JsonIgnore()]
        public string Dimensions
        {
            get
            {
                return $"{this.Width}x{this.Height}";
            }
        }
        [JsonIgnore()]
        public string Link
        {
            get
            {
                return string.Format(FEET_PICTURE_URLTEMPLATE, this.Name?.Replace(' ', '-'), this.Id);
            }
        }
        [JsonProperty("tags")]
        public IEnumerable<FootTag> FootTags { get; set; }
        [JsonProperty("picture")]
        public byte[] Picture { get; set; }
    }
}
