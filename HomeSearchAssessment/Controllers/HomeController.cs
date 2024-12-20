using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using HomeSearchAssessment.Clients;
using HomeSearchAssessment.Facades;
using HomeSearchAssessment.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HomeSearchAssessment.Controllers;

public class HomeController : Controller
{
    private static readonly HttpClient HttpClient = new();
    private readonly ILogger<HomeController> _logger;
    private readonly HomeSearchFacade _facade;

    public HomeController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;

        var scheme = httpContextAccessor.HttpContext.Request.Scheme;
        var host = httpContextAccessor.HttpContext.Request.Host.Value;

        var policiesMicroserviceUrl = $"{scheme}://{host}/api/policies";
        var claimsMicroserviceUrl = $"{scheme}://{host}/api/claims";

        var client = new HomeSearchClient(HttpClient, policiesMicroserviceUrl, claimsMicroserviceUrl);

        _facade = new HomeSearchFacade(client);
    }

    [HttpGet]
    public IActionResult Search()
    {
        return View(new SearchViewModel());
    }
        
    [HttpPost]
    public async Task<IActionResult> Search(SearchViewModel searchViewModel)
    {
        var postcode = searchViewModel.Postcode;
        _logger.LogInformation("Searching by postcode {Postcode}", postcode);
            
        searchViewModel.Policies = await _facade.GetPoliciesByPostcode(postcode ?? string.Empty);
            
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