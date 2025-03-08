using TechChallenge.Common.DTO;
using TechChallenge.Common.DTO.RiskLevelApi;

namespace TechChallenge.Services.InvoiceService.Interfaces;
public interface IRiskLevelServiceClient
{
    Task<RiskLevelResponse> GetRiskLevelAsync(Invoice invoice);
}