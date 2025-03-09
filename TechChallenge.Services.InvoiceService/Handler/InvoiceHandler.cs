using Microsoft.Extensions.Options;
using TechChallenge.Common.DTO;
using TechChallenge.Common.DTO.Enums;
using TechChallenge.Common.Repositories.Interfaces;
using TechChallenge.Services.InvoiceService.Interfaces;

namespace TechChallenge.Services.InvoiceService.Handler;

public class InvoiceHandler : IInvoiceHandler
{
    private readonly ILogger<InvoiceHandler> logger;
    private readonly Options.FileOptions options;
    private readonly IRiskLevelServiceClient riskLevelServiceClient;
    private readonly IInvoiceRepository invoiceRepository;

    public InvoiceHandler(ILogger<InvoiceHandler> logger,
                          IOptions<Options.FileOptions> options,
                          IRiskLevelServiceClient riskLevelServiceClient,
                          IInvoiceRepository invoiceRepository)
    {
        this.logger = logger;
        this.options = options.Value ?? throw new ArgumentNullException("FileOptions are missing");
        this.riskLevelServiceClient = riskLevelServiceClient;
        this.invoiceRepository = invoiceRepository;
    }

    public long CreateInvoice(Invoice invoice)
    {
        logger.LogInformation("InvoiceNumber: {invoiceNumber} | Invoicedate: {invoiceDate} | Amount: {amount} | Creating new Invoice", invoice.InvoiceNumber, invoice.InvoiceDate, invoice.Amount)
        var invoiceId = invoiceRepository.Insert(invoice);
        logger.LogDebug("Invoice: {@invoice}| Invoice has been created", invoice);
        return invoiceId;
    }

    public async Task SaveDocument(long invoiceId, MemoryStream memoryStream) 
    {
        var exists = invoiceRepository.Exists(invoiceId);
        logger.LogInformation("InvoiceId: {invoiceId} | Exists: {exists} | Saving Document for invoice", invoiceId, exists);
        if (!exists)
            throw new ArgumentException($"InvoiceId: {invoiceId} | An invoice with this Id does not exist. Please create the invoice before uploading the document");

        var filePath = $"{options.BasePath}/{invoiceId}";
        if (!Directory.Exists(filePath))
            Directory.CreateDirectory(filePath);

        var fullName = $"{filePath}/invoice.pdf";
        using(FileStream fileStream = new FileStream(fullName, FileMode.Create))
        {
            memoryStream.Seek(0, SeekOrigin.Begin);
            memoryStream.CopyTo(fileStream);
            await fileStream.FlushAsync();
            await memoryStream.DisposeAsync();
        }

        logger.LogDebug("InvoiceId: {invoiceId} | Exists: {exists} | File Fullname: {fullName} | Document saved", invoiceId, exists, fullName);
    }

    public async Task<EvaluationResponse> Evaluate(long invoiceId)
    {
        var invoice = invoiceRepository.Find(invoiceId);
        logger.LogInformation("InvoiceId: {invoiceId} | Exists: {exists} | Evaluating invoice", invoiceId, invoice != null);
        if (invoice == null)
            throw new ArgumentException($"InvoiceId: {invoiceId} | An invoice with this Id does not exist. Please create the invoice before evaluating");
        var fullName = $"{options.BasePath}/{invoiceId}/invoice.pdf";
        if(!File.Exists(fullName))
            throw new ArgumentException($"InvoiceId: {invoiceId} | Has no document attached. Please upload the document for the invoice before evaluating");

        var riskLevel = await riskLevelServiceClient.GetRiskLevelAsync(invoice);
        if (riskLevel == null)
            throw new Exception($"InvoiceId: {invoiceId} | Error getting Risklevel. Please check configuratoin or risklevel api");

        EvaluationResponse evaluationResponse = CreateEvaluationResponse(invoiceId, riskLevel);
        logger.LogDebug("InvoiceId: {invoiceId} | Exists: {exists} | Evalution: {@evalution} | Evaluating invoice", invoiceId, invoice != null, evaluationResponse);
        return evaluationResponse;
    }

    private static EvaluationResponse CreateEvaluationResponse(long invoiceId, RiskLevelResponse riskLevel)
    {
        var evaluationResponse = new EvaluationResponse();
        evaluationResponse.EvaluationId = "EVAL" + invoiceId;
        evaluationResponse.InvoiceId = invoiceId;
        evaluationResponse.Classification = riskLevel.Classification;
        evaluationResponse.RiskLevel = riskLevel.RiskLevel;
        Evaluate(evaluationResponse);

        return evaluationResponse;
    }

    private static void Evaluate(EvaluationResponse evaluationResponse)
    {
        switch (evaluationResponse.RiskLevel)
        {
            case RiskLevel.Low:
                evaluationResponse.Evaluation = Evaluation.Approve;
                evaluationResponse.EvaluationReason = "Approved since Risk is low";
                break;
            case RiskLevel.Medium:
                evaluationResponse.Evaluation = Evaluation.Review;
                evaluationResponse.EvaluationReason = "Review since Risk is medium";
                break;
            case RiskLevel.High:
                evaluationResponse.Evaluation = Evaluation.Deny;
                evaluationResponse.EvaluationReason = "Deny since Risk is high";
                break;
        }
    }
}