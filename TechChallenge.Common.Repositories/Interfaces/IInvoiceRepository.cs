using TechChallenge.Common.DTO;

namespace TechChallenge.Common.Repositories.Interfaces;
public interface IInvoiceRepository
{
    bool Exists(long invoiceId);
    Invoice Find(long invoiceId);
    long Insert(Invoice invoice);
}