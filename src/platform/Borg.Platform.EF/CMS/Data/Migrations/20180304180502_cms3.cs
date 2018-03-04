using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Borg.Platform.EF.CMS.Data.Migrations
{
    public partial class cms3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoryComponentAssociationStates",
                schema: "cms",
                columns: table => new
                {
                    CategoryId = table.Column<int>(nullable: false),
                    ComponentId = table.Column<int>(nullable: false),
                    IsPrimary = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryComponentAssociationStates", x => new { x.CategoryId, x.ComponentId })
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_Categories_CategoryComponentAssociation",
                        column: x => x.CategoryId,
                        principalSchema: "cms",
                        principalTable: "CategoryStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Components_CategoryComponentAssociation",
                        column: x => x.ComponentId,
                        principalSchema: "cms",
                        principalTable: "ComponentStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryComponentAssociationStates_ComponentId",
                schema: "cms",
                table: "CategoryComponentAssociationStates",
                column: "ComponentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryComponentAssociationStates",
                schema: "cms");
        }
    }
}
