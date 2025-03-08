namespace TechChallenge.Services.InvoiceService.Endpoints;

public static class SwaggerApi
{
    public static WebApplication UseSwaggerApi(this WebApplication app) 
    {
        //Anmerkung: Swagger immer generieren, um ggf in einer development Umgebung darauf zu, zu greifen. 
        app.UseSwagger();
        if (app.Environment.IsDevelopment())
            app.UseSwaggerUI();
        
        return app;
    }
}
