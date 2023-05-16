using System.IO;

namespace FeetScraperTests.TestFunctions
{
    internal static class HelperFunctions
    {
        internal static string GetEmbeddedHtml(string name)
        {
            using (Stream s = typeof(HelperFunctions).Assembly.GetManifestResourceStream($"{typeof(HelperFunctions).Assembly.GetName().Name}.TestHtmls.{name}"))
            {
                using (StreamReader r = new(s))
                {
                    return r.ReadToEnd();
                }
            }
        }

        internal static byte[] GetEmbeddedPicture(string name)
        {
            byte[] buffer = null;
            using (Stream s = typeof(HelperFunctions).Assembly.GetManifestResourceStream($"{typeof(HelperFunctions).Assembly.GetName().Name}.TestPictures.{name}"))
            {
                buffer = new byte[s.Length];

                s.Read(buffer, 0, buffer.Length);
            }
            return buffer;
        }
    }
}
