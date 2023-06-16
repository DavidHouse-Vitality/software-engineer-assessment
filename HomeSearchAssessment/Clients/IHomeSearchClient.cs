using HomeSearchAssessment.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeSearchAssessment.Clients
{
    public interface IHomeSearchClient
    {
        public Task<List<Policy>> GetPolicies();

        public Task<Policy> GetPolicy(int id);

        public Task<List<Claim>> GetClaims();

        public Task<List<Claim>> GetClaimsByPolicyId(int policyId);

        public Task<Claim> GetClaim(string claimNumber);
    }
}
