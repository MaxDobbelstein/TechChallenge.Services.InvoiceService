using TechChallenge.Common.DTO;

namespace TechChallenge.Services.InvoiceService.Interfaces;
public interface IRiskLevelServiceClient
{
    Task<RiskLevelResponse> GetRiskLevelAsync(Invoice invoice);
}