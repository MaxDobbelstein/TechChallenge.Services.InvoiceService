using Microsoft.AspNetCore.Http.HttpResults;
using TechChallenge.Common.DTO;
using TechChallenge.Services.InvoiceService.Interfaces;

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

    private static async Task<Results<Created<long>, ValidationProblem, ProblemHttpResult, BadRequest<string>>> Create(Invoice invoice, IInvoiceHandler invoiceHandler)
    {
        try
        {
            var invoiceId = invoiceHandler.CreateInvoice(invoice);

            return TypedResults.Created("/invoice/upload", invoiceId);
        }
        catch(ArgumentException ex) 
        {
            return TypedResults.BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return TypedResults.Problem(ex.Message);
        }
    }

    private static void AddUploadEndpoint(RouteGroupBuilder gb)
    {
        gb.MapPost("upload", Upload)
          //.WithOpenApi()
          .WithSummary("Upload an Invoice document")
          .DisableAntiforgery(); //Disabling Antiforgery for simplicity reasons
    }

    private static async Task<Results<Created, ValidationProblem, ProblemHttpResult, BadRequest<string>>> Upload(int invoiceId, IFormFile document, IInvoiceHandler invoiceHandler)
    {
        try
        {
            var memoryStream = new MemoryStream();
            await document.CopyToAsync(memoryStream);
            await invoiceHandler.SaveDocument(invoiceId, memoryStream);            
            return TypedResults.Created($"/invoice/evaluate/invoice/{invoiceId}");
        }
        catch (ArgumentException ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return TypedResults.Problem(ex.Message);
        }
    }

    private static void AddEvaluateEndpoint(RouteGroupBuilder gb)
    {
        gb.MapPut("evaluate/invoice/{invoiceId}", Evaluate)
          .WithOpenApi()
          .WithSummary("Evaluate an Invoice");
    }

    private static async Task<Results<Ok<EvaluationResponse>, ProblemHttpResult, BadRequest<string>>> Evaluate(long invoiceId, IInvoiceHandler invoiceHandler)
    {
        try 
        {
            var response = await invoiceHandler.Evaluate(invoiceId);

            return TypedResults.Ok(response);
        }
        catch (ArgumentException ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
        catch (Exception ex) 
        {
            return TypedResults.Problem(ex.Message);
        }
    }
}
