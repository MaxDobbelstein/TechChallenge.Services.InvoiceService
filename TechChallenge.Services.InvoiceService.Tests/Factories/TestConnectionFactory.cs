using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TechChallenge.Common.DTO.Contexts;

namespace TechChallenge.Services.InvoiceService.Tests.Factories;

public static class TestConnectionFactory
{
    public static InvoiceContext CreateInvoiceContext(bool shared = false, string name = "invoicedb")
    {
        var connection = CreateSqliteConnection(shared, name);
        var optionBuilder = new DbContextOptionsBuilder<InvoiceContext>();
        optionBuilder.UseSqlite(connection);
        optionBuilder.EnableSensitiveDataLogging();
        var option = optionBuilder.Options;
        var context = new InvoiceContext(option);
        EnsureCreated(context, shared);
        
        return context;
    }

    private static SqliteConnection CreateSqliteConnection(bool shared, string name)
    {
        var connection = new SqliteConnection(shared ? "DataSource=file:" + name + "?mode=memory&cache=shared" : "DataSource=:memory:");
        connection.Open();

        return connection;
    }

    private static void EnsureCreated(DbContext context, bool shared)
    {
        if (context != null)
        {
            if (!shared)
                context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }
    }
}
