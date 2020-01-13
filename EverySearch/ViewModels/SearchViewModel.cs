using EverySearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EverySearch.ViewModels
{
    public class SearchSavedViewModel
    {
        public string Filter { get; set; }
        public IEnumerable<Search> Searches { get; set; }
    }
}
