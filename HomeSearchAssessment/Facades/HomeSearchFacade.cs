using System.Collections.Generic;
using System.Linq;
using HomeSearchAssessment.Models;

namespace HomeSearchAssessment.Facades
{
    public class HomeSearchFacade
    {
        private List<Property> _propertiesFromDatabase;
        
        public HomeSearchFacade(List<Property> propertiesFromDatabase)
        {
            _propertiesFromDatabase = propertiesFromDatabase;
        }
        
        public List<Property> GetPropertiesByPostcode(string postcode)
        {
            // Your code goes here
            return null;
        }

        private static bool matchesWildcard(string search, string target)
        {
            // You can use this helper for the wildcard matching logic if you want
            return false;
        }
    }
}