using FeetScraper.Attributes;
using System.Linq;
using System.Reflection;
using static FeetScraper.Logic.Constants;

namespace FeetScraper.Models
{
    public class FeetRating
    {
        public enum FeetSiteRatings
        {
            Unrated = 0,
            [SiteRatingRange(FEET_GORGEOUS_RATING_WIDTH, FEET_GORGEOUS_RATING_WIDTH)]
            Gorgeous,
            [SiteRatingRange(108, 97)]
            Beautiful,
            [SiteRatingRange(96, 75)]
            Nice,
            [SiteRatingRange(74, 54)]
            Ok,
            [SiteRatingRange(53, 32)]
            Bad,
            [SiteRatingRange(31, 1)]
            Ugly
        }

        public FeetSiteRatings FeetSiteRating
        {
            get
            {
                if (this.Width == default || this.Width < 0)
                {
                    return FeetSiteRatings.Unrated;
                }

                FieldInfo t = typeof(FeetSiteRatings).GetFields(BindingFlags.Public | BindingFlags.Static)
                              .First(x => x.GetCustomAttributes<SiteRatingRangeAttribute>().Any(x => x.FromValue >= this.Width && x.ToValue <= this.Width));

                return (FeetSiteRatings)t.GetValue(this);
            }
        }

        public double Percent
        {
            get
            {
                return this.Width * 100 / FEET_MAX_RATING_WIDTH;
            }
        }

        public string PercentString
        {
            get
            {
                return $"{this.Percent:N2} %";
            }
        }

        public double Width { get; set; }
    }
}
