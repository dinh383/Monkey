#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> ClientModelValidator.cs </Name>
//         <Created> 05/10/17 10:52:37 PM </Created>
//         <Key> fce561fd-ef57-4988-bcd8-4e6a8ac43016 </Key>
//     </File>
//     <Summary>
//         ClientModelValidator.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using FluentValidation;
using Monkey.Core.Models.Auth;

namespace Monkey.Core.Validators.Auth
{
    public class ClientModelValidator
    {
        public class ClientCreateModelValidator : AbstractValidator<ClientCreateModel>
        {
            public ClientCreateModelValidator()
            {
                RuleFor(x => x.Name).NotEmpty().MaximumLength(50);

                RuleFor(x => x.Domains).MaximumLength(50);

                RuleFor(x => x.Type).IsInEnum();
            }
        }

        public class ClientUpdateModelValidator : AbstractValidator<ClientUpdateModel>
        {
            public ClientUpdateModelValidator()
            {
                RuleFor(x => x.Id).NotEmpty();

                RuleFor(x => x.Name).NotEmpty().MaximumLength(50);

                RuleFor(x => x.Domains).MaximumLength(50);

                RuleFor(x => x.Type).IsInEnum();

                RuleFor(x => x.BannedRemark).NotEmpty().MaximumLength(250).When(x => x.IsBanned);
            }
        }
    }
}