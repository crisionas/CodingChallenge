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
                .NotEmpty()
                .WithErrorCode(StatusCodes.Status400BadRequest.ToString());

            RuleFor(x => x.File)
                .NotNull()
                .WithErrorCode(StatusCodes.Status400BadRequest.ToString())
                .SetValidator(new FileValidator()!);
        }
    }
}
