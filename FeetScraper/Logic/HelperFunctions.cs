using System.Linq;
using System.Text;

namespace FeetScraper.Logic
{
    internal static class HelperFunctions
    {
        public static string HtmlString(string str)
        {
            return str.Replace(" ", "%20");
        }

        public static string GetCssAttribute(string inlinestyle, string attribute)
        {
            StringBuilder b = new();
            b.Append(inlinestyle?.Split(';').FirstOrDefault(x => x.Contains(attribute)));

            if (b.Length <= 0)
            {
                return null;
            }

            int l = b.Length;

            b.Append(b.ToString().Substring(b.ToString().IndexOf(':') + 1));
            b.Remove(0, l);

            return b.ToString();
        }

        public static string GetCssBackgroundImage(string inlinestyle)
        {
            string b = inlinestyle.Substring(inlinestyle.IndexOf('\'') + 1);
            b = b.Substring(0, b.LastIndexOf('\''));
            return b;
        }
    }
}
