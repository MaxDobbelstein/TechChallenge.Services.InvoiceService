namespace TechChallenge.Services.InvoiceService.Interfaces;

public interface IFileHandler
{
    bool Exists(long invoiceId);
    Task SaveFile(long invoiceId, MemoryStream memoryStream);
}