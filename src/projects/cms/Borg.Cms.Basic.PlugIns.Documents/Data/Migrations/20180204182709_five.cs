using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Borg.Cms.Basic.PlugIns.Documents.Data.Migrations
{
    public partial class five : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DocumentCheckOutStates",
                schema: "documents",
                columns: table => new
                {
                    DocumentId = table.Column<int>(nullable: false),
                    CheckedOutBy = table.Column<string>(unicode: false, maxLength: 256, nullable: false),
                    CheckOutVersion = table.Column<int>(nullable: false),
                    CheckedIn = table.Column<bool>(nullable: false),
                    CheckedInBy = table.Column<string>(unicode: false, maxLength: 256, nullable: true),
                    CheckedOutOn = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CheckedinOn = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentCheckOutStates", x => new { x.DocumentId, x.CheckedOutBy })
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentCheckOutState_CheckedOutOn",
                schema: "documents",
                table: "DocumentCheckOutStates",
                column: "CheckedOutOn");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentCheckOutStates",
                schema: "documents");
        }
    }
}