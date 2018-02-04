using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Borg.Cms.Basic.PlugIns.Documents.Data.Migrations
{
    public partial class three : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "AssociatedOn",
                schema: "documents",
                table: "DocumentOwnerStates",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentOwnerState_AssociatedOn",
                schema: "documents",
                table: "DocumentOwnerStates",
                column: "AssociatedOn");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DocumentOwnerState_AssociatedOn",
                schema: "documents",
                table: "DocumentOwnerStates");

            migrationBuilder.DropColumn(
                name: "AssociatedOn",
                schema: "documents",
                table: "DocumentOwnerStates");
        }
    }
}
