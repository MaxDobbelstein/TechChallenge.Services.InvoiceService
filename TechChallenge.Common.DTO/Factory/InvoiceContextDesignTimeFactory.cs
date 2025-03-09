using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using TechChallenge.Common.DTO.Contexts;

namespace TechChallenge.Common.DTO.Factory;

public class InvoiceContextDesignTimeFactory : IDesignTimeDbContextFactory<InvoiceContext>
{
    public InvoiceContext CreateDbContext(string[] args)
    {
        //TODO: make configurable
        var options = new DbContextOptionsBuilder<InvoiceContext>().UseSqlite("Data Source=c:/invoices/sqlite/invoice.db").Options;
        return new InvoiceContext(options);
    }
}
