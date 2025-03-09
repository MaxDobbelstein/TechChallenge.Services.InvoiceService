using Microsoft.Extensions.Options;
using TechChallenge.Common.DTO;
using TechChallenge.Services.InvoiceService.Interfaces;

namespace TechChallenge.Services.InvoiceService.Handler;

public class InvoiceHandler : IInvoiceHandler
{
    private readonly ILogger<InvoiceHandler> logger;
    private readonly Options.FileOptions options;
    private readonly IRiskLevelServiceClient riskLevelServiceClient;

    public InvoiceHandler(ILogger<InvoiceHandler> logger, IOptions<Options.FileOptions> options, IRiskLevelServiceClient riskLevelServiceClient)
    {
        this.logger = logger;
        this.options = options.Value ?? throw new ArgumentNullException("FileOptions are missing");
        this.riskLevelServiceClient = riskLevelServiceClient;
    }

    public long CreateInvoice(Invoice invoice)
    {
        return 0;
    }

    public void SaveDocument(long invoiceId, MemoryStream memoryStream) { }

    public EvaluationResponse Evaluate(long invoiceId)
    {
        return new EvaluationResponse();
    }
}
