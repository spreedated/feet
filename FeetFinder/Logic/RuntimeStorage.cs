using FeetScraper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeetFinder.Logic
{
    internal static class RuntimeStorage
    {
        public static PageResponse FeetofthedayResponse { get; set; }
        public static FeetOfTheDay Feetoftheday { get; set; }
    }
}
