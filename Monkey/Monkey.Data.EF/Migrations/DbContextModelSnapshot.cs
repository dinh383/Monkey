using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using System;

namespace Monkey.Data.EF.Migrations
{
    [DbContext(typeof(DbContext))]
    partial class DbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Monkey.Core.Entities.Auth.ClientEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BannedRemark");

                    b.Property<DateTimeOffset?>("BannedTime");

                    b.Property<int?>("CreatedBy");

                    b.Property<DateTimeOffset>("CreatedTime");

                    b.Property<int?>("DeletedBy");

                    b.Property<DateTimeOffset?>("DeletedTime");

                    b.Property<string>("Domains");

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

            modelBuilder.Entity("Monkey.Core.Entities.Auth.PermissionEntity", b =>
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

            modelBuilder.Entity("Monkey.Core.Entities.Auth.RefreshTokenEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AccuracyRadius");

                    b.Property<string>("BrowserName");

                    b.Property<string>("BrowserVersion");

                    b.Property<int?>("CityGeoNameId");

                    b.Property<string>("CityName");

                    b.Property<int?>("ClientId");

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

            modelBuilder.Entity("Monkey.Core.Entities.Auth.RoleEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CreatedBy");

                    b.Property<DateTimeOffset>("CreatedTime");

                    b.Property<int?>("DeletedBy");

                    b.Property<DateTimeOffset?>("DeletedTime");

                    b.Property<string>("Description");

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

            modelBuilder.Entity("Monkey.Core.Entities.ImageEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("ContentLength");

                    b.Property<int?>("CreatedBy");

                    b.Property<DateTimeOffset>("CreatedTime");

                    b.Property<int?>("DeletedBy");

                    b.Property<DateTimeOffset?>("DeletedTime");

                    b.Property<string>("Extension");

                    b.Property<string>("GlobalId")
                        .IsRequired()
                        .HasMaxLength(68);

                    b.Property<string>("ImageDominantHexColor");

                    b.Property<int>("ImageHeightPx");

                    b.Property<int>("ImageWidthPx");

                    b.Property<int?>("LastUpdatedBy");

                    b.Property<DateTimeOffset?>("LastUpdatedTime");

                    b.Property<string>("MimeType");

                    b.Property<string>("Name");

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.HasIndex("DeletedTime");

                    b.HasIndex("GlobalId");

                    b.HasIndex("Id");

                    b.ToTable("Image");
                });

            modelBuilder.Entity("Monkey.Core.Entities.User.ProfileEntity", b =>
                {
                    b.Property<int>("Id");

                    b.Property<int?>("AvatarId");

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

                    b.HasIndex("AvatarId");

                    b.HasIndex("DeletedTime");

                    b.HasIndex("GlobalId");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("Profile");
                });

            modelBuilder.Entity("Monkey.Core.Entities.User.UserEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset?>("ActiveTime");

                    b.Property<string>("BannedRemark");

                    b.Property<DateTimeOffset?>("BannedTime");

                    b.Property<string>("ConfirmEmailToken");

                    b.Property<DateTimeOffset?>("ConfirmEmailTokenExpireOn");

                    b.Property<string>("ConfirmPhoneToken");

                    b.Property<DateTimeOffset?>("ConfirmPhoneTokenExpireOn");

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

                    b.Property<DateTimeOffset?>("PasswordLastUpdatedTime");

                    b.Property<string>("Phone");

                    b.Property<DateTimeOffset?>("PhoneConfirmedTime");

                    b.Property<int?>("RoleId");

                    b.Property<string>("SetPasswordToken");

                    b.Property<DateTimeOffset?>("SetPasswordTokenExpireOn");

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

            modelBuilder.Entity("Monkey.Core.Entities.Auth.PermissionEntity", b =>
                {
                    b.HasOne("Monkey.Core.Entities.Auth.RoleEntity", "Role")
                        .WithMany("Permissions")
                        .HasForeignKey("RoleId");
                });

            modelBuilder.Entity("Monkey.Core.Entities.Auth.RefreshTokenEntity", b =>
                {
                    b.HasOne("Monkey.Core.Entities.Auth.ClientEntity", "Client")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("ClientId");

                    b.HasOne("Monkey.Core.Entities.User.UserEntity", "User")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Monkey.Core.Entities.User.ProfileEntity", b =>
                {
                    b.HasOne("Monkey.Core.Entities.ImageEntity", "Avatar")
                        .WithMany()
                        .HasForeignKey("AvatarId");

                    b.HasOne("Monkey.Core.Entities.User.UserEntity", "User")
                        .WithOne("Profile")
                        .HasForeignKey("Monkey.Core.Entities.User.ProfileEntity", "Id");
                });

            modelBuilder.Entity("Monkey.Core.Entities.User.UserEntity", b =>
                {
                    b.HasOne("Monkey.Core.Entities.Auth.RoleEntity", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId");
                });
        }
    }
}