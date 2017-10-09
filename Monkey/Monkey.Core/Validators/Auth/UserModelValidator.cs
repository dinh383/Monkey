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
                RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(500).When(x => string.IsNullOrWhiteSpace(x.Phone));

                RuleFor(x => x.Phone).NotEmpty().MaximumLength(50).When(x => string.IsNullOrWhiteSpace(x.Email));
            }
        }

        public class UserUpdateModelValidator : AbstractValidator<UserUpdateModel>
        {
            public UserUpdateModelValidator()
            {
                RuleFor(x => x.Id).NotEmpty();

                RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(500).When(x => string.IsNullOrWhiteSpace(x.Phone));

                RuleFor(x => x.Phone).NotEmpty().MaximumLength(50).When(x => string.IsNullOrWhiteSpace(x.Email));

                RuleFor(x => x.UserName).MaximumLength(50);

                RuleFor(x => x.BannedRemark).NotEmpty().MaximumLength(250).When(x => x.IsBanned);
            }
        }
    }
}