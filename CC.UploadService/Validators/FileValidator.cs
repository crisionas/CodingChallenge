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
                .Must(x => x.Equals("text/csv") || x.Equals("application/vnd.openxmlformats-officedocument.wordprocessingml.document"))
                .WithMessage("File type is not supported. Please re-upload a file with CSV or Docx format.");
        }
    }
}
