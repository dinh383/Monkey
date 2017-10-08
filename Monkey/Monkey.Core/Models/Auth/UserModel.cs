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

using System;
using FluentValidation.Attributes;
using Monkey.Core.Validators.Auth;
using Puppy.DataTable.Attributes;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Puppy.DataTable.Constants;

namespace Monkey.Core.Models.Auth
{
    [Validator(typeof(UserModelValidator.UserCreateModelValidator))]
    public class UserCreateModel
    {
        [DataTable(Order = 3)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataTable(IsVisible = false)]
        public int RoleId { get; set; }
    }

    [Validator(typeof(UserModelValidator.UserUpdateModelValidator))]
    public class UserUpdateModel : UserCreateModel
    {
        [DataTable(IsVisible = false, Order = 1)]
        public int Id { get; set; }

        [DataTable(Order = 4, SortDirection = SortDirection.Ascending)]
        [Remote("CheckUniqueUserName", "User", HttpMethod = "POST", AdditionalFields = "Id", ErrorMessage = "The user name of User already exist, please try another.")]
        public string UserName { get; set; }

        [DataTable(Order = 2, SortDirection = SortDirection.Ascending)]
        public string FullName { get; set; }

        [Display(Name = "Banned")]
        [DataTableIgnore]
        public bool IsBanned { get; set; }

        [Display(Name = "Banned Remark")]
        [DataTableIgnore]
        public string BannedRemark { get; set; }
    }

    public class UserModel : UserUpdateModel
    {
        [Display(Name = "Banned Time")]
        [DataTable(DisplayName = "Banned Time", Order = 5)]
        public DateTimeOffset? BannedTime { get; set; }

        public string RoleName { get; set; }
    }
}