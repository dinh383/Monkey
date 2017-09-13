using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Monkey.Data.EF.Migrations
{
    public partial class rolepermission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "User",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Profile",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
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
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccuracyRadius = table.Column<int>(nullable: true),
                    BrowserFullInfo = table.Column<string>(nullable: true),
                    BrowserName = table.Column<string>(nullable: true),
                    BrowserVersion = table.Column<string>(nullable: true),
                    CityGeoNameId = table.Column<int>(nullable: true),
                    CityName = table.Column<string>(nullable: true),
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
                    EngineFullInfo = table.Column<string>(nullable: true),
                    EngineName = table.Column<string>(nullable: true),
                    EngineVersion = table.Column<string>(nullable: true),
                    ExpireOn = table.Column<DateTimeOffset>(nullable: true),
                    GlobalId = table.Column<string>(maxLength: 68, nullable: false),
                    IpAddress = table.Column<string>(nullable: true),
                    LastUpdatedBy = table.Column<int>(nullable: true),
                    LastUpdatedTime = table.Column<DateTimeOffset>(nullable: true),
                    Latitude = table.Column<double>(nullable: true),
                    Longitude = table.Column<double>(nullable: true),
                    MarkerFullInfo = table.Column<string>(nullable: true),
                    MarkerName = table.Column<string>(nullable: true),
                    MarkerVersion = table.Column<string>(nullable: true),
                    OsFullInfo = table.Column<string>(nullable: true),
                    OsName = table.Column<string>(nullable: true),
                    OsVersion = table.Column<string>(nullable: true),
                    PostalCode = table.Column<string>(nullable: true),
                    RefreshToken = table.Column<string>(nullable: true),
                    TimeZone = table.Column<string>(nullable: true),
                    TotalUsage = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    UserAgent = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshToken_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    DisplayOrder = table.Column<double>(nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "IX_User_RoleId",
                table: "User",
                column: "RoleId");

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
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_User_Role_RoleId",
                table: "User",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Role_RoleId",
                table: "User");

            migrationBuilder.DropTable(
                name: "Permission");

            migrationBuilder.DropTable(
                name: "Profile");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropIndex(
                name: "IX_User_RoleId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "User");
        }
    }
}
