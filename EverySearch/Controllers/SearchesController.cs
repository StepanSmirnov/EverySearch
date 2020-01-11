using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EverySearch.Lib;
using EverySearch.Models;
using Microsoft.AspNetCore.Mvc;

namespace EverySearch.Controllers
{
    public class SearchesController : Controller
    {
        private readonly EverySearchContext context;
        private readonly SearchManager searchManager;

        public SearchesController(EverySearchContext context, SearchManager searchManager)
        {
            this.context = context;
            this.searchManager = searchManager;
        }

        private async Task DoSomething()
        {
            await Task.Run(delegate() { System.Threading.Thread.Sleep(3000); });
            return;
        }

        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(string query)
        {
            Search search = new Search();
            search.Query = query;
            
            search.Timestamp = DateTime.Now;
            await DoSomething();
            return Json(new { code = 200 });
        }

        public async Task<IActionResult> ShowAsync(int Id)
        {
            //context.Searches.Find;
            return View();
        }
    }
}