using Microsoft.EntityFrameworkCore;

namespace TechChallenge.Common.DTO.Contexts;

public class InvoiceContext : DbContext
{
    public DbSet<Invoice> Invoices { get; set; }

    public InvoiceContext(DbContextOptions<InvoiceContext> options) : base(options) { }

    protected InvoiceContext() { }
}
