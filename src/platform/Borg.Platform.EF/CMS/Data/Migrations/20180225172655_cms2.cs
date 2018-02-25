using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Borg.Platform.EF.CMS.Data.Migrations
{
    public partial class cms2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaxonomyStates_CategoryStates_Id",
                schema: "cms",
                table: "TaxonomyStates");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryStates_TaxonomyStates_Id",
                schema: "cms",
                table: "CategoryStates",
                column: "Id",
                principalSchema: "cms",
                principalTable: "TaxonomyStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryStates_TaxonomyStates_Id",
                schema: "cms",
                table: "CategoryStates");

            migrationBuilder.AddForeignKey(
                name: "FK_TaxonomyStates_CategoryStates_Id",
                schema: "cms",
                table: "TaxonomyStates",
                column: "Id",
                principalSchema: "cms",
                principalTable: "CategoryStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
