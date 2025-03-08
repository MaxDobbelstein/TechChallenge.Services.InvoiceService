using FluentValidation;
using Polly;
using Serilog;
using TechChallenge.Services.InvoiceService.Helper;
using TechChallenge.Services.InvoiceService.Interfaces;
using TechChallenge.Services.InvoiceService.Options;
using TechChallenge.Services.InvoiceService.ServiceClient;
using TechChallenge.Services.InvoiceService.Validation;

namespace TechChallenge.Services.InvoiceService.Extensions;

public static class WebApplicationBuilderExtension
{
    public static WebApplicationBuilder AddInvoiceService(this WebApplicationBuilder builder)
    {
        AddSwagger(builder);
        AddLogging(builder);
        AddValidation(builder);
        AddServiceClients(builder);
        return builder;
    }

    private static void AddSwagger(WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
    }

    private static void AddLogging(WebApplicationBuilder builder)
    {
        builder.Logging
        .ClearProviders()
        .AddSerilog(new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .CreateLogger());
    }

    private static void AddValidation(WebApplicationBuilder builder)
    {
        builder.AddFluentValidationEndpointFilter();
        builder.Services.AddValidatorsFromAssemblyContaining<InvoiceValidator>();
    }     
    
    private static void AddServiceClients(WebApplicationBuilder builder) 
    {
        var retryPolicyTimeSpans = RetryPolicyFactory.CreateRetryPolicies();
        var riskLevelServiceOptions = new RiskLevelServiceOptions();

        builder.Configuration.GetSection(RiskLevelServiceOptions.CONFIGURATIONSECTION).Bind(riskLevelServiceOptions);

        if (!retryPolicyTimeSpans.ContainsKey(riskLevelServiceOptions.RetryPolicy))
            throw new ArgumentOutOfRangeException("Unknown Retry Policy configured");

        var configuredRetries = retryPolicyTimeSpans[riskLevelServiceOptions.RetryPolicy];
        builder.Services
               .AddHttpClient<IRiskLevelServiceClient, RiskLevelServiceClient>(opt => { opt.BaseAddress = new Uri(riskLevelServiceOptions.BaseUrl); })
               .AddTransientHttpErrorPolicy(retryBuilder => retryBuilder.WaitAndRetryAsync(configuredRetries));


    }
}
