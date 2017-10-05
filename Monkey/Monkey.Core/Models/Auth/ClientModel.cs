#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> ClientModel.cs </Name>
//         <Created> 14/09/17 11:05:39 PM </Created>
//         <Key> 98cbaaa0-f039-434e-b538-f8d92fa9a52a </Key>
//     </File>
//     <Summary>
//         ClientModel.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using FluentValidation.Attributes;
using Monkey.Core.Constants;
using Monkey.Core.Validators.Auth;
using Puppy.DataTable.Attributes;
using Puppy.DataTable.Constants;
using System;
using System.ComponentModel.DataAnnotations;

namespace Monkey.Core.Models.Auth
{
    [Validator(typeof(ClientModelValidator.ClientCreateModelValidator))]
    public class ClientCreateModel
    {
        [DataTable(Order = 2, SortDirection = SortDirection.Ascending)]
        public string Name { get; set; }

        /// <summary>
        ///     Use " " to split domains 
        /// </summary>
        [DataTable(Order = 3)]
        [Display(Name = "Domains (Split by ' ')")]
        public string Domains { get; set; }

        [DataTable(Order = 4)]
        public Enums.ClientType Type { get; set; }
    }

    [Validator(typeof(ClientModelValidator.ClientUpdateModelValidator))]
    public class ClientUpdateModel : ClientCreateModel
    {
        [DataTable(IsVisible = false, Order = 1)]
        public int Id { get; set; }

        [Display(Name = "Banned")]
        [DataTableIgnore]
        public bool IsBanned { get; set; }

        [Display(Name = "Banned Remark")]
        [DataTableIgnore]
        public string BannedRemark { get; set; }
    }

    public class ClientModel : ClientUpdateModel
    {
        [DataTableIgnore]
        public string Subject { get; set; }

        [DataTableIgnore]
        public string Secret { get; set; }

        [Display(Name = "Banned Time")]
        [DataTable(DisplayName = "Banned Time", Order = 5)]
        public DateTimeOffset? BannedTime { get; set; }
    }
}