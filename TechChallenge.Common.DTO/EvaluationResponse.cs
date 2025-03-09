using TechChallenge.Common.DTO.Enums;

namespace TechChallenge.Common.DTO;

public class EvaluationResponse
{
    public string EvaluationId { get; set; }
    public long InvoiceId { get; set; } 
    public Classification Classification { get; set; }
    public RiskLevel RiskLevel { get; set; }
    public Evaluation Evaluation { get; set; }
    public string EvaluationReason { get; set; }
}
