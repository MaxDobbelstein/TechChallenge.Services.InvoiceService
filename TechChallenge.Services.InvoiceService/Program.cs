using TechChallenge.Services.InvoiceService.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.AddInvoiceService();

var app = builder.Build();
app.UseInvoiceService();
app.Run();

