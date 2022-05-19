using CC.UploadService.Helpers;
using FluentValidation;

namespace CC.UploadService.Validators
{
    public class FileValidator : AbstractValidator<IFormFile>
    {
        public FileValidator()
        {
            RuleFor(x => x.Length).NotNull()
                .LessThanOrEqualTo(1073741824)
                .WithMessage("File size is larger than allowed, 1Gb");

            RuleFor(x => x.ContentType)
                .NotNull()
                .Must(x => x.Equals(FileFormats.CsvFormat) || x.Equals(FileFormats.PdfFormat))
                .WithMessage("File type is not supported. Please re-upload a file with CSV or PDF format.");
        }
    }
}
