#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> UserMap.cs </Name>
//         <Created> 18/07/17 4:13:13 PM </Created>
//         <Key> def0b09c-ceb1-4037-ac46-2b9bdaaddf98 </Key>
//     </File>
//     <Summary>
//         UserMap.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Monkey.Data.Entities;
using Puppy.EF.Maps;

namespace Monkey.Data.EF.Map
{
    public class UserMap : EntityTypeConfiguration<UserEntity>
    {
        public override void Map(EntityTypeBuilder<UserEntity> builder)
        {
            base.Map(builder);
            builder.ToTable(nameof(UserEntity));
            builder.HasIndex(x => x.UserNameNorm);
            builder.HasIndex(x => x.PasswordHash);
        }
    }
}