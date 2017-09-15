#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Entity Map </Project>
//     <File>
//         <Name> PermissionEntityMap.cs </Name>
//         <Created> 13/09/17 11:37:01 PM </Created>
//         <Key> d852be4c-7f8e-429c-ada4-28c9e3395724 </Key>
//     </File>
//     <Summary>
//         PermissionEntityMap.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Monkey.Core.Entities.User;
using Puppy.EF.Maps;

namespace Monkey.Data.EF.Map.User
{
    public class PermissionEntityMap : EntityTypeConfiguration<PermissionEntity>
    {
        public override void Map(EntityTypeBuilder<PermissionEntity> builder)
        {
            base.Map(builder);

            builder.ToTable(nameof(PermissionEntity));
        }
    }
}