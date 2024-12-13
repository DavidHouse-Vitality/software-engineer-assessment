using System.Collections.Generic;
using System;
using System.Linq;
using HomeSearchAssessment.Clients;
using HomeSearchAssessment.Facades;
using Moq;
using Xunit;
using HomeSearchAssessment.Models;
using System.Threading.Tasks;

namespace HomeSearchAssessmentTest
{
    public class HomeSearchFacadeTests
    {
        #region Test Data

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
         * GIVEN a HomeSearchFacade with a mocked client
         * WHEN I call GetPoliciesByPostcode with an empty string
         * THEN no properties are returned to me
         */
        [Fact]
        public static async Task HomeSearchFacade_EmptySting_NoPropertiesReturned()
        {
            var clientMock = GetMockClient();
            var facade = new HomeSearchFacade(clientMock.Object);

            var policies = await facade.GetPoliciesByPostcode("");
            Assert.NotNull(policies);
            Assert.Empty(policies);
        }

        /*
         * GIVEN a HomeSearchFacade with a mocked client
         * WHEN I call GetPoliciesByPostcode with a postcode matching one or more properties
         * THEN the matching properties are returned to me
         */
        [Fact]
        public static async Task HomeSearchFacade_ValidPostcode_MatchingPropertiesReturned()
        {
            var clientMock = GetMockClient();
            var facade = new HomeSearchFacade(clientMock.Object);

            var policies = await facade.GetPoliciesByPostcode("BH141PA");
            Assert.NotNull(policies);
            Assert.Equal(2, policies.Count);
            Assert.True(policies.Exists(p => p.Id == 6));
            Assert.True(policies.Exists(p => p.Id == 7));
        }

        /*
         * GIVEN a HomeSearchFacade with a mocked client
         * WHEN I call GetPoliciesByPostcode with a postcode matching one or more properties
         * THEN the properties that don't match are not returned to me
         */
        [Fact]
        public static async Task HomeSearchFacade_ValidPostcode_MismatchingPropertiesNotReturned()
        {
            var clientMock = GetMockClient();
            var facade = new HomeSearchFacade(clientMock.Object);

            var policies = await facade.GetPoliciesByPostcode("NG549BQ");
            Assert.NotNull(policies);
            Assert.NotEmpty(policies);
            Assert.False(policies.Exists(p => p.Id == 6));
        }

        /*
         * GIVEN a HomeSearchFacade with a mocked client
         * WHEN I call GetPoliciesByPostcode with a postcode matching one or more properties
         * THEN the properties are returned in order with the most expensive first
         */
        [Fact]
        public static async Task HomeSearchFacade_ValidPostcode_PropertiesReturnedByHighestToLowestPrice()
        {
            var clientMock = GetMockClient();
            var facade = new HomeSearchFacade(clientMock.Object);

            var policies = await facade.GetPoliciesByPostcode("BH141PA");
            Assert.NotNull(policies);
            Assert.NotEmpty(policies);
            Assert.Equal(2, policies.Count);
            Assert.Equal(640000, policies.First().Property.PriceInPounds);
            Assert.Equal(620000, policies.Skip(1).First().Property.PriceInPounds);
        }

        /*
         * GIVEN a HomeSearchFacade with a mocked client
         * WHEN I call GetPoliciesByPostcode with a wildcard postcode matching one or more properties
         * THEN the matching properties are returned to me
         */
        [Fact]
        public static async Task HomeSearchFacade_ValidWildcardPostcode_MatchingPropertiesReturned()
        {
            var clientMock = GetMockClient();
            var facade = new HomeSearchFacade(clientMock.Object);

            var policies = await facade.GetPoliciesByPostcode("BH9****");
            Assert.NotNull(policies);
            Assert.Equal(2, policies.Count);
            Assert.True(policies.Exists(p => p.Property.Postcode == "BH943KV"));
            Assert.True(policies.Exists(p => p.Property.Postcode == "BH994MH"));
        }

