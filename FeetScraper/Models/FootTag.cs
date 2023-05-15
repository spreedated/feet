using FeetScraper.Attributes;
using System.Linq;
using System.Reflection;

namespace FeetScraper.Models
{
    public class FootTag
    {
        public enum Foottags
        {
            [Foottag('S', "Soles")]
            Soles,
            [Foottag('N', "Nylons")]
            Nylons,
            [Foottag('C', "Close-Up")]
            CloseUp,
            [Foottag('A', "Arches")]
            Arches,
            [Foottag('B', "Barefoot")]
            Barefoot,
            [Foottag('T', "Toenails")]
            Toenails
        }

        public char Id { get; init; }
        public string Name
        {
            get
            {
                FieldInfo p = typeof(Foottags).GetFields(BindingFlags.Public | BindingFlags.Static).FirstOrDefault(x => x.GetCustomAttribute<FoottagAttribute>().Id == this.Id);

                if (p == null)
                {
                    return null;
                }

                return p.GetCustomAttribute<FoottagAttribute>().Description;
            }
        }
        public FootTag(char id)
        {
            this.Id = id;
        }
    }
}
