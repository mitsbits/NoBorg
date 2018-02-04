using Microsoft.EntityFrameworkCore.Migrations;

namespace Borg.Cms.Basic.PlugIns.Documents.Data.Migrations
{
    public partial class six : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_Documents_CheckOuts",
                schema: "documents",
                table: "DocumentCheckOutStates",
                column: "DocumentId",
                principalSchema: "documents",
                principalTable: "DocumentStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_CheckOuts",
                schema: "documents",
                table: "DocumentCheckOutStates");
        }
    }
}