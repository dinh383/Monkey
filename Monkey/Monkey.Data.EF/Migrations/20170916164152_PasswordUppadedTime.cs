using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Monkey.Data.EF.Migrations
{
    public partial class PasswordUppadedTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                table: "User");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "PasswordLastUpdatedTime",
                table: "User",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordLastUpdatedTime",
                table: "User");

            migrationBuilder.AddColumn<string>(
                name: "PasswordSalt",
                table: "User",
                nullable: true);
        }
    }
}
