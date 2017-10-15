#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> UserModel.cs </Name>
//         <Created> 08/10/17 4:33:16 PM </Created>
//         <Key> 21c699e6-ba94-4166-97d0-3a3fc38970ce </Key>
//     </File>
//     <Summary>
//         UserModel.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Monkey.Core.Validators.Auth;
using FluentValidation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Monkey.Core.Models.Auth
{
    [Validator(typeof(SetPasswordModelValidator))]
    public class SetPasswordModel
    {
        public string Token { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}