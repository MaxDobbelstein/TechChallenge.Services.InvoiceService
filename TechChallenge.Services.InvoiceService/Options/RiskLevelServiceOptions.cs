namespace TechChallenge.Services.InvoiceService.Options;

public class RiskLevelServiceOptions
{
    public const string CONFIGURATIONSECTION = "RiskLevelServiceOptions";
    public string BaseUrl { get; set; }
    public string RetryPolicy { get; set; }
}
