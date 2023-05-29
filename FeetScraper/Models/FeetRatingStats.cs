using System.Linq;
using System.Reflection;

namespace FeetScraper.Models
{
    public class FeetRatingStats
    {
        public int Beautiful { get; set; }
        public int Nice { get; set; }
        public int Okay { get; set; }
        public int Bad { get; set; }
        public int Ugly { get; set; }
        public int Total
        {
            get
            {
                int total = 0;

                foreach (PropertyInfo item in typeof(FeetRatingStats).GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(x => x.Name != "Total" && x.PropertyType == typeof(int)))
                {
                    total += (int)item.GetValue(this);
                }

                return total;
            }
        }
        public float FiveStarRating
        {
            get
            {
                if (this.Total <= 0)
                {
                    return 0.0f;
                }

                float beautiful = this.Beautiful * 5;
                float nice = this.Nice * 4;
                float okay = this.Okay * 3;
                float bad = this.Bad * 2;

                return (beautiful + nice + okay + bad + this.Ugly) / this.Total;
            }
        }
    }
}
