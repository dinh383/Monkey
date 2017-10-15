#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> LoginModelValidator.cs </Name>
//         <Created> 15/09/17 7:46:32 PM </Created>
//         <Key> db2edc34-0bee-4397-954c-bb7a1fd0321f </Key>
//     </File>
//     <Summary>
//         LoginModelValidator.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Monkey.Core.Models.Auth;
using FluentValidation;

namespace Monkey.Core.Validators.Auth
{
    public class SigInModelValidator : AbstractValidator<SignInModel>
    {
        public SigInModelValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("User Name can't be empty")
                .MinimumLength(6).WithMessage("User Name must have at least 6 character");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password can't be empty")
                .MinimumLength(6).WithMessage("Password must have at least 6 characters");
        }
    }
}