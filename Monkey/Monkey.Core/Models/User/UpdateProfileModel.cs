#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> UpdateProfileModel.cs </Name>
//         <Created> 10/10/17 8:46:18 PM </Created>
//         <Key> 5e7bba08-73ff-4787-8e22-60ced3d28882 </Key>
//     </File>
//     <Summary>
//         UpdateProfileModel.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using FluentValidation.Attributes;
using Microsoft.AspNetCore.Http;
using Monkey.Core.Validators.User;

namespace Monkey.Core.Models.User
{
    [Validator(typeof(UpdateProfileModelValidator))]
    public class UpdateProfileModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string AvatarUrl { get; set; }

        public IFormFile Avatar { get; set; }
    }
}