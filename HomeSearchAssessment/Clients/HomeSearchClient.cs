using HomeSearchAssessment.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace HomeSearchAssessment.Clients;

public class HomeSearchClient : IHomeSearchClient
{
    private readonly HttpClient _httpClient;
    private readonly string _policiesMicroserviceUrl;
    private readonly string _claimsMicroserviceUrl;
    private readonly JsonSerializerOptions _serializerOptions;

    public HomeSearchClient(HttpClient httpClient, string policiesMicroserviceUrl, string claimsMicroserviceUrl)
    {
        _httpClient = httpClient;
        _policiesMicroserviceUrl = policiesMicroserviceUrl;
        _claimsMicroserviceUrl = claimsMicroserviceUrl;
        _serializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
    }

    public async Task<List<Policy>> GetPolicies()
    {
        return null;
    }

    public async Task<Policy> GetPolicy(int id)
    {
        return null;
    }

    public async Task<List<Claim>> GetClaims()
    {
        return null;
    }

    public async Task<List<Claim>> GetClaimsByPolicyId(int policyId)
    {
        return null;
    }

    public async Task<Claim> GetClaim(string claimNumber)
    {
        return null;
    }
}