#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Repository </Project>
//     <File>
//         <Name> ProfileEntity.cs </Name>
//         <Created> 15/08/17 2:15:16 PM </Created>
//         <Key> ee6cf0d4-2de1-4cfd-a538-760a94735eb3 </Key>
//     </File>
//     <Summary>
//         ProfileEntity.cs
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
    public class ProfileMapEntity: IEntityTypeConfiguration<ProfileEntity>
	{
        public void Map(EntityTypeBuilder<ProfileEntity> builder)
        {
            builder.ToTable(nameof(ProfileMapEntity));
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Version).IsRowVersion();
        }
    }
}