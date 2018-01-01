using FluentValidation;
using Monkey.Core.Models.User;

namespace Monkey.Core.Validators.User
{
    public class UserModelValidator
    {
        public class CreateUserModelValidator : AbstractValidator<CreateUserModel>
        {
            public CreateUserModelValidator()
            {
                RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(500);

                RuleFor(x => x.FullName).NotEmpty().MaximumLength(200);

                RuleFor(x => x.UserName).NotEmpty().MaximumLength(500);
            }
        }

        public class UpdateUserModelValidator : AbstractValidator<UpdateUserModel>
        {
            public UpdateUserModelValidator()
            {
                RuleFor(x => x.Id).NotEmpty();

                RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(500);

                RuleFor(x => x.FullName).NotEmpty().MaximumLength(200);

                RuleFor(x => x.UserName).NotEmpty().MaximumLength(500);

                RuleFor(x => x.BannedRemark).NotEmpty().MaximumLength(250).When(x => x.IsBanned);
            }
        }
    }
}