using TechChallenge.Services.InvoiceService.Interfaces;

namespace TechChallenge.Services.InvoiceService.Tests.Mocks;

public class FileHandlerMock : IFileHandler
{
    public bool Exists(long invoiceId)
        => true;

    public async Task SaveFile(long invoiceId, MemoryStream memoryStream) { }
}
