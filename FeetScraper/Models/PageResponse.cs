using System;
using System.Collections.Generic;

namespace FeetScraper.Models
{
    public class PageResponse
    {
        public DateTime Birthday { get; set; }
        public string Birthplace { get; set; }
        public IEnumerable<object> Feet { get; set; }
        public int FeetCount { get; set; }
        public string Name { get; set; }
        public FeetRating Rating { get; set; }
        public object RatingStats { get; set; }
        public string ShoeSize { get; set; }
    }
}
