using TechChallenge.Common.DTO.Enums;

namespace TechChallenge.Common.DTO;

public class EvaluationResponse
{
    public string EvaluationId { get; set; }
    public long InvoiceId { get; set; } 
    public Classification Classification { get; set; }
}
