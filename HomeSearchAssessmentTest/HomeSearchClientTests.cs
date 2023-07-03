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

namespace HomeSearchAssessmentTest
{
    public class HomeSearchClientTests
    {
        #region Test Data

        private static readonly string policiesMicroserviceUrl = @"https://test.com/api/policies";
        private static readonly string claimsMicroserviceUrl = @"https://test.com/api/claims";

        private static readonly List<Policy> testPolicies = new()
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

        private static readonly List<Claim> testClaims = new()
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
        public static void HomeSearchClient_GetPolicies_AllPoliciesReturned()
        {
            var httpClientMock = GetMockHttpClient();
            var client = new HomeSearchClient(httpClientMock, policiesMicroserviceUrl, claimsMicroserviceUrl);

            var policiesResult = client.GetPolicies().Result;
            Assert.NotNull(policiesResult);
            Assert.Equal(testPolicies.Count, policiesResult.Count);

            for (var i = 0; i < testPolicies.Count; i++)
            {
                Assert.Equal(testPolicies[i].Id, policiesResult[i].Id);
                Assert.Equal(testPolicies[i].Property.Postcode, policiesResult[i].Property.Postcode);
            }
        }

        /*
         * GIVEN a HomeSearchClient
         * WHEN I call the GetPolicy method with a valid policy id
         * THEN the matching policy is returned
         */
        [Fact]
        public static void HomeSearchClient_ValidGetPolicy_MatchingPolicyReturned()
        {
            var httpClientMock = GetMockHttpClient();
            var client = new HomeSearchClient(httpClientMock, policiesMicroserviceUrl, claimsMicroserviceUrl);

            var policyResult = client.GetPolicy(1).Result;
            Assert.NotNull(policyResult);

            var testPolicy = testPolicies.Single(p => p.Id == 1);
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
        public static void HomeSearchClient_InvalidGetPolicy_NoPolicyReturned()
        {
            var httpClientMock = GetMockHttpClient();
            var client = new HomeSearchClient(httpClientMock, policiesMicroserviceUrl, claimsMicroserviceUrl);

            var policyResult = client.GetPolicy(10).Result;
            Assert.Null(policyResult);
        }

        /*
         * GIVEN a HomeSearchClient
         * WHEN I call the GetClaims method
         * THEN all the claims are returned
         */
        [Fact]
        public static void HomeSearchClient_GetClaims_AllClaimsReturned()
        {
            var httpClientMock = GetMockHttpClient();
            var client = new HomeSearchClient(httpClientMock, policiesMicroserviceUrl, claimsMicroserviceUrl);

            var claimsResult = client.GetClaims().Result;
            Assert.NotNull(claimsResult);
            Assert.Equal(testClaims.Count, claimsResult.Count);

            for (var i = 0; i < testClaims.Count; i++)
            {
                Assert.Equal(testClaims[i].ClaimNumber, claimsResult[i].ClaimNumber);
            }
        }

        /*
         * GIVEN a HomeSearchClient
         * WHEN I call the GetClaimsByPolicyId method with a valid policy id
         * THEN the matching claims are returned
         */
        [Fact]
        public static void HomeSearchClient_ValidGetClaimsByPolicyId_MatchingClaimsReturned()
        {
            var httpClientMock = GetMockHttpClient();
            var client = new HomeSearchClient(httpClientMock, policiesMicroserviceUrl, claimsMicroserviceUrl);

            var claimsResult = client.GetClaimsByPolicyId(7).Result;
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
        public static void HomeSearchClient_InvalidGetClaimsByPolicyId_NoClaimsReturned()
        {
            var httpClientMock = GetMockHttpClient();
            var client = new HomeSearchClient(httpClientMock, policiesMicroserviceUrl, claimsMicroserviceUrl);

            var claimsResult = client.GetClaimsByPolicyId(10).Result;
            Assert.NotNull(claimsResult);
            Assert.Empty(claimsResult);
        }

        /*
         * GIVEN a HomeSearchClient
         * WHEN I call the GetClaim method with a valid claim number
         * THEN the matching claim is returned
         */
        [Fact]
        public static void HomeSearchClient_ValidGetClaim_MatchingClaimReturned()
        {
            var httpClientMock = GetMockHttpClient();
            var client = new HomeSearchClient(httpClientMock, policiesMicroserviceUrl, claimsMicroserviceUrl);

            var claimResult = client.GetClaim("C001").Result;
            Assert.NotNull(claimResult);
            Assert.Equal(testClaims.Single(c => c.ClaimNumber == "C001").ClaimNumber, claimResult.ClaimNumber);
        }

        /*
         * GIVEN a HomeSearchClient
         * WHEN I call the GetClaim method with an invalid claim number
         * THEN no claim is returned
         */
        [Fact]
        public static void HomeSearchClient_InvalidGetClaim_NoClaimReturned()
        {
            var httpClientMock = GetMockHttpClient();
            var client = new HomeSearchClient(httpClientMock, policiesMicroserviceUrl, claimsMicroserviceUrl);

            var claimResult = client.GetClaim("C010").Result;
            Assert.Null(claimResult);
        }

        private static HttpClient GetMockHttpClient()
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(m => m.RequestUri.ToString() == policiesMicroserviceUrl), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(testPolicies)
                });

            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(m => Regex.IsMatch(m.RequestUri.ToString(), $"^{policiesMicroserviceUrl}/\\d+$")), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync((HttpRequestMessage request, CancellationToken cancellationToken) =>
                {
                    var policyId = int.Parse(request.RequestUri.ToString().Split('/').Last());

                    return new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.NotFound
                    };
                });

            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(m => Regex.IsMatch(m.RequestUri.ToString(), $"^{policiesMicroserviceUrl}/[1-9]$")), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync((HttpRequestMessage request, CancellationToken cancellationToken) =>
                {
                    var policyId = int.Parse(request.RequestUri.ToString().Split('/').Last());

                    return new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = JsonContent.Create(testPolicies.Single(p => p.Id == policyId))
                    };
                });

            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(m => m.RequestUri.ToString() == claimsMicroserviceUrl), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(testClaims)
                });

            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(m => Regex.IsMatch(m.RequestUri.ToString(), $"^{claimsMicroserviceUrl}?policyId=\\d+$")), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync((HttpRequestMessage request, CancellationToken cancellationToken) =>
                {
                    var policyId = int.Parse(request.RequestUri.ToString().Split('=').Last());

                    return new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.NotFound
                    };
                });

            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(m => Regex.IsMatch(m.RequestUri.ToString(), $"^{claimsMicroserviceUrl}?policyId=[1-9]$")), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync((HttpRequestMessage request, CancellationToken cancellationToken) =>
                {
                    var policyId = int.Parse(request.RequestUri.ToString().Split('=').Last());

                    return new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = JsonContent.Create(testClaims.Where(c => c.PolicyId == policyId))
                    };
                });

            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(m => Regex.IsMatch(m.RequestUri.ToString(), $"^{claimsMicroserviceUrl}/[a-zA-Z0-9]+$")), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync((HttpRequestMessage request, CancellationToken cancellationToken) =>
                {
                    var claimNumber = request.RequestUri.ToString().Split('/').Last();

                    return new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.NotFound
                    };
                });

            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(m => Regex.IsMatch(m.RequestUri.ToString(), $"^{claimsMicroserviceUrl}/C00[1-9]$")), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync((HttpRequestMessage request, CancellationToken cancellationToken) =>
                {
                    var claimNumber = request.RequestUri.ToString().Split('/').Last();

                    return new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = JsonContent.Create(testClaims.Single(c => c.ClaimNumber == claimNumber))
                    };
                });

            return new HttpClient(mockHttpMessageHandler.Object);
        }
    }
}
