#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2018 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Entity Map </Project>
//     <File>
//         <Name> DataLogEntityMap.cs </Name>
//         <Created> 28/01/2018 10:57:52 PM </Created>
//         <Key> 6773530b-2810-4468-b28e-602a98ee65aa </Key>
//     </File>
//     <Summary>
//         DataLogEntityMap.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Monkey.Core.Entities.DataLog;
using Puppy.EF.Maps;

namespace Monkey.Data.EF.LogMap
{
    public class DataLogEntityMap : TypeConfiguration<DataLogEntity>
    {
        public override void Map(EntityTypeBuilder<DataLogEntity> builder)
        {
            base.Map(builder);

            builder.ToTable(nameof(DataLogEntity));

            builder.HasKey(e => e.LogId);

            builder.HasIndex(e => e.LogId);
            builder.HasIndex(e => e.LogGlobalId);
            builder.HasIndex(e => e.LogType);

            builder.HasIndex(e => e.DataId);
            builder.HasIndex(e => e.DataGlobalId);

            builder.HasIndex(e => e.DataCreatedTime);
            builder.HasIndex(e => e.DataCreatedBy);

            builder.HasIndex(e => e.DataLastUpdatedTime);
            builder.HasIndex(e => e.DataLastUpdatedBy);

            builder.HasIndex(e => e.DataDeletedTime);
            builder.HasIndex(e => e.DataDeletedBy);
        }
    }
}