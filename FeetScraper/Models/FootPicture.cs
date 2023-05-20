using System.Collections.Generic;
using static FeetScraper.Logic.Constants;

namespace FeetScraper.Models
{
    public class FootPicture
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Dimensions
        {
            get
            {
                return $"{this.Width}x{this.Height}";
            }
        }
        public string Link
        {
            get
            {
                return string.Format(FEET_PICTURE_URLTEMPLATE, this.Name?.Replace(' ', '-'), this.Id);
            }
        }
        public IEnumerable<FootTag> FootTags { get; set; }
    }
}
