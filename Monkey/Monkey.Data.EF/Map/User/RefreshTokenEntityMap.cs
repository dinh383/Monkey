#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Entity Map </Project>
//     <File>
//         <Name> RefreshTokenEntityMap.cs </Name>
//         <Created> 13/09/17 11:36:29 PM </Created>
//         <Key> a1a9f31d-f44f-4d42-9ffa-f181dd3287f4 </Key>
//     </File>
//     <Summary>
//         RefreshTokenEntityMap.cs
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
    public class RefreshTokenEntityMap : EntityTypeConfiguration<RefreshTokenEntity>
    {
        public override void Map(EntityTypeBuilder<RefreshTokenEntity> builder)
        {
            base.Map(builder);

            builder.ToTable(nameof(RefreshTokenEntity));

            builder.HasOne(x => x.User).WithMany(x => x.RefreshTokens).HasForeignKey(x => x.UserId);

            builder.HasOne(x => x.Client).WithMany(x => x.RefreshTokens).HasForeignKey(x => x.ClientId);

            builder.HasIndex(x => x.RefreshToken);
            builder.HasIndex(x => x.ClientId);
            builder.HasIndex(x => x.UserId);
        }
    }
}