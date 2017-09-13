#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Entity Map </Project>
//     <File>
//         <Name> ProfileEntityMap.cs </Name>
//         <Created> 13/09/17 11:39:05 PM </Created>
//         <Key> 0ae63de3-6af7-47b1-8385-a22402116328 </Key>
//     </File>
//     <Summary>
//         ProfileEntityMap.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Monkey.Data.Entities.User;
using Puppy.EF.Maps;

namespace Monkey.Data.EF.Map
{
    public class ProfileEntityMap : EntityTypeConfiguration<ProfileEntity>
    {
        public override void Map(EntityTypeBuilder<ProfileEntity> builder)
        {
            base.Map(builder);

            builder.ToTable(nameof(ProfileEntity));
        }
    }
}