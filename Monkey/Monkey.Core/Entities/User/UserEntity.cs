#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> UserEntity.cs </Name>
//         <Created> 18/07/17 4:11:13 PM </Created>
//         <Key> abe1ae76-0722-4dba-8ebf-51eb28f5796a </Key>
//     </File>
//     <Summary>
//         UserEntity.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using System;
using System.Collections.Generic;

namespace Monkey.Core.Entities.User
{
    public class UserEntity : Entity
    {
        public string UserName { get; set; }

        public string UserNameNorm { get; set; }

        // Password

        public string PasswordHash { get; set; }

        public DateTimeOffset? PasswordLastUpdatedTime { get; set; }
        // Email

        public string Email { get; set; }

        public string EmailNorm { get; set; }

        public DateTimeOffset? EmailConfirmedTime { get; set; }

        // Phone

        public string Phone { get; set; }

        public DateTimeOffset? PhoneConfirmedTime { get; set; }

        public virtual ProfileEntity Profile { get; set; }

        public DateTimeOffset? ActiveTime { get; set; }

        // Ban

        public DateTimeOffset? BannedTime { get; set; }

        public string BannedRemark { get; set; }

        public int? RoleId { get; set; }

        public virtual RoleEntity Role { get; set; }

        public virtual ICollection<RefreshTokenEntity> RefreshTokens { get; set; }
    }
}