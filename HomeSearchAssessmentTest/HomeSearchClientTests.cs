using System.Collections.Generic;
using System;
using HomeSearchAssessment.Clients;
using Moq;
using Xunit;
using HomeSearchAssessment.Models;
using System.Threading.Tasks;
using System.Net.Http;
using Moq.Protected;
using System.Net;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using System.Linq;
using System.Threading;

namespace HomeSearchAssessmentTest;

public class HomeSearchClientTests
{
    #region Test Data

    private const string PoliciesMicroserviceUrl = "https://test.com/api/policies";
    private const string ClaimsMicroserviceUrl = "https://test.com/api/claims";

    private static readonly List<Policy> TestPolicies = new()
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

    private static readonly List<Claim> TestClaims = new()
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

    #endregion

    /*
     * GIVEN a HomeSearchClient
     * WHEN I call the GetPolicies method
     * THEN all the policies are returned
     */
    [Fact]
    public static async Task HomeSearchClient_GetPolicies_AllPoliciesReturned()
    {
        var httpClientMock = GetMockHttpClient();
        var client = new HomeSearchClient(httpClientMock, PoliciesMicroserviceUrl, ClaimsMicroserviceUrl);

        var policiesResult = await client.GetPolicies();
        Assert.NotNull(policiesResult);
        Assert.Equal(TestPolicies.Count, policiesResult.Count);

        for (var i = 0; i < TestPolicies.Count; i++)
        {
            Assert.Equal(TestPolicies[i].Id, policiesResult[i].Id);
            Assert.Equal(TestPolicies[i].Property.Postcode, policiesResult[i].Property.Postcode);
        }
    }

    /*
     * GIVEN a HomeSearchClient
     * WHEN I call the GetPolicy method with a valid policy id
     * THEN the matching policy is returned
     */
    [Fact]
    public static async Task HomeSearchClient_ValidGetPolicy_MatchingPolicyReturned()
    {
        var httpClientMock = GetMockHttpClient();
        var client = new HomeSearchClient(httpClientMock, PoliciesMicroserviceUrl, ClaimsMicroserviceUrl);

        var policyResult = await client.GetPolicy(1);
        Assert.NotNull(policyResult);

        var testPolicy = TestPolicies.Single(p => p.Id == 1);
        Assert.Equal(testPolicy.Id, policyResult.Id);
        Assert.Equal(testPolicy.Property.Postcode, policyResult.Property.Postcode);
        Assert.True(policyResult.Id == 1);
    }

    /*
     * GIVEN a HomeSearchClient
     * WHEN I call the GetPolicy method with an invalid policy id
     * THEN no policy is returned
     */
    [Fact]
    public static async Task HomeSearchClient_InvalidGetPolicy_NoPolicyReturned()
    {
        var httpClientMock = GetMockHttpClient();
        var client = new HomeSearchClient(httpClientMock, PoliciesMicroserviceUrl, ClaimsMicroserviceUrl);

        var policyResult = await client.GetPolicy(10);
        Assert.Null(policyResult);
    }

    /*
     * GIVEN a HomeSearchClient
     * WHEN I call the GetClaims method
     * THEN all the claims are returned
     */
    [Fact]
    public static async Task HomeSearchClient_GetClaims_AllClaimsReturned()
    {
        var httpClientMock = GetMockHttpClient();
        var client = new HomeSearchClient(httpClientMock, PoliciesMicroserviceUrl, ClaimsMicroserviceUrl);

        var claimsResult = await client.GetClaims();
        Assert.NotNull(claimsResult);
        Assert.Equal(TestClaims.Count, claimsResult.Count);

        for (var i = 0; i < TestClaims.Count; i++)
        {
            Assert.Equal(TestClaims[i].ClaimNumber, claimsResult[i].ClaimNumber);
        }
    }

    /*
     * GIVEN a HomeSearchClient
     * WHEN I call the GetClaimsByPolicyId method with a valid policy id
     * THEN the matching claims are returned
     */
    [Fact]
    public static async Task HomeSearchClient_ValidGetClaimsByPolicyId_MatchingClaimsReturned()
    {
        var httpClientMock = GetMockHttpClient();
        var client = new HomeSearchClient(httpClientMock, PoliciesMicroserviceUrl, ClaimsMicroserviceUrl);

        var claimsResult = await client.GetClaimsByPolicyId(7);
        Assert.NotNull(claimsResult);
        Assert.Equal(2, claimsResult.Count);
        Assert.True(claimsResult.Exists(c => c.ClaimNumber == "C006"));
        Assert.True(claimsResult.Exists(c => c.ClaimNumber == "C007"));
    }

