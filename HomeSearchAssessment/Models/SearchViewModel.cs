using System.Collections.Generic;

namespace HomeSearchAssessment.Models
{
    public class SearchViewModel
    {
        public SearchViewModel()
        {
            Postcode = string.Empty;
            Properties = new List<Property>();
        }
        
        public string Postcode { get; set; }
        
        public List<Property> Properties { get; set; }
    }
}