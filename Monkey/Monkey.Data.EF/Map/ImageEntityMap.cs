#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Entity Map </Project>
//     <File>
//         <Name> ImageEntityMap.cs </Name>
//         <Created> 10/10/17 8:05:16 PM </Created>
//         <Key> dc7ed67d-3861-49f4-a3bb-c4a29fcd2733 </Key>
//     </File>
//     <Summary>
//         ImageEntityMap.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Monkey.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Puppy.EF.Maps;

namespace Monkey.Data.EF.Map
{
    public class ImageEntityMap : EntityTypeConfiguration<ImageEntity>
    {
        public override void Map(EntityTypeBuilder<ImageEntity> builder)
        {
            base.Map(builder);

            builder.ToTable(nameof(ImageEntity));
        }
    }
}