using CC.NotificationService.Models;
using FluentValidation;

namespace CC.NotificationService.Validators
{
    public class EmailMessageValidator : AbstractValidator<EmailMessage>
    {
        public EmailMessageValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Message)
                .NotEmpty();

            RuleFor(x => x.Subject)
                .NotEmpty();
        }
    }
}
