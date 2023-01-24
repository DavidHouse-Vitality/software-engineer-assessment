using System.Collections.Generic;
using HomeSearchAssessment.Models;

namespace HomeSearchAssessment.Readers
{
    public class PropertyDatabaseReader
    {
        public List<Property> Read()
        {
            var properties = new List<Property>
            {
                new Property
                {
                    StreetAddress = "65 George Street BOURNEMOUTH",
                    Postcode = "BH943KV",
                    PriceInPounds = 255000,
                    IsFreehold = true
                },
                new Property
                {
                    StreetAddress = "284 The Crescent DUDLEY",
                    Postcode = "DY694KY",
                    PriceInPounds = 389000,
                    IsFreehold = true
                },
                new Property
                {
                    StreetAddress = "30 Windsor Road TWICKENHAM",
                    Postcode = "TW987OF",
                    PriceInPounds = 123500,
                    IsFreehold = false
                },
                new Property
                {
                    StreetAddress = "9022 Green Lane DONCASTER",
                    Postcode = "DN977MJ",
                    PriceInPounds = 178500,
                    IsFreehold = false
                },
                new Property
                {
                    StreetAddress = "20 New Road NOTTINGHAM",
                    Postcode = "NG549BQ",
                    PriceInPounds = 1150000,
                    IsFreehold = true
                },
                new Property
                {
                    StreetAddress = "599 Mill Road BOURNEMOUTH",
                    Postcode = "BH141PA",
                    PriceInPounds = 620000,
                    IsFreehold = true
                },
                new Property
                {
                    StreetAddress = "637 Mill Road BOURNEMOUTH",
                    Postcode = "BH141PA",
                    PriceInPounds = 640000,
                    IsFreehold = true
                },
                new Property
                {
                    StreetAddress = "49 Park Avenue TWICKENHAM",
                    Postcode = "TW270RE",
                    PriceInPounds = 660000,
                    IsFreehold = true
                },
                new Property
                {
                    StreetAddress = "25 West Street BOURNEMOUTH",
                    Postcode = "BH994MH",
                    PriceInPounds = 478000,
                    IsFreehold = true
                }
            };
            return properties;
            
        }
    }
}