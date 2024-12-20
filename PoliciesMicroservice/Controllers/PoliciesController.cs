using Microsoft.AspNetCore.Mvc;
using PoliciesMicroservice.Models;

namespace PoliciesMicroservice.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PoliciesController
{
    private readonly List<Policy> policies = new()
    {
        new Policy
        {
            Id = 1,
            Property = new Property
            {
                StreetAddress = "65 George Street BOURNEMOUTH",
                Postcode = "BH943KV",
                PriceInPounds = 255000
            }
        },
        new Policy
        {
            Id = 2,
            Property = new Property
            {
                StreetAddress = "284 The Crescent DUDLEY",
                Postcode = "DY694KY",
                PriceInPounds = 389000
            }
        },
        new Policy
        {
            Id = 3,
            Property = new Property
            {
                StreetAddress = "30 Windsor Road TWICKENHAM",
                Postcode = "TW987OF",
                PriceInPounds = 123500
            }
        },
        new Policy
        {
            Id = 4,
            Property = new Property
            {
                StreetAddress = "9022 Green Lane DONCASTER",
                Postcode = "DN977MJ",
                PriceInPounds = 178500
            }
        },
        new Policy
        {
            Id = 5,
            Property = new Property
            {
                StreetAddress = "20 New Road NOTTINGHAM",
                Postcode = "NG549BQ",
                PriceInPounds = 1150000
            }
        },
        new Policy
        {
            Id = 6,
            Property = new Property
            {
                StreetAddress = "599 Mill Road BOURNEMOUTH",
                Postcode = "BH141PA",
                PriceInPounds = 620000
            }
        },
        new Policy
        {
            Id = 7,
            Property = new Property
            {
                StreetAddress = "637 Mill Road BOURNEMOUTH",
                Postcode = "BH141PA",
                PriceInPounds = 640000
            }
        },
        new Policy
        {
            Id = 8,
            Property = new Property
            {
                StreetAddress = "49 Park Avenue TWICKENHAM",
                Postcode = "TW270RE",
                PriceInPounds = 660000
            }
        },
        new Policy
        {
            Id = 9,
            Property = new Property
            {
                StreetAddress = "25 West Street BOURNEMOUTH",
                Postcode = "BH994MH",
                PriceInPounds = 478000
            }
        }
    };

    [HttpGet()]
    public IActionResult Get()
    {
        return new OkObjectResult(policies);
    }

    [HttpGet("{policyId}")]
    public IActionResult Get(int policyId)
    {
        var policy = policies.FirstOrDefault(p => p.Id == policyId);

        if (policy == null)
        {
            return new NotFoundResult();
        }

        return new OkObjectResult(policy);
    }
}