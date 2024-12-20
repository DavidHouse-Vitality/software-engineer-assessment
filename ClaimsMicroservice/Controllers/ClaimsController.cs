using ClaimsMicroservice.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClaimsMicroservice.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClaimsController
{
    private readonly List<Claim> claims = new()
    {
        new Claim
        {
            ClaimNumber = "C001",
            PolicyId = 1,
            Status = "Paid",
            AmountInPounds = 1000,
            CreationDate = new DateTime(2019, 1, 30)
        },
        new Claim
        {
            ClaimNumber = "C002",
            PolicyId = 1,
            Status = "Paid",
            AmountInPounds = 2000,
            CreationDate = new DateTime(2021, 2, 28)
        },
        new Claim
        {
            ClaimNumber = "C003",
            PolicyId = 2,
            Status = "Open",
            AmountInPounds = 1400,
            CreationDate = new DateTime(2023, 3, 1)
        },
        new Claim
        {
            ClaimNumber = "C004",
            PolicyId = 4,
            Status = "Open",
            AmountInPounds = 3000,
            CreationDate = new DateTime(2022, 4, 2)
        },
        new Claim
        {
            ClaimNumber = "C005",
            PolicyId = 6,
            Status = "Declined",
            AmountInPounds = 4000,
            CreationDate = new DateTime(2020, 5, 3)
        },
        new Claim
        {
            ClaimNumber = "C006",
            PolicyId = 7,
            Status = "Paid",
            AmountInPounds = 7200,
            CreationDate = new DateTime(2020, 6, 4)
        },
        new Claim
        {
            ClaimNumber = "C007",
            PolicyId = 7,
            Status = "Open",
            AmountInPounds = 8800,
            CreationDate = new DateTime(2023, 1, 11)
        },
        new Claim
        {
            ClaimNumber = "C008",
            PolicyId = 8,
            Status = "Paid",
            AmountInPounds = 200,
            CreationDate = new DateTime(2016, 10, 13)
        },
        new Claim
        {
            ClaimNumber = "C009",
            PolicyId = 8,
            Status = "Open",
            AmountInPounds = 18000,
            CreationDate = new DateTime(2023, 6, 11)
        }
    };

    [HttpGet]
    public IActionResult Get([FromQuery]int? policyId)
    {
        if (policyId == null)
        {
            return new OkObjectResult(claims);
        } else
        {
            return new OkObjectResult(claims.Where(c => c.PolicyId == policyId));
        }
    }

    [HttpGet("{claimNumber}")]
    public IActionResult Get(string? claimNumber)
    {
        if (string.IsNullOrWhiteSpace(claimNumber))
        {
            return new BadRequestObjectResult("'claimNumber' must be provided.");
        }

        var claim = claims.FirstOrDefault(c => c.ClaimNumber == claimNumber);
        if (claim == null)
        {
            return new NotFoundResult();
        }

        return new OkObjectResult(claim);
    }
}