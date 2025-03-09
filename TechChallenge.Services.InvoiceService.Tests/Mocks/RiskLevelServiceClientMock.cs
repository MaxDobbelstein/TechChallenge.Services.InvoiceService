using TechChallenge.Common.DTO;
using TechChallenge.Services.InvoiceService.Interfaces;

namespace TechChallenge.Services.InvoiceService.Tests.Mocks;

public class RiskLevelServiceClientMock : IRiskLevelServiceClient
{
    public async Task<RiskLevelResponse> GetRiskLevelAsync(Invoice invoice)
    {
        var riskLevelResponse = new RiskLevelResponse();
        riskLevelResponse.RiskLevel = Common.DTO.Enums.RiskLevel.High;
        riskLevelResponse.Classification = Common.DTO.Enums.Classification.WaterLeakDetection;

        return riskLevelResponse;
    }
}
