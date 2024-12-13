using System.Collections.Generic;
using System.Threading.Tasks;
using HomeSearchAssessment.Clients;
using HomeSearchAssessment.Models;
using Microsoft.Extensions.Logging;

namespace HomeSearchAssessment.Facades
{
    public class HomeSearchFacade
    {
        private readonly ILogger _logger;
        private readonly IHomeSearchClient _client;

        public HomeSearchFacade(IHomeSearchClient client)
        {
            _logger = new Logger<HomeSearchFacade>(new LoggerFactory());
            _client = client;
        }

        public Task<List<Policy>> GetPoliciesByPostcode(string postcode)
        {
            // Todo: Add code here
            return null;
        }
    }
}