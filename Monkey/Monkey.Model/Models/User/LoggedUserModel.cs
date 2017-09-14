#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> LoggedUserModel.cs </Name>
//         <Created> 13/09/17 10:33:42 PM </Created>
//         <Key> 45c835b1-4933-4f5e-8d6e-a89477e16a00 </Key>
//     </File>
//     <Summary>
//         LoggedUserModel.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Monkey.Core.Constants;
using System;
using System.Collections.Generic;

namespace Monkey.Model.Models.User
{
    public class LoggedUserModel
    {
        public int Id { get; set; }

        public string GlobalId { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public DateTimeOffset? EmailConfirmedTime { get; set; }

        public string Phone { get; set; }

        public DateTimeOffset? PhoneConfirmedTime { get; set; }

        public List<Enums.Permission> ListPermission { get; set; }

        public int ClientId { get; set; }

        public string ClientNo { get; set; }
    }
}