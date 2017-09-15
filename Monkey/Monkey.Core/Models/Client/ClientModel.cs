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

using System;
using Monkey.Core.Constants;

namespace Monkey.Core.Models.Client
{
    public class ClientCreateModel
    {
        public string Name { get; set; }

        public string Domain { get; set; }

        public Enums.ClientType Type { get; set; }
    }

    public class ClientUpdateModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Enums.ClientType Type { get; set; }
        public bool IsBanned { get; set; }
        public string BannedRemark { get; set; }
    }

    public class ClientModel : ClientCreateModel
    {
        public int Id { get; set; }

        public string GlobalId { get; set; }

        public string Secret { get; set; }

        public bool IsBanned { get; set; }

        public DateTimeOffset? BannedTime { get; set; }

        public string BannedRemark { get; set; }
    }
}