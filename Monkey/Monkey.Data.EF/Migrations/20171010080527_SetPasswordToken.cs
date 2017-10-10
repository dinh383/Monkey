using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Monkey.Data.EF.Migrations
{
    public partial class SetPasswordToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SetPasswordToken",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "SetPasswordTokenExpireOn",
                table: "User",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SetPasswordToken",
                table: "User");

            migrationBuilder.DropColumn(
                name: "SetPasswordTokenExpireOn",
                table: "User");
        }
    }
}
