using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Monkey.Data.EF.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BannedRemark = table.Column<string>(nullable: true),
                    BannedTime = table.Column<DateTimeOffset>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(nullable: false),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedTime = table.Column<DateTimeOffset>(nullable: true),
                    Domains = table.Column<string>(nullable: true),
                    GlobalId = table.Column<string>(maxLength: 68, nullable: false),
                    LastUpdatedBy = table.Column<int>(nullable: true),
                    LastUpdatedTime = table.Column<DateTimeOffset>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    NameNorm = table.Column<string>(nullable: true),
                    Secret = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Configuration",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(nullable: false),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedTime = table.Column<DateTimeOffset>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    GlobalId = table.Column<string>(nullable: true),
                    Group = table.Column<string>(nullable: true),
                    Key = table.Column<string>(nullable: true),
                    LastUpdatedBy = table.Column<int>(nullable: true),
                    LastUpdatedTime = table.Column<DateTimeOffset>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuration", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Image",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Caption = table.Column<string>(nullable: true),
                    ContentLength = table.Column<double>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(nullable: false),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedTime = table.Column<DateTimeOffset>(nullable: true),
                    Extension = table.Column<string>(nullable: true),
                    GlobalId = table.Column<string>(maxLength: 68, nullable: false),
                    ImageDominantHexColor = table.Column<string>(nullable: true),
                    ImageHeightPx = table.Column<int>(nullable: false),
                    ImageWidthPx = table.Column<int>(nullable: false),
                    LastUpdatedBy = table.Column<int>(nullable: true),
                    LastUpdatedTime = table.Column<DateTimeOffset>(nullable: true),
                    MimeType = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Image", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(nullable: false),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedTime = table.Column<DateTimeOffset>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    GlobalId = table.Column<string>(maxLength: 68, nullable: false),
                    LastUpdatedBy = table.Column<int>(nullable: true),
                    LastUpdatedTime = table.Column<DateTimeOffset>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    NameNorm = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(nullable: false),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedTime = table.Column<DateTimeOffset>(nullable: true),
                    GlobalId = table.Column<string>(maxLength: 68, nullable: false),
                    LastUpdatedBy = table.Column<int>(nullable: true),
                    LastUpdatedTime = table.Column<DateTimeOffset>(nullable: true),
                    Permission = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Permission_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ActiveTime = table.Column<DateTimeOffset>(nullable: true),
                    BannedRemark = table.Column<string>(nullable: true),
                    BannedTime = table.Column<DateTimeOffset>(nullable: true),
                    ConfirmEmailToken = table.Column<string>(nullable: true),
                    ConfirmEmailTokenExpireOn = table.Column<DateTimeOffset>(nullable: true),
                    ConfirmPhoneToken = table.Column<string>(nullable: true),
                    ConfirmPhoneTokenExpireOn = table.Column<DateTimeOffset>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(nullable: false),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedTime = table.Column<DateTimeOffset>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    EmailConfirmedTime = table.Column<DateTimeOffset>(nullable: true),
                    EmailNorm = table.Column<string>(nullable: true),
                    GlobalId = table.Column<string>(maxLength: 68, nullable: false),
                    LastUpdatedBy = table.Column<int>(nullable: true),
                    LastUpdatedTime = table.Column<DateTimeOffset>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    PasswordLastUpdatedTime = table.Column<DateTimeOffset>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    PhoneConfirmedTime = table.Column<DateTimeOffset>(nullable: true),
                    RoleId = table.Column<int>(nullable: true),
                    SetPasswordToken = table.Column<string>(nullable: true),
                    SetPasswordTokenExpireOn = table.Column<DateTimeOffset>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    UserNameNorm = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Profile",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    AvatarId = table.Column<int>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(nullable: false),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedTime = table.Column<DateTimeOffset>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    FirstNameNorm = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    FullNameNorm = table.Column<string>(nullable: true),
                    GlobalId = table.Column<string>(maxLength: 68, nullable: false),
                    LastName = table.Column<string>(nullable: true),
                    LastNameNorm = table.Column<string>(nullable: true),
                    LastUpdatedBy = table.Column<int>(nullable: true),
                    LastUpdatedTime = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Profile_Image_AvatarId",
                        column: x => x.AvatarId,
                        principalTable: "Image",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Profile_User_Id",
                        column: x => x.Id,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccuracyRadius = table.Column<int>(nullable: true),
                    BrowserName = table.Column<string>(nullable: true),
                    BrowserVersion = table.Column<string>(nullable: true),
                    CityGeoNameId = table.Column<int>(nullable: true),
                    CityName = table.Column<string>(nullable: true),
                    ClientId = table.Column<int>(nullable: true),
                    ContinentCode = table.Column<string>(nullable: true),
                    ContinentGeoNameId = table.Column<int>(nullable: true),
                    ContinentName = table.Column<string>(nullable: true),
                    CountryGeoNameId = table.Column<int>(nullable: true),
                    CountryIsoCode = table.Column<string>(nullable: true),
                    CountryName = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(nullable: false),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedTime = table.Column<DateTimeOffset>(nullable: true),
                    DeviceHash = table.Column<string>(nullable: true),
                    DeviceType = table.Column<int>(nullable: false),
                    EngineName = table.Column<string>(nullable: true),
                    EngineVersion = table.Column<string>(nullable: true),
                    ExpireOn = table.Column<DateTimeOffset>(nullable: true),
                    GlobalId = table.Column<string>(maxLength: 68, nullable: false),
                    IpAddress = table.Column<string>(nullable: true),
                    LastUpdatedBy = table.Column<int>(nullable: true),
                    LastUpdatedTime = table.Column<DateTimeOffset>(nullable: true),
                    Latitude = table.Column<double>(nullable: true),
                    Longitude = table.Column<double>(nullable: true),
                    MarkerName = table.Column<string>(nullable: true),
                    MarkerVersion = table.Column<string>(nullable: true),
                    OsName = table.Column<string>(nullable: true),
                    OsVersion = table.Column<string>(nullable: true),
                    PostalCode = table.Column<string>(nullable: true),
                    RefreshToken = table.Column<string>(nullable: true),
                    TimeZone = table.Column<string>(nullable: true),
                    TotalUsage = table.Column<int>(nullable: false),
                    UserAgent = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshToken_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RefreshToken_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Client_DeletedTime",
                table: "Client",
                column: "DeletedTime");

            migrationBuilder.CreateIndex(
                name: "IX_Client_GlobalId",
                table: "Client",
                column: "GlobalId");

            migrationBuilder.CreateIndex(
                name: "IX_Client_Id",
                table: "Client",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Client_NameNorm",
                table: "Client",
                column: "NameNorm");

            migrationBuilder.CreateIndex(
                name: "IX_Client_Secret",
                table: "Client",
                column: "Secret");

            migrationBuilder.CreateIndex(
                name: "IX_Image_DeletedTime",
                table: "Image",
                column: "DeletedTime");

            migrationBuilder.CreateIndex(
                name: "IX_Image_GlobalId",
                table: "Image",
                column: "GlobalId");

            migrationBuilder.CreateIndex(
                name: "IX_Image_Id",
                table: "Image",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_DeletedTime",
                table: "Permission",
                column: "DeletedTime");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_GlobalId",
                table: "Permission",
                column: "GlobalId");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_Id",
                table: "Permission",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_RoleId",
                table: "Permission",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Profile_AvatarId",
                table: "Profile",
                column: "AvatarId");

            migrationBuilder.CreateIndex(
                name: "IX_Profile_DeletedTime",
                table: "Profile",
                column: "DeletedTime");

            migrationBuilder.CreateIndex(
                name: "IX_Profile_GlobalId",
                table: "Profile",
                column: "GlobalId");

            migrationBuilder.CreateIndex(
                name: "IX_Profile_Id",
                table: "Profile",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_ClientId",
                table: "RefreshToken",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_DeletedTime",
                table: "RefreshToken",
                column: "DeletedTime");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_GlobalId",
                table: "RefreshToken",
                column: "GlobalId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_Id",
                table: "RefreshToken",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_RefreshToken",
                table: "RefreshToken",
                column: "RefreshToken");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_UserId",
                table: "RefreshToken",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Role_DeletedTime",
                table: "Role",
                column: "DeletedTime");

            migrationBuilder.CreateIndex(
                name: "IX_Role_GlobalId",
                table: "Role",
                column: "GlobalId");

            migrationBuilder.CreateIndex(
                name: "IX_Role_Id",
                table: "Role",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_User_DeletedTime",
                table: "User",
                column: "DeletedTime");

            migrationBuilder.CreateIndex(
                name: "IX_User_EmailNorm",
                table: "User",
                column: "EmailNorm");

            migrationBuilder.CreateIndex(
                name: "IX_User_GlobalId",
                table: "User",
                column: "GlobalId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Id",
                table: "User",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_User_PasswordHash",
                table: "User",
                column: "PasswordHash");

            migrationBuilder.CreateIndex(
                name: "IX_User_Phone",
                table: "User",
                column: "Phone");

            migrationBuilder.CreateIndex(
                name: "IX_User_RoleId",
                table: "User",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_User_UserNameNorm",
                table: "User",
                column: "UserNameNorm");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Configuration");

            migrationBuilder.DropTable(
                name: "Permission");

            migrationBuilder.DropTable(
                name: "Profile");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "Image");

            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}