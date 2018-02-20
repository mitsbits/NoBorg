using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Borg.Platform.EF.CMS.Data.Migrations
{
    public partial class fourteen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MimeType",
                schema: "cms",
                table: "ComponentDocumentAssociationStates",
                maxLength: 256,
                nullable: false,
                defaultValue: "application/octet-stream");

            migrationBuilder.AddColumn<string>(
                name: "Uri",
                schema: "cms",
                table: "ComponentDocumentAssociationStates",
                maxLength: 1024,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MimeType",
                schema: "cms",
                table: "ComponentDocumentAssociationStates");

            migrationBuilder.DropColumn(
                name: "Uri",
                schema: "cms",
                table: "ComponentDocumentAssociationStates");
        }
    }
}
