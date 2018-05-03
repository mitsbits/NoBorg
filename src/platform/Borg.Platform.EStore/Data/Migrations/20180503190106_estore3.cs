using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Borg.Platform.EStore.Data.Migrations
{
    public partial class estore3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaxonomyState",
                schema: "estore",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    LanguageCode = table.Column<string>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    Depth = table.Column<int>(nullable: false),
                    ParentId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Weight = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxonomyState", x => new { x.Id, x.LanguageCode })
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_TaxonomyState_ComponentState_Id",
                        column: x => x.Id,
                        principalSchema: "estore",
                        principalTable: "ComponentState",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaxonomyState_Id",
                schema: "estore",
                table: "TaxonomyState",
                column: "Id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaxonomyState",
                schema: "estore");
        }
    }
}
