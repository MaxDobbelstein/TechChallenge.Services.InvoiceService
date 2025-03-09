using RestSharp;
using TechChallenge.Common.DTO;
using TechChallenge.Services.InvoiceService.Interfaces;

namespace TechChallenge.Services.InvoiceService.ServiceClient;

public class RiskLevelServiceClient : IRiskLevelServiceClient
{
    private readonly ILogger<RiskLevelServiceClient> logger;
    private readonly IRestClient restClient;

    public RiskLevelServiceClient(ILogger<RiskLevelServiceClient> logger, HttpClient httpClient)
    {
        this.logger = logger;
        this.restClient = new RestClient(httpClient);
    }

    public async Task<RiskLevelResponse> GetRiskLevelAsync(Invoice invoice)
    { 
        var restRequest = new RestRequest("/risklevel");
        restRequest.AddJsonBody(invoice);
        var response = await restClient.PostAsync<RiskLevelResponse>(restRequest);

        return response;
    }
}
