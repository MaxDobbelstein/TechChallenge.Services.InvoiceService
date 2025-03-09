using Microsoft.Extensions.Logging;
using TechChallenge.Common.DTO;
using TechChallenge.Common.DTO.Contexts;
using TechChallenge.Common.Repositories.Interfaces;

namespace TechChallenge.Common.Repositories;

public class InvoiceRepository : IInvoiceRepository
{
    private readonly ILogger<InvoiceRepository> logger;
    private readonly InvoiceContext context;

    public InvoiceRepository(ILogger<InvoiceRepository> logger, InvoiceContext context)
    {
        this.logger = logger;
        this.context = context;
    }

    public bool Exists(long invoiceId)
      => context.Invoices.FirstOrDefault(x => x.Id == invoiceId) != null;

    public Invoice Find(long invoiceId)
        => context.Invoices.FirstOrDefault(x => x.Id == invoiceId);

    public long Insert(Invoice invoice)
    {
        context.Add(invoice);
        context.SaveChanges();
        return invoice.Id;
    }

}
