using FluentValidation;
using Serilog;
using TechChallenge.Services.InvoiceService.Validation;

namespace TechChallenge.Services.InvoiceService.Extensions;

public static class WebApplicationBuilderExtension
{
    public static WebApplicationBuilder AddInvoiceService(this WebApplicationBuilder builder)
    {
        AddSwagger(builder);
        AddLogging(builder);
        AddValidation(builder);
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
        => builder.Services.AddValidatorsFromAssemblyContaining<InvoiceValidator>();
    

}
