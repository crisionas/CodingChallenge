using CC.UploadService.Models.Requests;
using FluentValidation;

namespace CC.UploadService.Validators
{
    public class FileUploadRequestValidator : AbstractValidator<FileUploadRequest>
    {
        public FileUploadRequestValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Name)
                .NotEmpty();

            RuleFor(x => x.File)
                .NotNull()
                .SetValidator(new FileValidator()!);
        }
    }
}
