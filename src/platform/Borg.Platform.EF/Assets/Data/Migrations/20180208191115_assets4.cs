using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Borg.Platform.EF.Assets.Data.Migrations
{
    public partial class assets4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Extension",
                schema: "assets",
                table: "FileRecords",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_FileRecord_Extension",
                schema: "assets",
                table: "FileRecords",
                column: "Extension");

            migrationBuilder.AddForeignKey(
                name: "FK_MimeTypes_Records",
                schema: "assets",
                table: "FileRecords",
                column: "Extension",
                principalSchema: "assets",
                principalTable: "MimeTypeRecords",
                principalColumn: "Extension",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MimeTypes_Records",
                schema: "assets",
                table: "FileRecords");

            migrationBuilder.DropIndex(
                name: "IX_FileRecord_Extension",
                schema: "assets",
                table: "FileRecords");

            migrationBuilder.DropColumn(
                name: "Extension",
                schema: "assets",
                table: "FileRecords");
        }
    }
}
