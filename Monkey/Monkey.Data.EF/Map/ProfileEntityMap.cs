#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> ProfileEntityMap.cs </Name>
//         <Created> 18/07/17 5:33:30 PM </Created>
//         <Key> 9604deee-c952-4dcd-b671-e8bd1ca907b2 </Key>
//     </File>
//     <Summary>
//         ProfileEntityMap.cs
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
    public class ProfileEntityMap : IEntityTypeConfiguration<ProfileEntity>
    {
        public void Map(EntityTypeBuilder<ProfileEntity> builder)
        {
            builder.ToTable(nameof(ProfileEntity));
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Version).IsRowVersion();
        }
    }
}