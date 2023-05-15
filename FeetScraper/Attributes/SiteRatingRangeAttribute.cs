using System;

namespace FeetScraper.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class SiteRatingRangeAttribute : Attribute
    {
        public int FromValue { get; init; }
        public int ToValue { get; init; }
        public SiteRatingRangeAttribute(int from, int to) : base()
        {
            this.FromValue = from;
            this.ToValue = to;
        }
    }
}
