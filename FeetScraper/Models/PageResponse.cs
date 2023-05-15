using System;
using System.Collections.Generic;

namespace FeetScraper.Models
{
    public class PageResponse
    {
        public DateTime? Birthday { get; set; }
        public string Birthplace { get; set; }
        public IEnumerable<FootPicture> Feet { get; set; }
        public string Name { get; set; }
        public FeetRatingStats RatingStats { get; set; }
        public float? ShoeSize { get; set; }
    }
}
