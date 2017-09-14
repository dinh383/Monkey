using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Monkey.Data.EF.Migrations
{
    public partial class temp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Client_No",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "No",
                table: "Client");

            migrationBuilder.AlterColumn<string>(
                name: "NameNorm",
                table: "Client",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Client_NameNorm",
                table: "Client",
                column: "NameNorm");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Client_NameNorm",
                table: "Client");

            migrationBuilder.AlterColumn<string>(
                name: "NameNorm",
                table: "Client",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "No",
                table: "Client",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Client_No",
                table: "Client",
                column: "No");
        }
    }
}
