using TechChallenge.Common.DTO.Enums;

namespace TechChallenge.Common.DTO;

public class RiskLevelResponse
{
    public Classification Classification { get; set; }
    public RiskLevel RiskLevel { get; set; }
}
