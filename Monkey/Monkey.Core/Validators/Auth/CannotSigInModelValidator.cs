using Monkey.Core.Models.Auth;
using FluentValidation;

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