using RestSharp;
using RestSharp.Serializers.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
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
        this.restClient = new RestClient(httpClient, configureSerialization: s => s.UseSystemTextJson(CreateSerializerOptions()));

    }

    private JsonSerializerOptions CreateSerializerOptions()
        => new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            Converters = { new JsonStringEnumConverter() },
            PropertyNameCaseInsensitive = true,
            AllowTrailingCommas = true,
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            RespectNullableAnnotations = true,
            WriteIndented = false,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };



    public async Task<RiskLevelResponse> GetRiskLevelAsync(Invoice invoice)
    {
        var restRequest = new RestRequest("/risklevel");
        restRequest.AddJsonBody(invoice);
        var response = await restClient.PostAsync<RiskLevelResponse>(restRequest);

        return response;
    }
}
