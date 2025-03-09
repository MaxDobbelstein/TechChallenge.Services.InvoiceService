using TechChallenge.Common.DTO;

namespace TechChallenge.Services.InvoiceService.Interfaces;
public interface IInvoiceHandler
{
    long CreateInvoice(Invoice invoice);
    EvaluationResponse Evaluate(long invoiceId);
    void SaveDocument(long invoiceId, MemoryStream memoryStream);
}