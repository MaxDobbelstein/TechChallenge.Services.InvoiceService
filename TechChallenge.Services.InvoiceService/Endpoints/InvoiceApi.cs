using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TechChallenge.Common.DTO;
using TechChallenge.Services.InvoiceService.Interfaces;
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
        AddCreateEndpoint(invoiceGroup);
        AddUploadEndpoint(invoiceGroup);
        AddEvaluateEndpoint(invoiceGroup);
        return app;
    }

    private static void AddCreateEndpoint(RouteGroupBuilder gb)
    {
        gb.MapPost("create", Create)
          .WithOpenApi()
          .WithSummary("Create an Invoice");
    }

    private static async Task<Results<Ok, ValidationProblem>> Create(Invoice invoice, IInvoiceHandler invoiceHandler)
        => TypedResults.Ok();

    private static void AddUploadEndpoint(RouteGroupBuilder gb)
    {
        gb.MapPost("upload", Upload)
          .WithOpenApi()
          .WithSummary("Upload an Invoice document");
    }

    private static async Task<Results<Ok, ValidationProblem>> Upload(int invoiceId, IFormFile document, IInvoiceHandler invoiceHandler)
    { 

        //var file = document.CopyToAsync()
        return TypedResults.Ok();
    }

    private static void AddEvaluateEndpoint(RouteGroupBuilder gb)
    {
        gb.MapPut("evaluate/invoiceid/{invoiceId}", Evaluate)
          .WithOpenApi()
          .WithSummary("Evaluate an Invoice");
    }

    private static async Task Evaluate(long invoiceId, IInvoiceHandler invoiceHandler)
    {
        throw new NotImplementedException();
    }
}
