using CC.IdentityService.Models.Requests;
using CC.IdentityService.Models.Settings;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace CC.IdentityService.Validators
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator(IOptions<IdentitySettings> options)
        {
            var settings = options.Value;

            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Username)
                .NotEmpty()
                .WithErrorCode(StatusCodes.Status400BadRequest.ToString());

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithErrorCode(StatusCodes.Status400BadRequest.ToString());

            RuleFor(x => x.Company)
                .NotEmpty()
                .WithErrorCode(StatusCodes.Status400BadRequest.ToString());

            RuleFor(x => x.Scopes)
                .NotNull()
                .WithErrorCode(StatusCodes.Status400BadRequest.ToString())
                .ForEach(x =>
                {
                    x.NotEmpty()
                        .WithErrorCode(StatusCodes.Status400BadRequest.ToString())
                        .Must(g => settings.AuthCredentials.Select(a => a.Scope).Contains(g))
                        .WithErrorCode(StatusCodes.Status400BadRequest.ToString())
                        .WithMessage("The scopes are invalid.");
                });
        }
    }
}