    /*
     * GIVEN a HomeSearchClient
     * WHEN I call the GetClaimsByPolicyId method with an invalid policy id
     * THEN no claims are returned
     */
    [Fact]
    public static async Task HomeSearchClient_InvalidGetClaimsByPolicyId_NoClaimsReturned()
    {
        var httpClientMock = GetMockHttpClient();
        var client = new HomeSearchClient(httpClientMock, PoliciesMicroserviceUrl, ClaimsMicroserviceUrl);

        var claimsResult = await client.GetClaimsByPolicyId(10);
        Assert.NotNull(claimsResult);
        Assert.Empty(claimsResult);
    }

    /*
     * GIVEN a HomeSearchClient
     * WHEN I call the GetClaim method with a valid claim number
     * THEN the matching claim is returned
     */
    [Fact]
    public static async Task HomeSearchClient_ValidGetClaim_MatchingClaimReturned()
    {
        var httpClientMock = GetMockHttpClient();
        var client = new HomeSearchClient(httpClientMock, PoliciesMicroserviceUrl, ClaimsMicroserviceUrl);

        var claimResult = await client.GetClaim("C001");
        Assert.NotNull(claimResult);
        Assert.Equal(TestClaims.Single(c => c.ClaimNumber == "C001").ClaimNumber, claimResult.ClaimNumber);
    }

    /*
     * GIVEN a HomeSearchClient
     * WHEN I call the GetClaim method with an invalid claim number
     * THEN no claim is returned
     */
    [Fact]
    public static async Task HomeSearchClient_InvalidGetClaim_NoClaimReturned()
    {
        var httpClientMock = GetMockHttpClient();
        var client = new HomeSearchClient(httpClientMock, PoliciesMicroserviceUrl, ClaimsMicroserviceUrl);

        var claimResult = await client.GetClaim("C010");
        Assert.Null(claimResult);
    }

    private static HttpClient GetMockHttpClient()
    {
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();

        mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(m => m.RequestUri.ToString() == PoliciesMicroserviceUrl), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(TestPolicies)
            });

        mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(m => Regex.IsMatch(m.RequestUri.ToString(), $"^{PoliciesMicroserviceUrl}/\\d+$")), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync((HttpRequestMessage _, CancellationToken _) => new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound
            });

        mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(m => Regex.IsMatch(m.RequestUri.ToString(), $"^{PoliciesMicroserviceUrl}/[1-9]$")), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync((HttpRequestMessage request, CancellationToken _) =>
            {
                var policyId = int.Parse(request.RequestUri?.ToString().Split('/').Last() ?? string.Empty);

                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(TestPolicies.Single(p => p.Id == policyId))
                };
            });

        mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(m => m.RequestUri.ToString() == ClaimsMicroserviceUrl), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(TestClaims)
            });

        mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(m => Regex.IsMatch(m.RequestUri.ToString(), $"^{ClaimsMicroserviceUrl}\\?policyId=\\d+$")), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync((HttpRequestMessage _, CancellationToken _) => new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound
            });

        mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(m => Regex.IsMatch(m.RequestUri.ToString(), $"^{ClaimsMicroserviceUrl}\\?policyId=[1-9]$")), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync((HttpRequestMessage request, CancellationToken _) =>
            {
                var policyId = int.Parse(request.RequestUri?.ToString().Split('=').Last() ?? string.Empty);

                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(TestClaims.Where(c => c.PolicyId == policyId))
                };
            });

        mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(m => Regex.IsMatch(m.RequestUri.ToString(), $"^{ClaimsMicroserviceUrl}/[a-zA-Z0-9]+$")), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync((HttpRequestMessage _, CancellationToken _) => new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound
            });

        mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(m => Regex.IsMatch(m.RequestUri.ToString(), $"^{ClaimsMicroserviceUrl}/C00[1-9]$")), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync((HttpRequestMessage request, CancellationToken _) =>
            {
                var claimNumber = request.RequestUri?.ToString().Split('/').Last();

                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(TestClaims.Single(c => c.ClaimNumber == claimNumber))
                };
            });

        return new HttpClient(mockHttpMessageHandler.Object);
    }
}