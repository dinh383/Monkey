using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Monkey.Data.EF.Migrations
{
    public partial class UserRefreshToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BannedRemark",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "BannedTime",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "EmailConfirmedTime",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailNorm",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "PhoneConfirmedTime",
                table: "User",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BannedRemark",
                table: "User");

            migrationBuilder.DropColumn(
                name: "BannedTime",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "User");

            migrationBuilder.DropColumn(
                name: "EmailConfirmedTime",
                table: "User");

            migrationBuilder.DropColumn(
                name: "EmailNorm",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "User");

            migrationBuilder.DropColumn(
                name: "PhoneConfirmedTime",
                table: "User");
        }
    }
}
