using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Borg.Platform.EF.Assets.Data.Migrations
{
    public partial class version_asset_index : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VersionRecords_AssetRecordId",
                schema: "assets",
                table: "VersionRecords");

            migrationBuilder.CreateIndex(
                name: "PK_Version_Asset",
                schema: "assets",
                table: "VersionRecords",
                columns: new[] { "AssetRecordId", "Version" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "PK_Version_Asset",
                schema: "assets",
                table: "VersionRecords");

            migrationBuilder.CreateIndex(
                name: "IX_VersionRecords_AssetRecordId",
                schema: "assets",
                table: "VersionRecords",
                column: "AssetRecordId");
        }
    }
}
