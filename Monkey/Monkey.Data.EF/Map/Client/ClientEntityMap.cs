#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Entity Map </Project>
//     <File>
//         <Name> ClientEntityMap.cs </Name>
//         <Created> 14/09/17 7:57:54 PM </Created>
//         <Key> ff6bfa4e-fb9f-4c7b-a4ac-3141680f5438 </Key>
//     </File>
//     <Summary>
//         ClientEntityMap.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Monkey.Data.Entities.Client;
using Puppy.EF.Maps;

namespace Monkey.Data.EF.Map.Client
{
    public class ClientEntityMap : EntityTypeConfiguration<ClientEntity>
    {
        public override void Map(EntityTypeBuilder<ClientEntity> builder)
        {
            base.Map(builder);

            builder.ToTable(nameof(ClientEntity));

            builder.HasIndex(x => x.GlobalId);
            builder.HasIndex(x => x.Secret);
            builder.HasIndex(x => x.NameNorm);
        }
    }
}