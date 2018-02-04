using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Borg.Cms.Basic.PlugIns.Documents.Data.Migrations
{
    public partial class four : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "documents",
                table: "DocumentStates",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublished",
                schema: "documents",
                table: "DocumentStates",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Document_Owners",
                schema: "documents",
                table: "DocumentOwnerStates",
                column: "DocumentId",
                principalSchema: "documents",
                principalTable: "DocumentStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Document_Owners",
                schema: "documents",
                table: "DocumentOwnerStates");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "documents",
                table: "DocumentStates");

            migrationBuilder.DropColumn(
                name: "IsPublished",
                schema: "documents",
                table: "DocumentStates");
        }
    }
}
