using FluentValidation;
using System.Text.RegularExpressions;
using TechChallenge.Common.DTO;

namespace TechChallenge.Services.InvoiceService.Validation;

public class InvoiceValidator : AbstractValidator<Invoice>
{
    private const string REGEXINVOICENUMBER = @"(?<=S)(\d{5})";
    public InvoiceValidator()
    {
        RuleFor(x => x.InvoiceDate).LessThan(DateOnly.FromDateTime(DateTime.Now)).NotNull();
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.Id).GreaterThan(0);
        //Anmerkung -> die Strings sollten Konstanten sein. 
        RuleFor(x => x.InvoiceNumber)
               .MinimumLength(6).WithMessage("Die Mindestlänge für {PropertyName} beträgt 6")
               .MaximumLength(6).WithMessage("Die Maximallänge für {PropertyName} beträgt 6")
               .Must(x => x.StartsWith("S")).WithMessage("{PropertyName} muss mit einem 'S' beginnen")
               .Must(x => Regex.IsMatch(x, REGEXINVOICENUMBER)).WithMessage("{PropertyName} muss mit einem 'S' beginnen und darauffolgend 5 Ziffern");
    }
}
