using EverySearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EverySearch
{
    public class SearchViewModel
    {
        public bool HasError { get; set; }
        public IEnumerable<SearchResult> searchResults { get; set; }
        public string ErrorMessage { get; set; }
    }
}
