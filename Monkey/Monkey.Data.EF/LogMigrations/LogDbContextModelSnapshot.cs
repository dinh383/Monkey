﻿// <auto-generated />
using Monkey.Core.Entities.DataLog;
using Monkey.Data.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Monkey.Data.EF.LogMigrations
{
    [DbContext(typeof(LogDbContext))]
    partial class LogDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Monkey.Core.Entities.DataLog.DataLogEntity", b =>
                {
                    b.Property<int>("LogId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("DataCreatedBy");

                    b.Property<DateTimeOffset>("DataCreatedTime");

                    b.Property<int?>("DataDeletedBy");

                    b.Property<DateTimeOffset?>("DataDeletedTime");

                    b.Property<string>("DataGlobalId");

                    b.Property<string>("DataGroup");

                    b.Property<int>("DataId");

                    b.Property<string>("DataJson");

                    b.Property<int?>("DataLastUpdatedBy");

                    b.Property<DateTimeOffset>("DataLastUpdatedTime");

                    b.Property<int?>("LogCreatedBy");

                    b.Property<DateTimeOffset>("LogCreatedTime");

                    b.Property<string>("LogGlobalId");

                    b.Property<string>("LogHttpContextInfoJson");

                    b.Property<int>("LogType");

                    b.HasKey("LogId");

                    b.HasIndex("DataCreatedBy");

                    b.HasIndex("DataCreatedTime");

                    b.HasIndex("DataDeletedBy");

                    b.HasIndex("DataDeletedTime");

                    b.HasIndex("DataGlobalId");

                    b.HasIndex("DataId");

                    b.HasIndex("DataLastUpdatedBy");

                    b.HasIndex("DataLastUpdatedTime");

                    b.HasIndex("LogGlobalId");

                    b.HasIndex("LogId");

                    b.HasIndex("LogType");

                    b.ToTable("DataLog");
                });
#pragma warning restore 612, 618
        }
    }
}
