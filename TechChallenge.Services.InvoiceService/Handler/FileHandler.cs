using Microsoft.Extensions.Options;
using TechChallenge.Services.InvoiceService.Interfaces;

namespace TechChallenge.Services.InvoiceService.Handler;

public class FileHandler : IFileHandler
{
    private readonly ILogger<FileHandler> logger;
    private readonly Options.FileOptions options;

    public FileHandler(ILogger<FileHandler> logger, IOptions<Options.FileOptions> options)
    {
        this.logger = logger;
        this.options = options.Value ?? throw new ArgumentNullException("FileOptions are missing");
    }

    public async Task SaveFile(long invoiceId, MemoryStream memoryStream)
    {
        var filePath = $"{options.BasePath}/{invoiceId}";
        if (!Directory.Exists(filePath))
            Directory.CreateDirectory(filePath);

        var fullName = $"{filePath}/invoice.pdf";
        using (FileStream fileStream = new FileStream(fullName, FileMode.Create))
        {
            memoryStream.Seek(0, SeekOrigin.Begin);
            memoryStream.CopyTo(fileStream);
            await fileStream.FlushAsync();
            await memoryStream.DisposeAsync();
        }
    }

    public bool Exists(long invoiceId)
    {
        var fullName = $"{options.BasePath}/{invoiceId}/invoice.pdf";
        return File.Exists(fullName);
    }
}
