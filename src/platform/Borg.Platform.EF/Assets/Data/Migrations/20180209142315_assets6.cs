using Microsoft.EntityFrameworkCore.Migrations;

namespace Borg.Platform.EF.Assets.Data.Migrations
{
    public partial class assets6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Version_FileRecordId",
                schema: "assets",
                table: "VersionRecords");

            migrationBuilder.CreateIndex(
                name: "IX_Version_FileRecordId",
                schema: "assets",
                table: "VersionRecords",
                column: "FileRecordId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Version_FileRecordId",
                schema: "assets",
                table: "VersionRecords");

            migrationBuilder.CreateIndex(
                name: "IX_Version_FileRecordId",
                schema: "assets",
                table: "VersionRecords",
                column: "FileRecordId",
                unique: true);
        }
    }
}