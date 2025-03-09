using TechChallenge.Common.DTO;

namespace TechChallenge.Services.InvoiceService.Interfaces;
public interface IInvoiceHandler
{
    long CreateInvoice(Invoice invoice);
    Task<EvaluationResponse> Evaluate(long invoiceId);
    Task SaveDocument(long invoiceId, MemoryStream memoryStream);
}