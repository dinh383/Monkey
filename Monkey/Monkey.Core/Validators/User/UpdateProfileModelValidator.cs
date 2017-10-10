#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> UpdateProfileModelValidator.cs </Name>
//         <Created> 10/10/17 8:51:21 PM </Created>
//         <Key> 208787c6-4279-4e33-9d51-90fb13f7ddb4 </Key>
//     </File>
//     <Summary>
//         UpdateProfileModelValidator.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using FluentValidation;
using Monkey.Core.Models.User;

namespace Monkey.Core.Validators.User
{
    public class UpdateProfileModelValidator : AbstractValidator<UpdateProfileModel>
    {
        public UpdateProfileModelValidator()
        {
            RuleFor(x => x.FirstName).Length(0, 50);

            RuleFor(x => x.LastName).Length(0, 50);
        }
    }
}