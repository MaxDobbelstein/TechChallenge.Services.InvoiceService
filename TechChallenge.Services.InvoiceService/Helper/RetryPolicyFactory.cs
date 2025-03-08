using TechChallenge.Services.InvoiceService.Constants;

namespace TechChallenge.Services.InvoiceService.Helper;

public static class RetryPolicyFactory
{
    public static Dictionary<string, TimeSpan[]> CreateRetryPolicies()
    { 
        var retryPolicies = new Dictionary<string, TimeSpan[]>();
        retryPolicies.Add(HttpClientPolicyConstants.CONSTANTPOLICY, CreateConstantPolicy());
        retryPolicies.Add(HttpClientPolicyConstants.LINEARPOLICY, CreateLinearPolicy());
        retryPolicies.Add(HttpClientPolicyConstants.EXPONENTIALPOLICY, CreateExponentialPolicy());

        return retryPolicies;
    }

    private static TimeSpan[] CreateConstantPolicy()
        => [TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5)];

    private static TimeSpan[] CreateLinearPolicy()
        => [TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(15)];

    private static TimeSpan[] CreateExponentialPolicy()
        => [TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(4), TimeSpan.FromSeconds(8), TimeSpan.FromSeconds(16)];
}
