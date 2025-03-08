using FluentValidation;

namespace TechChallenge.Services.InvoiceService.Validation;

public class FileValidator : AbstractValidator<IFormFile>
{
    private const int MAXFILESIZE = 5242880;
    private const string MIMETYPEPDF = "application/pdf"
    public FileValidator()
    {
        RuleFor(x => x.Length).NotNull().LessThanOrEqualTo(MAXFILESIZE).WithMessage("Die Dateigröße darf 5 Megabyte nicht überschreiten");
        RuleFor(x => x.ContentType).NotNull().Must(x => x.Equals(MIMETYPEPDF)).WithMessage("Es dürfen nur PDF Dateien hochgeladen werden");
    }

}
