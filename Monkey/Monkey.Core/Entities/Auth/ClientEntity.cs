#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Entity Map </Project>
//     <File>
//         <Name> ClientEntity.cs </Name>
//         <Created> 14/09/17 7:49:30 PM </Created>
//         <Key> 4ade2907-dc30-4868-8241-de12366262fe </Key>
//     </File>
//     <Summary>
//         ClientEntity.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Monkey.Core.Constants;
using System;
using System.Collections.Generic;

namespace Monkey.Core.Entities.Auth
{
    public class ClientEntity : Entity
    {
        public string Name { get; set; }

        public string NameNorm { get; set; }

        public string Secret { get; set; } = Guid.NewGuid().ToString("N");

        public string Domain { get; set; }

        public Enums.ClientType Type { get; set; } = Enums.ClientType.Website;

        public DateTimeOffset? BannedTime { get; set; }

        public string BannedRemark { get; set; }

        public virtual ICollection<RefreshTokenEntity> RefreshTokens { get; set; }
    }
}