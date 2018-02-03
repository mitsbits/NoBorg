using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Borg.Cms.Basic.PlugIns.Documents.Data.Migrations
{
    public partial class two : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DoecumentId",
                schema: "documents",
                table: "DocumentOwnerStates",
                newName: "DocumentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DocumentId",
                schema: "documents",
                table: "DocumentOwnerStates",
                newName: "DoecumentId");
        }
    }
}
