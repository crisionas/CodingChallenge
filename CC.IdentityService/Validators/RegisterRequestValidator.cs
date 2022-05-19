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
                .NotEmpty();


            RuleFor(x => x.Password)
                .NotEmpty();

            RuleFor(x => x.Company)
                .NotEmpty();

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Scopes)
                .NotNull()
                .ForEach(x =>
                {
                    x.NotEmpty()
                        .Must(g => settings.AuthCredentials.Select(a => a.Scope).Contains(g))
                        .WithMessage("The scopes are invalid.");
                });
        }
    }
}
