using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Monkey.Data.EF.Migrations
{
    public partial class Image : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AvatarId",
                table: "Profile",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Image",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
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
                    MineType = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Path = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Image", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Profile_AvatarId",
                table: "Profile",
                column: "AvatarId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Profile_Image_AvatarId",
                table: "Profile",
                column: "AvatarId",
                principalTable: "Image",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Profile_Image_AvatarId",
                table: "Profile");

            migrationBuilder.DropTable(
                name: "Image");

            migrationBuilder.DropIndex(
                name: "IX_Profile_AvatarId",
                table: "Profile");

            migrationBuilder.DropColumn(
                name: "AvatarId",
                table: "Profile");
        }
    }
}
