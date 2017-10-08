using FluentValidation;
using Monkey.Core.Models.Auth;

namespace Monkey.Core.Validators.Auth
{
    public class UserModelValidator
    {
        public class UserCreateModelValidator : AbstractValidator<UserCreateModel>
        {
            public UserCreateModelValidator()
            {
                RuleFor(x => x.Email).NotEmpty().MaximumLength(50);
            }
        }

        public class UserUpdateModelValidator : AbstractValidator<UserUpdateModel>
        {
            public UserUpdateModelValidator()
            {
                RuleFor(x => x.Id).NotEmpty();

                RuleFor(x => x.UserName).NotEmpty().MaximumLength(50);

                RuleFor(x => x.FullName).MaximumLength(200);

                RuleFor(x => x.BannedRemark).NotEmpty().MaximumLength(250).When(x => x.IsBanned);
            }
        }
    }
}