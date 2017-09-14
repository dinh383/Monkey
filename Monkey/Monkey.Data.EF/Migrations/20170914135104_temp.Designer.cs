using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Monkey.Data.EF;
using Monkey.Core.Constants;
using Puppy.Web.HttpRequestDetection.Device;

namespace Monkey.Data.EF.Migrations
{
    [DbContext(typeof(DbContext))]
    [Migration("20170914135104_temp")]
    partial class temp
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Monkey.Data.Entities.Client.ClientEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BannedRemark");

                    b.Property<DateTimeOffset?>("BannedTime");

                    b.Property<int?>("CreatedBy");

                    b.Property<DateTimeOffset>("CreatedTime");

                    b.Property<int?>("DeletedBy");

                    b.Property<DateTimeOffset?>("DeletedTime");

                    b.Property<string>("Domain");

                    b.Property<string>("GlobalId")
                        .IsRequired()
                        .HasMaxLength(68);

                    b.Property<int?>("LastUpdatedBy");

                    b.Property<DateTimeOffset?>("LastUpdatedTime");

                    b.Property<string>("Name");

                    b.Property<string>("NameNorm");

                    b.Property<string>("Secret");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.HasIndex("DeletedTime");

                    b.HasIndex("GlobalId");

                    b.HasIndex("Id");

                    b.HasIndex("NameNorm");

                    b.HasIndex("Secret");

                    b.ToTable("Client");
                });

            modelBuilder.Entity("Monkey.Data.Entities.User.PermissionEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CreatedBy");

                    b.Property<DateTimeOffset>("CreatedTime");

                    b.Property<int?>("DeletedBy");

                    b.Property<DateTimeOffset?>("DeletedTime");

                    b.Property<string>("GlobalId")
                        .IsRequired()
                        .HasMaxLength(68);

                    b.Property<int?>("LastUpdatedBy");

                    b.Property<DateTimeOffset?>("LastUpdatedTime");

                    b.Property<int>("Permission");

                    b.Property<int>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("DeletedTime");

                    b.HasIndex("GlobalId");

                    b.HasIndex("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Permission");
                });

            modelBuilder.Entity("Monkey.Data.Entities.User.ProfileEntity", b =>
                {
                    b.Property<int>("Id");

                    b.Property<int?>("CreatedBy");

                    b.Property<DateTimeOffset>("CreatedTime");

                    b.Property<int?>("DeletedBy");

                    b.Property<DateTimeOffset?>("DeletedTime");

                    b.Property<string>("FirstName");

                    b.Property<string>("FirstNameNorm");

                    b.Property<string>("FullName");

                    b.Property<string>("FullNameNorm");

                    b.Property<string>("GlobalId")
                        .IsRequired()
                        .HasMaxLength(68);

                    b.Property<string>("LastName");

                    b.Property<string>("LastNameNorm");

                    b.Property<int?>("LastUpdatedBy");

                    b.Property<DateTimeOffset?>("LastUpdatedTime");

                    b.HasKey("Id");

                    b.HasIndex("DeletedTime");

                    b.HasIndex("GlobalId");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("Profile");
                });

            modelBuilder.Entity("Monkey.Data.Entities.User.RefreshTokenEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AccuracyRadius");

                    b.Property<string>("BrowserName");

                    b.Property<string>("BrowserVersion");

                    b.Property<int?>("CityGeoNameId");

                    b.Property<string>("CityName");

                    b.Property<int>("ClientId");

                    b.Property<string>("ContinentCode");

                    b.Property<int?>("ContinentGeoNameId");

                    b.Property<string>("ContinentName");

                    b.Property<int?>("CountryGeoNameId");

                    b.Property<string>("CountryIsoCode");

                    b.Property<string>("CountryName");

                    b.Property<int?>("CreatedBy");

                    b.Property<DateTimeOffset>("CreatedTime");

                    b.Property<int?>("DeletedBy");

                    b.Property<DateTimeOffset?>("DeletedTime");

                    b.Property<string>("DeviceHash");

                    b.Property<int>("DeviceType");

                    b.Property<string>("EngineName");

                    b.Property<string>("EngineVersion");

                    b.Property<DateTimeOffset?>("ExpireOn");

                    b.Property<string>("GlobalId")
                        .IsRequired()
                        .HasMaxLength(68);

                    b.Property<string>("IpAddress");

                    b.Property<int?>("LastUpdatedBy");

                    b.Property<DateTimeOffset?>("LastUpdatedTime");

                    b.Property<double?>("Latitude");

                    b.Property<double?>("Longitude");

                    b.Property<string>("MarkerName");

                    b.Property<string>("MarkerVersion");

                    b.Property<string>("OsName");

                    b.Property<string>("OsVersion");

                    b.Property<string>("PostalCode");

                    b.Property<string>("RefreshToken");

                    b.Property<string>("TimeZone");

                    b.Property<int>("TotalUsage");

                    b.Property<string>("UserAgent");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("DeletedTime");

                    b.HasIndex("GlobalId");

                    b.HasIndex("Id");

                    b.HasIndex("RefreshToken");

                    b.HasIndex("UserId");

                    b.ToTable("RefreshToken");
                });

            modelBuilder.Entity("Monkey.Data.Entities.User.RoleEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CreatedBy");

                    b.Property<DateTimeOffset>("CreatedTime");

                    b.Property<int?>("DeletedBy");

                    b.Property<DateTimeOffset?>("DeletedTime");

                    b.Property<string>("Description");

                    b.Property<double>("DisplayOrder");

                    b.Property<string>("GlobalId")
                        .IsRequired()
                        .HasMaxLength(68);

                    b.Property<int?>("LastUpdatedBy");

                    b.Property<DateTimeOffset?>("LastUpdatedTime");

                    b.Property<string>("Name");

                    b.Property<string>("NameNorm");

                    b.HasKey("Id");

                    b.HasIndex("DeletedTime");

                    b.HasIndex("GlobalId");

                    b.HasIndex("Id");

                    b.ToTable("Role");
                });

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

                    b.Property<int?>("RoleId");

                    b.Property<string>("UserName");

                    b.Property<string>("UserNameNorm");

                    b.HasKey("Id");

                    b.HasIndex("DeletedTime");

                    b.HasIndex("EmailNorm");

                    b.HasIndex("GlobalId");

                    b.HasIndex("Id");

                    b.HasIndex("PasswordHash");

                    b.HasIndex("Phone");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserNameNorm");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Monkey.Data.Entities.User.PermissionEntity", b =>
                {
                    b.HasOne("Monkey.Data.Entities.User.RoleEntity", "Role")
                        .WithMany("Permissions")
                        .HasForeignKey("RoleId");
                });

            modelBuilder.Entity("Monkey.Data.Entities.User.ProfileEntity", b =>
                {
                    b.HasOne("Monkey.Data.Entities.User.UserEntity", "User")
                        .WithOne("Profile")
                        .HasForeignKey("Monkey.Data.Entities.User.ProfileEntity", "Id");
                });

            modelBuilder.Entity("Monkey.Data.Entities.User.RefreshTokenEntity", b =>
                {
                    b.HasOne("Monkey.Data.Entities.Client.ClientEntity", "Client")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("ClientId");

                    b.HasOne("Monkey.Data.Entities.User.UserEntity", "User")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Monkey.Data.Entities.User.UserEntity", b =>
                {
                    b.HasOne("Monkey.Data.Entities.User.RoleEntity", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId");
                });
        }
    }
}
