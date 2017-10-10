using FluentValidation;
using Monkey.Core.Models.Auth;

namespace Monkey.Core.Validators.Auth
{
    public class ChangePasswordModelValidator : AbstractValidator<ChangePasswordModel>
    {
        public ChangePasswordModelValidator()
        {
            RuleFor(x => x.CurrentPassword).NotEmpty().Length(6, 100).WithMessage("Current Password must between 6 and 100 characters");
            RuleFor(x => x.Password).NotEmpty().Length(6, 100).WithMessage("Password must between 6 and 100 characters");
            RuleFor(x => x.ConfirmPassword).NotEmpty().Equal(x => x.Password).WithMessage("Confirm Password must match with Password");
        }
    }
}