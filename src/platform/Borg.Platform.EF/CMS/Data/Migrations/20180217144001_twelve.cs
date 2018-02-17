using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Borg.Platform.EF.CMS.Data.Migrations
{
    public partial class twelve : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ComponentDocumentAssociationStates",
                schema: "cms",
                columns: table => new
                {
                    ComponentId = table.Column<int>(nullable: false),
                    DocumentId = table.Column<int>(nullable: false),
                    FileId = table.Column<int>(nullable: false),
                    Version = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentDocumentAssociationStates", x => new { x.ComponentId, x.DocumentId })
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_Components_Documents",
                        column: x => x.ComponentId,
                        principalSchema: "cms",
                        principalTable: "ComponentStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComponentDocumentAssociationStates_FileId",
                schema: "cms",
                table: "ComponentDocumentAssociationStates",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentDocumentAssociationStates_Version",
                schema: "cms",
                table: "ComponentDocumentAssociationStates",
                column: "Version");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComponentDocumentAssociationStates",
                schema: "cms");
        }
    }
}
