#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> LoginModel.cs </Name>
//         <Created> 15/09/17 7:45:53 PM </Created>
//         <Key> 9a1479a7-e755-49be-95c3-8f6c64575e4a </Key>
//     </File>
//     <Summary>
//         LoginModel.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using FluentValidation.Attributes;
using Monkey.Core.Validators.Auth;
using System.ComponentModel.DataAnnotations;

namespace Monkey.Core.Models.Auth
{
    [Validator(typeof(SigInModelValidator))]
    public class SignInModel
    {
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string RedirectUrl { get; set; }

        public SignInModel()
        {
        }

        public SignInModel(string redirectUrl) : this()
        {
            RedirectUrl = redirectUrl;
        }
    }
}