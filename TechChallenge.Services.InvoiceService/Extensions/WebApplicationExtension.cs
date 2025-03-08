using TechChallenge.Services.InvoiceService.Endpoints;

namespace TechChallenge.Services.InvoiceService.Extensions;

public static class WebApplicationExtension
{
    public static WebApplication UseInvoiceService(this WebApplication app) 
        => app.UseSwaggerApi().UseInvoiceApi();
    
}
