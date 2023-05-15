using System;
using System.Collections.Generic;

namespace FeetScraper.Models
{
    public class SearchResponse
    {
        public string Name { get; set; }
        public int Piccount { get; set; }
        public FeetRating Rating { get; set; }
        public DateTime Updated { get; set; }
        public string PageLink { get; set; }
        public IEnumerable<string> ThumbnailLinks { get; set; }
    }
}
