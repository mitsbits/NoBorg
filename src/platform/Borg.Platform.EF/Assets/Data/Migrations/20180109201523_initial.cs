using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Borg.Platform.EF.Assets.Data.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "assets");

            migrationBuilder.CreateSequence<int>(
                name: "AssetsSQC",
                schema: "assets");

            migrationBuilder.CreateSequence<int>(
                name: "FilesSQC",
                schema: "assets");

            migrationBuilder.CreateSequence<int>(
                name: "VersionsSQC",
                schema: "assets");

            migrationBuilder.CreateTable(
                name: "AssetRecords",
                schema: "assets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CurrentVersion = table.Column<int>(nullable: false, defaultValueSql: "0"),
                    DocumentState = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 512, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetRecords", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateTable(
                name: "FileRecords",
                schema: "assets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreationDate = table.Column<DateTime>(nullable: false, defaultValueSql: "GetUtcDate()"),
                    FullPath = table.Column<string>(maxLength: 1024, nullable: false, defaultValue: ""),
                    LastRead = table.Column<DateTime>(nullable: true),
                    LastWrite = table.Column<DateTime>(nullable: false, defaultValueSql: "GetUtcDate()"),
                    MimeType = table.Column<string>(maxLength: 256, nullable: false, defaultValue: ""),
                    Name = table.Column<string>(maxLength: 512, nullable: false, defaultValue: ""),
                    SizeInBytes = table.Column<long>(nullable: false, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileRecords", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateTable(
                name: "VersionRecords",
                schema: "assets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AssetRecordId = table.Column<int>(nullable: false),
                    FileRecordId = table.Column<int>(nullable: false),
                    Version = table.Column<int>(nullable: false, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VersionRecords", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_Asset_Version",
                        column: x => x.AssetRecordId,
                        principalSchema: "assets",
                        principalTable: "AssetRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Version_File",
                        column: x => x.FileRecordId,
                        principalSchema: "assets",
                        principalTable: "FileRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_File_FullPath",
                schema: "assets",
                table: "FileRecords",
                column: "FullPath");

            migrationBuilder.CreateIndex(
                name: "IX_VersionRecords_AssetRecordId",
                schema: "assets",
                table: "VersionRecords",
                column: "AssetRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_Version_FileRecordId",
                schema: "assets",
                table: "VersionRecords",
                column: "FileRecordId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Version_Version",
                schema: "assets",
                table: "VersionRecords",
                column: "Version");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VersionRecords",
                schema: "assets");

            migrationBuilder.DropTable(
                name: "AssetRecords",
                schema: "assets");

            migrationBuilder.DropTable(
                name: "FileRecords",
                schema: "assets");

            migrationBuilder.DropSequence(
                name: "AssetsSQC",
                schema: "assets");

            migrationBuilder.DropSequence(
                name: "FilesSQC",
                schema: "assets");

            migrationBuilder.DropSequence(
                name: "VersionsSQC",
                schema: "assets");
        }
    }
}
