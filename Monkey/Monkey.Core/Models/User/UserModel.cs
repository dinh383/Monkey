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

using FluentValidation.Attributes;
using Microsoft.AspNetCore.Mvc;
using Monkey.Core.Validators.User;
using Puppy.DataTable.Attributes;
using Puppy.DataTable.Constants;
using System;
using System.ComponentModel.DataAnnotations;

namespace Monkey.Core.Models.User
{
    [Validator(typeof(UserModelValidator.UserCreateModelValidator))]
    public class UserCreateModel
    {
        [DataTable(Order = 3, SortDirection = SortDirection.Ascending, DisplayName = "Full Name")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [DataTable(Order = 4)]
        [DataType(DataType.EmailAddress)]
        [Remote("CheckUniqueEmail", "User", HttpMethod = "POST", AdditionalFields = "Id", ErrorMessage = "The email of User already exist, please try another.")]
        public string Email { get; set; }

        [DataTable(Order = 5)]
        [Remote("CheckUniquePhone", "User", HttpMethod = "POST", AdditionalFields = "Id", ErrorMessage = "The phone of User already exist, please try another.")]
        public string Phone { get; set; }

        [DataTable(IsVisible = false, Order = 6, DisplayName = "User Name")]
        [Remote("CheckUniqueUserName", "User", HttpMethod = "POST", AdditionalFields = "Id", ErrorMessage = "The user name of User already exist, please try another.")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [DataTable(IsVisible = false, DisplayName = "Role Id")]
        [Display(Name = "Role")]
        public int? RoleId { get; set; }
    }

    [Validator(typeof(UserModelValidator.UserUpdateModelValidator))]
    public class UserUpdateModel : UserCreateModel
    {
        [DataTable(IsVisible = false, Order = 0)]
        public int Id { get; set; }

        [DataTable(IsVisible = false, Order = 0)]
        public string Subject { get; set; }

        [Display(Name = "Ban")]
        [DataTableIgnore]
        public bool IsBanned { get; set; }

        [Display(Name = "Ban Remark")]
        [DataTableIgnore]
        public string BannedRemark { get; set; }
    }

    public class UserModel : UserUpdateModel
    {
        [DataTableIgnore]
        public int? AvatarId { get; set; }

        [DataTable(Order = 1, DisplayName = "Avatar")]
        [Display(Name = "Avatar")]
        public string AvatarUrl { get; set; }

        [DataTable(Order = 7, DisplayName = "Role")]
        public string RoleName { get; set; }

        [Display(Order = 8, Name = "Banned Time")]
        [DataTable(DisplayName = "Banned Time")]
        public DateTimeOffset? BannedTime { get; set; }

        [DataTable(Order = 9, DisplayName = "Active Time")]
        public DateTimeOffset? ActiveTime { get; set; }

        [DataTable(IsVisible = false, DisplayName = "Phone Confirmed Time")]
        public DateTimeOffset? PhoneConfirmedTime { get; set; }

        [DataTable(IsVisible = false, DisplayName = "Email Confirmed Time")]
        public DateTimeOffset? EmailConfirmedTime { get; set; }
    }
}