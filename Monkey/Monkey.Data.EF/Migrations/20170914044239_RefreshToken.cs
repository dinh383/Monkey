using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Monkey.Data.EF.Migrations
{
    public partial class RefreshToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BrowserFullInfo",
                table: "RefreshToken");

            migrationBuilder.DropColumn(
                name: "EngineFullInfo",
                table: "RefreshToken");

            migrationBuilder.DropColumn(
                name: "MarkerFullInfo",
                table: "RefreshToken");

            migrationBuilder.DropColumn(
                name: "OsFullInfo",
                table: "RefreshToken");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "RefreshToken");

            migrationBuilder.AddColumn<int>(
                name: "DeviceType",
                table: "RefreshToken",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeviceType",
                table: "RefreshToken");

            migrationBuilder.AddColumn<string>(
                name: "BrowserFullInfo",
                table: "RefreshToken",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EngineFullInfo",
                table: "RefreshToken",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MarkerFullInfo",
                table: "RefreshToken",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OsFullInfo",
                table: "RefreshToken",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "RefreshToken",
                nullable: false,
                defaultValue: 0);
        }
    }
}
