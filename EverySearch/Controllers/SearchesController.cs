using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EverySearch.Controllers
{
    public class SearchesController : Controller
    {
        public IActionResult New()
        {
            return View();
        }
    }
}