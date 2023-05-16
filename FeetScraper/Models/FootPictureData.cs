using System.Collections.Generic;

namespace FeetScraper.Models
{
    public class FootPictureData
    {
        public string ContentType { get; set; }
        public IEnumerable<byte> Data { get; set; }
    }
}
