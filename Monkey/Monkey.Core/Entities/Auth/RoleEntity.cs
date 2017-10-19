#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Entity Map </Project>
//     <File>
//         <Name> RoleEntity.cs </Name>
//         <Created> 13/09/17 11:34:58 PM </Created>
//         <Key> aa38116e-4fd8-4d23-8530-4135f976817e </Key>
//     </File>
//     <Summary>
//         RoleEntity.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Monkey.Core.Entities.User;
using System.Collections.Generic;

namespace Monkey.Core.Entities.Auth
{
    public class RoleEntity : Entity
    {
        public string Name { get; set; }

        public string NameNorm { get; set; }

        public string Description { get; set; }

        public virtual ICollection<PermissionEntity> Permissions { get; set; }

        public virtual ICollection<UserEntity> Users { get; set; }
    }
}