

using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using TechChallenge.Common.DTO;

namespace TechChallenge.Services.InvoiceService.Endpoints;

public static class InvoiceApi
{
    public static WebApplication UseInvoiceApi(this WebApplication app) 
    {
        AddEvaluateEndpoint(app);
        return app;
    }

    private static void AddEvaluateEndpoint(WebApplication app)
    {
        app.MapPost("evaluate", Evaluate)
           .WithTags("Invoice Evaluation")
           .WithOpenApi()
           .WithSummary("Evaluate an Invoice");

    }

    private static async Task<Results<Ok, ValidationProblem>> Evaluate(Invoice invoice, IValidator<Invoice> validator)
    {
        var validationResult = await validator.ValidateAsync(invoice);
        if (!validationResult.IsValid)
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        
        return TypedResults.Ok();
    }
}
