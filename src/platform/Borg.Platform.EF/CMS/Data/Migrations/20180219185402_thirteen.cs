using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Borg.Platform.EF.CMS.Data.Migrations
{
    public partial class thirteen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PrimaryImageDocumentId",
                schema: "cms",
                table: "PageMetadataStates",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PrimaryImageFileId",
                schema: "cms",
                table: "PageMetadataStates",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrimaryImageDocumentId",
                schema: "cms",
                table: "PageMetadataStates");

            migrationBuilder.DropColumn(
                name: "PrimaryImageFileId",
                schema: "cms",
                table: "PageMetadataStates");
        }
    }
}
