using Monkey.Core.Models.Auth;
using FluentValidation;

namespace Monkey.Core.Validators.Auth
{
    public class SetPasswordModelValidator : AbstractValidator<SetPasswordModel>
    {
        public SetPasswordModelValidator()
        {
            RuleFor(x => x.Password).NotEmpty().Length(6, 100).WithMessage("Password must between 6 and 100 characters");
            RuleFor(x => x.ConfirmPassword).NotEmpty().Equal(x => x.Password).WithMessage("Confirm Password must match with Password");
        }
    }
}