using FluentValidation.Attributes;
using Microsoft.AspNetCore.Mvc;
using Monkey.Core.Validators.Auth;
using System.ComponentModel.DataAnnotations;

namespace Monkey.Core.Models.Auth
{
    [Validator(typeof(ChangePasswordModelValidator))]
    public class ChangePasswordModel
    {
        [Remote("CheckCurrentPassword", "Auth", HttpMethod = "POST", ErrorMessage = "The current password is wrong.")]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}