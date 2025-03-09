using Microsoft.Extensions.Options;
using System.Data;
using System.Runtime.CompilerServices;
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
        var invoiceId = invoiceRepository.Insert(invoice);

        return invoiceId;
    }

    public async Task SaveDocument(long invoiceId, MemoryStream memoryStream) 
    {
        if (!invoiceRepository.Exists(invoiceId))
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

    }

    public async Task<EvaluationResponse> EvaluateAsync(long invoiceId)
    {
        var invoice = invoiceRepository.Find(invoiceId);
        if (invoice == null)
            throw new ArgumentException($"InvoiceId: {invoiceId} | An invoice with this Id does not exist. Please create the invoice before evaluating");
        var riskLevel = await riskLevelServiceClient.GetRiskLevelAsync(invoice);
        EvaluationResponse evaluationResponse = CreateEvaluationResponse(invoiceId, riskLevel);
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