using FeetScraper.Models;
using System;
using System.Collections.Generic;

namespace FeetScraper.EventArguments
{
    public class SearchCompletedEventArgs : EventArgs
    {
        public IEnumerable<SearchResponse> SearchResponses { get; init; }
        public SearchCompletedEventArgs(IEnumerable<SearchResponse> searchResponses) : base()
        {
            this.SearchResponses = searchResponses;
        }
    }
}
