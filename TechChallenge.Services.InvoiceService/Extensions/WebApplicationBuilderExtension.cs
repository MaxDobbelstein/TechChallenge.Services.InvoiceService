using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Polly;
using Serilog;
using System.ComponentModel;
using TechChallenge.Common.DTO.Contexts;
using TechChallenge.Services.InvoiceService.Handler;
using TechChallenge.Services.InvoiceService.Helper;
using TechChallenge.Services.InvoiceService.Interfaces;
using TechChallenge.Services.InvoiceService.Options;
using TechChallenge.Services.InvoiceService.ServiceClient;
using TechChallenge.Services.InvoiceService.Validation;
using FileOptions = TechChallenge.Services.InvoiceService.Options.FileOptions;

namespace TechChallenge.Services.InvoiceService.Extensions;

public static class WebApplicationBuilderExtension
{
    public static WebApplicationBuilder AddInvoiceService(this WebApplicationBuilder builder)
    {
        AddSwagger(builder);
        AddLogging(builder);
        AddValidation(builder);
        AddServiceClients(builder);
        AddConfiguration(builder);
        AddHandler(builder);
        AddRepositories(builder);
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

    private static void AddConfiguration(WebApplicationBuilder builder)
    {
        builder.Services.Configure<FileOptions>(builder.Configuration.GetSection(FileOptions.CONFIGURATIONSECTION));
    }

    private static void AddHandler(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IInvoiceHandler, InvoiceHandler>();
    }

    private static void AddRepositories(WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<InvoiceContext>(options => options.UseSqlite(builder.Configuration["ConnectionStrings:InvoiceConnection"]), ServiceLifetime.Scoped);
    }

}
