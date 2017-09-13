using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Monkey.Data.EF;

namespace Monkey.Data.EF.Migrations
{
    [DbContext(typeof(DbContext))]
    [Migration("20170913154350_UserRefreshToken")]
    partial class UserRefreshToken
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Monkey.Data.Entities.User.UserEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BannedRemark");

                    b.Property<DateTimeOffset?>("BannedTime");

                    b.Property<int?>("CreatedBy");

                    b.Property<DateTimeOffset>("CreatedTime");

                    b.Property<int?>("DeletedBy");

                    b.Property<DateTimeOffset?>("DeletedTime");

                    b.Property<string>("Email");

                    b.Property<DateTimeOffset?>("EmailConfirmedTime");

                    b.Property<string>("EmailNorm");

                    b.Property<string>("GlobalId")
                        .IsRequired()
                        .HasMaxLength(68);

                    b.Property<int?>("LastUpdatedBy");

                    b.Property<DateTimeOffset?>("LastUpdatedTime");

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PasswordSalt");

                    b.Property<string>("Phone");

                    b.Property<DateTimeOffset?>("PhoneConfirmedTime");

                    b.Property<string>("UserName");

                    b.Property<string>("UserNameNorm");

                    b.HasKey("Id");

                    b.HasIndex("DeletedTime");

                    b.HasIndex("GlobalId");

                    b.HasIndex("Id");

                    b.HasIndex("PasswordHash");

                    b.HasIndex("UserNameNorm");

                    b.ToTable("User");
                });
        }
    }
}
