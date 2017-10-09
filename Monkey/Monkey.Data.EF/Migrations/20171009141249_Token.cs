using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Monkey.Data.EF.Migrations
{
    public partial class Token : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConfirmEmailToken",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ConfirmEmailTokenExpireOn",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConfirmPhoneToken",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ConfirmPhoneTokenExpireOn",
                table: "User",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmEmailToken",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ConfirmEmailTokenExpireOn",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ConfirmPhoneToken",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ConfirmPhoneTokenExpireOn",
                table: "User");
        }
    }
}
