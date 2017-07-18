#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> UserEntityMap.cs </Name>
//         <Created> 18/07/17 4:13:13 PM </Created>
//         <Key> def0b09c-ceb1-4037-ac46-2b9bdaaddf98 </Key>
//     </File>
//     <Summary>
//         UserEntityMap.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Monkey.Data.Entities;
using Puppy.EF.Mapping;

namespace Monkey.Data.EF.Map
{
    public class UserEntityMap : IEntityTypeConfiguration<UserEntity>
    {
        public void Map(EntityTypeBuilder<UserEntity> builder)
        {
            builder.ToTable(nameof(UserEntity));
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Version).IsRowVersion();
        }
    }
}