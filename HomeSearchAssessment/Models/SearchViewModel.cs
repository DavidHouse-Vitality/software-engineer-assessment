using System.Collections.Generic;

namespace HomeSearchAssessment.Models;

public class SearchViewModel
{
    public SearchViewModel()
    {
        Postcode = string.Empty;
        Policies = new List<Policy>();
    }
        
    public string Postcode { get; set; }
        
    public List<Policy> Policies { get; set; }
}