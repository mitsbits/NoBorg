using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Borg.Cms.Basic.PlugIns.Documents.Data.Migrations
{
    public partial class seven : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DocumentCheckOutStates",
                schema: "documents",
                table: "DocumentCheckOutStates");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DocumentCheckOutStates",
                schema: "documents",
                table: "DocumentCheckOutStates",
                columns: new[] { "DocumentId", "CheckedOutBy", "CheckedOutOn" })
                .Annotation("SqlServer:Clustered", true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DocumentCheckOutStates",
                schema: "documents",
                table: "DocumentCheckOutStates");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DocumentCheckOutStates",
                schema: "documents",
                table: "DocumentCheckOutStates",
                columns: new[] { "DocumentId", "CheckedOutBy" })
                .Annotation("SqlServer:Clustered", true);
        }
    }
}
