namespace TechChallenge.Common.DTO;
//Anmerkung: Seperates Projekt, damit es später evtl als NuGet Paket bereitgestellt werden kann.
public class Invoice
{
    public long Id { get; set; }
    public string InvoiceNumber { get; set; }
    public DateOnly InvoiceDate { get; set; }
    public string Comment { get; set; }
    public decimal Amount { get; set; }       
}
