using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TechChallenge.Common.DTO;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TechChallenge.Services.InvoiceService.Endpoints;

public static class InvoiceApi
{
    private const string ROUTEGROUPINVOICE = "/invoice";
    public static WebApplication UseInvoiceApi(this WebApplication app) 
    {
        var invoiceGroup = app.MapGroup(ROUTEGROUPINVOICE)
                              .WithTags("Invoice Operations")
                              .AddFluentValidationFilter();
        AddEvaluateEndpoint(invoiceGroup);
        AddUploadEndpoint(invoiceGroup);

        return app;
    }

    private static void AddEvaluateEndpoint(RouteGroupBuilder gb)
    {
        gb.MapPost("evaluate", Evaluate)          
          .WithOpenApi()
          .WithSummary("Evaluate an Invoice");                  
    }

    private static async Task<Results<Ok, ValidationProblem>> Evaluate(Invoice invoice)
        => TypedResults.Ok();

    private static void AddUploadEndpoint(RouteGroupBuilder gb)
    {
        gb.MapPost("upload", Upload)          
          .WithOpenApi()
          .WithSummary("Upload a Invoice document");
    }

    private static async Task<Results<Ok, ValidationProblem>> Upload(int invoiceId, IFormFile formFile)
        => TypedResults.Ok();
}
