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

using FluentValidation;
using Monkey.Core.Models.User;

namespace Monkey.Core.Validators.User
{
    public class LoginModelValidator : AbstractValidator<LoginModel>
    {
        public LoginModelValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("User Name can't be empty");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password can't be empty");
            //.Matches(Regex.PasswordFormat).WithMessage("Password must have at least 8 characters include uppercase and numeric");
        }
    }
}