#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> RequestTokenModelValidator.cs </Name>
//         <Created> 27/08/17 12:58:58 AM </Created>
//         <Key> eae63f22-40f6-4b07-ae16-4cf33de1b54b </Key>
//     </File>
//     <Summary>
//         RequestTokenModelValidator.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using FluentValidation;
using Monkey.Core.Constants.Auth;
using Monkey.Core.Models.Auth;

namespace Monkey.Core.Validators.Auth
{
    public class RequestTokenModelValidator : AbstractValidator<RequestTokenModel>
    {
        public RequestTokenModelValidator()
        {
            RuleFor(x => x.ClientId).NotEmpty();

            RuleFor(x => x.ClientSecret).NotEmpty();

            RuleFor(x => x.GrantType).IsInEnum();

            RuleFor(x => x.RefreshToken).NotEmpty()
                .When(x => x.GrantType == GrantType.RefreshToken);

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("User Name can't be empty")
                .MinimumLength(6).WithMessage("User Name must have at least 6 character")
                .When(x => x.GrantType == GrantType.Password);

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password can't be empty")
                .MinimumLength(6).WithMessage("Password must have at least 6 characters")
                .When(x => x.GrantType == GrantType.Password);
        }
    }
}