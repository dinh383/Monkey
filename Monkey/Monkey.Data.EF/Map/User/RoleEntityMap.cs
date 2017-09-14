#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Entity Map </Project>
//     <File>
//         <Name> RoleEntityMap.cs </Name>
//         <Created> 13/09/17 11:36:49 PM </Created>
//         <Key> c022b8f7-0c81-496d-8ca6-16e5655053ac </Key>
//     </File>
//     <Summary>
//         RoleEntityMap.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Monkey.Data.Entities.User;
using Puppy.EF.Maps;

namespace Monkey.Data.EF.Map.User
{
    public class RoleEntityMap : EntityTypeConfiguration<RoleEntity>
    {
        public override void Map(EntityTypeBuilder<RoleEntity> builder)
        {
            base.Map(builder);

            builder.ToTable(nameof(RoleEntity));
        }
    }
}