using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TechChallenge.Common.DTO;
//Anmerkung: Seperates Projekt, damit es später evtl als NuGet Paket bereitgestellt werden kann.
//Anmerkung: Normalerweise separier ich DTO und Entity und mappe dann mit Automapper oder Mapperli um (DTOs sind bei mir abgeflachtere 
public class Invoice
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [JsonIgnore]
    public long Id { get; set; }
    public string InvoiceNumber { get; set; }
    public DateOnly InvoiceDate { get; set; }
    public string Comment { get; set; }
    public decimal Amount { get; set; }       
}
