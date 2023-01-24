using System.Diagnostics;
using HomeSearchAssessment.Facades;
using HomeSearchAssessment.Models;
using HomeSearchAssessment.Readers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HomeSearchAssessment.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PropertyDatabaseReader _reader;
        private readonly HomeSearchFacade _facade;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _reader = new PropertyDatabaseReader();
            _facade = new HomeSearchFacade(_reader.Read());
        }

        [HttpGet]
        public IActionResult Search()
        {
            return View(new SearchViewModel());
        }
        
        [HttpPost]
        public IActionResult Search(SearchViewModel searchViewModel)
        {
            var postcode = searchViewModel.Postcode;
            const string message = "Searching by postcode {0}";
            _logger.LogInformation(string.Format(message, postcode));
            
            searchViewModel.Properties = _facade.GetPropertiesByPostcode(postcode ?? string.Empty);
            
            return View(searchViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
