using System.IO;

namespace UnitTests.TestFunctions
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
    }
}