        /*
         * GIVEN a HomeSearchFacade with a mocked client
         * WHEN I call GetPoliciesByPostcode with a wildcard postcode matching one or more properties
         * THEN the mismatching properties are not returned to me
         */
        [Fact]
        public static async Task HomeSearchFacade_ValidWildcardPostcode_MismatchingPropertiesNotReturned()
        {
            var clientMock = GetMockClient();
            var facade = new HomeSearchFacade(clientMock.Object);

            var policies = await facade.GetPoliciesByPostcode("BH9****");
            Assert.NotNull(policies);
            Assert.NotEmpty(policies);
            Assert.False(policies.Exists(p => p.Property.Postcode == "BH141PA"));
        }


        /*
         * GIVEN a HomeSearchFacade with a mocked client
         * WHEN I call GetPoliciesByPostcode with a postcode matching one or more properties without any associated claims
         * THEN no claim information is returned to me
         */
        [Fact]
        public static async Task HomeSearchFacade_ValidPostcodeWithoutClaims_NoClaimsReturned()
        {
            var clientMock = GetMockClient();
            var facade = new HomeSearchFacade(clientMock.Object);

            var policies = await facade.GetPoliciesByPostcode("TW987OF");
            Assert.NotNull(policies);
            Assert.NotEmpty(policies);
            Assert.False(policies.Exists(p => p.Claims.Any()));
        }


        /*
         * GIVEN a HomeSearchFacade with a mocked client
         * WHEN I call GetPoliciesByPostcode with a postcode matching one or more properties with an associated claim
         * THEN the relevant claim information is returned to me
         */
        [Fact]
        public static async Task HomeSearchFacade_ValidPostcodeWithClaim_RelevantClaimReturned()
        {
            var clientMock = GetMockClient();
            var facade = new HomeSearchFacade(clientMock.Object);

            var policies = await facade.GetPoliciesByPostcode("BH943KV");
            Assert.NotNull(policies);
            Assert.NotEmpty(policies);
            Assert.Single(policies.First().Claims);
            Assert.True(policies.First().Claims.Exists(c => c.ClaimNumber == "C001"));
        }


        /*
         * GIVEN a HomeSearchFacade with a mocked client
         * WHEN I call GetPoliciesByPostcode with a postcode matching one or more properties with multiple associated claims
         * THEN the relevant claim information is returned to me
         */
        [Fact]
        public static async Task HomeSearchFacade_ValidPostcodeWithMultipleClaims_RelevantClaimsReturned()
        {
            var clientMock = GetMockClient();
            var facade = new HomeSearchFacade(clientMock.Object);

            var policies = await facade.GetPoliciesByPostcode("TW270RE");
            Assert.NotNull(policies);
            Assert.NotEmpty(policies);
            Assert.Equal(2, policies.First().Claims.Count);
            Assert.True(policies.First().Claims.Exists(c => c.ClaimNumber == "C008"));
            Assert.True(policies.First().Claims.Exists(c => c.ClaimNumber == "C009"));
        }

        private static Mock<IHomeSearchClient> GetMockClient()
        {
            var clientMock = new Mock<IHomeSearchClient>();
            clientMock.Setup(c => c.GetPolicies()).Returns(new Task<List<Policy>>(() => TestPolicies));
            clientMock.Setup(c => c.GetPolicy(It.IsAny<int>())).Returns<int>(id => new Task<Policy>(() => TestPolicies.Find(p => p.Id == id)));
            clientMock.Setup(c => c.GetClaims()).Returns(new Task<List<Claim>>(() => TestClaims));
            clientMock.Setup(c => c.GetClaimsByPolicyId(It.IsAny<int>())).Returns<int>(policyId => new Task<List<Claim>>(() => TestClaims.FindAll(c => c.PolicyId == policyId)));
            clientMock.Setup(c => c.GetClaim(It.IsAny<string>())).Returns<string>(claimNumber => new Task<Claim>(() => TestClaims.Find(c => c.ClaimNumber == claimNumber)));
            return clientMock;
        }
    }
}
