using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Monkey.Data.EF.LogMigrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DataLog",
                columns: table => new
                {
                    LogId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DataCreatedBy = table.Column<int>(nullable: true),
                    DataCreatedTime = table.Column<DateTimeOffset>(nullable: false),
                    DataDeletedBy = table.Column<int>(nullable: true),
                    DataDeletedTime = table.Column<DateTimeOffset>(nullable: true),
                    DataGlobalId = table.Column<string>(nullable: true),
                    DataGroup = table.Column<string>(nullable: true),
                    DataId = table.Column<int>(nullable: false),
                    DataJson = table.Column<string>(nullable: true),
                    DataLastUpdatedBy = table.Column<int>(nullable: true),
                    DataLastUpdatedTime = table.Column<DateTimeOffset>(nullable: false),
                    LogCreatedBy = table.Column<int>(nullable: true),
                    LogCreatedTime = table.Column<DateTimeOffset>(nullable: false),
                    LogGlobalId = table.Column<string>(nullable: true),
                    LogHttpContextInfoJson = table.Column<string>(nullable: true),
                    LogType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataLog", x => x.LogId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DataLog_DataCreatedBy",
                table: "DataLog",
                column: "DataCreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DataLog_DataCreatedTime",
                table: "DataLog",
                column: "DataCreatedTime");

            migrationBuilder.CreateIndex(
                name: "IX_DataLog_DataDeletedBy",
                table: "DataLog",
                column: "DataDeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DataLog_DataDeletedTime",
                table: "DataLog",
                column: "DataDeletedTime");

            migrationBuilder.CreateIndex(
                name: "IX_DataLog_DataGlobalId",
                table: "DataLog",
                column: "DataGlobalId");

            migrationBuilder.CreateIndex(
                name: "IX_DataLog_DataId",
                table: "DataLog",
                column: "DataId");

            migrationBuilder.CreateIndex(
                name: "IX_DataLog_DataLastUpdatedBy",
                table: "DataLog",
                column: "DataLastUpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DataLog_DataLastUpdatedTime",
                table: "DataLog",
                column: "DataLastUpdatedTime");

            migrationBuilder.CreateIndex(
                name: "IX_DataLog_LogGlobalId",
                table: "DataLog",
                column: "LogGlobalId");

            migrationBuilder.CreateIndex(
                name: "IX_DataLog_LogId",
                table: "DataLog",
                column: "LogId");

            migrationBuilder.CreateIndex(
                name: "IX_DataLog_LogType",
                table: "DataLog",
                column: "LogType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataLog");
        }
    }
}