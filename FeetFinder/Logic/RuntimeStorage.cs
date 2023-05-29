using FeetFinder.Service;
using FeetScraper.Models;

namespace FeetFinder.Logic
{
    internal static class RuntimeStorage
    {
        public static DownloadService DownloadService { get; set; }
        public static PageResponse FeetofthedayResponse { get; set; }
        public static FeetOfTheDay Feetoftheday { get; set; }
    }
}
