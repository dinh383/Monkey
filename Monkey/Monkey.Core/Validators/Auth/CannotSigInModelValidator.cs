using FluentValidation;
using Monkey.Core.Models.Auth;

namespace Monkey.Core.Validators.Auth
{
    public class CannotSigInModelValidator : AbstractValidator<CannotSigInModel>
    {
        public CannotSigInModelValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(500);
        }
    }
}