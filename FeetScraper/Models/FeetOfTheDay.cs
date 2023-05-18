using static FeetScraper.Logic.Constants;

namespace FeetScraper.Models
{
    public class FeetOfTheDay
    {
        public string Name { get; set; }
        public string Link
        {
            get
            {
                return $"{FEET_URL}{this.Name.Replace(' ', '_')}";
            }
        }
    }
}
