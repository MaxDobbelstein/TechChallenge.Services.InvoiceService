using Microsoft.Extensions.Logging;
using TechChallenge.Common.DTO.Contexts;
using TechChallenge.Common.Repositories;
using TechChallenge.Common.Repositories.Interfaces;

namespace TechChallenge.Services.InvoiceService.Tests.Factories;

public static class RepositoryFactory
{
    private static LoggerFactory loggerFactory = new LoggerFactory();

    public static IInvoiceRepository CreateInvoiceRepository(InvoiceContext context) 
    {
        var invoiceRepository = new InvoiceRepository(loggerFactory.CreateLogger<InvoiceRepository>(), context);
        return invoiceRepository;
    }

}
