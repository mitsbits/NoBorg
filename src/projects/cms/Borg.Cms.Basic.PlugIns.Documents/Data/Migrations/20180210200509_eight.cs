using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Borg.Cms.Basic.PlugIns.Documents.Data.Migrations
{
    public partial class eight : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MimeTypeGroupingStates",
                schema: "documents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true, defaultValue: ""),
                    Name = table.Column<string>(maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MimeTypeGroupingStates", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateTable(
                name: "MimeTypeGroupingExtensionStates",
                schema: "documents",
                columns: table => new
                {
                    MimeTypeGroupingId = table.Column<int>(nullable: false),
                    Extension = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MimeTypeGroupingExtensionStates", x => new { x.MimeTypeGroupingId, x.Extension })
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_Grouping_Extension",
                        column: x => x.MimeTypeGroupingId,
                        principalSchema: "documents",
                        principalTable: "MimeTypeGroupingStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MimeTypeGrouping_Name",
                schema: "documents",
                table: "MimeTypeGroupingStates",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MimeTypeGroupingExtensionStates",
                schema: "documents");

            migrationBuilder.DropTable(
                name: "MimeTypeGroupingStates",
                schema: "documents");
        }
    }
}
