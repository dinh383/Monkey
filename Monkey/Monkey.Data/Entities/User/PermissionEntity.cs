#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Entity Map </Project>
//     <File>
//         <Name> PermissionEntity.cs </Name>
//         <Created> 13/09/17 11:33:40 PM </Created>
//         <Key> f95d9e8a-de79-46de-8adc-039ca639a420 </Key>
//     </File>
//     <Summary>
//         PermissionEntity.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Monkey.Core.Constants;

namespace Monkey.Data.Entities.User
{
    public class PermissionEntity : Entity
    {
        public int RoleId { get; set; }

        public virtual RoleEntity Role { get; set; }

        public Enums.Permission Permission { get; set; }
    }
}