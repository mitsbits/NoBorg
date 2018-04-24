using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Borg.Platform.Documents.Data.Migrations
{
    public partial class doc1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "documents");

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
                schema: "documents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false, defaultValueSql: "NEXT VALUE FOR assets.AssetsSQC"),
                    CurrentVersion = table.Column<int>(nullable: false, defaultValueSql: "0"),
                    DocumentBehaviourState = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 512, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetRecords", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateTable(
                name: "DocumentStates",
                schema: "documents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    IsPublished = table.Column<bool>(nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentStates", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateTable(
                name: "MimeTypeGroupingStates",
                schema: "documents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true, defaultValue: ""),
                    Name = table.Column<string>(maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MimeTypeGroupingStates", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateTable(
                name: "MimeTypeRecords",
                schema: "documents",
                columns: table => new
                {
                    Extension = table.Column<string>(maxLength: 64, nullable: false),
                    MimeType = table.Column<string>(maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MimeTypeRecords", x => x.Extension)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateTable(
                name: "DocumentCheckOutStates",
                schema: "documents",
                columns: table => new
                {
                    DocumentId = table.Column<int>(nullable: false),
                    CheckedOutBy = table.Column<string>(unicode: false, maxLength: 256, nullable: false),
                    CheckedOutOn = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CheckOutVersion = table.Column<int>(nullable: false),
                    CheckedIn = table.Column<bool>(nullable: false),
                    CheckedInBy = table.Column<string>(unicode: false, maxLength: 256, nullable: true),
                    CheckedinOn = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentCheckOutStates", x => new { x.DocumentId, x.CheckedOutBy, x.CheckedOutOn })
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_Documents_CheckOuts",
                        column: x => x.DocumentId,
                        principalSchema: "documents",
                        principalTable: "DocumentStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocumentOwnerStates",
                schema: "documents",
                columns: table => new
                {
                    DocumentId = table.Column<int>(nullable: false),
                    Owner = table.Column<string>(unicode: false, maxLength: 256, nullable: false),
                    AssociatedOn = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentOwnerStates", x => new { x.DocumentId, x.Owner })
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_Document_Owners",
                        column: x => x.DocumentId,
                        principalSchema: "documents",
                        principalTable: "DocumentStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MimeTypeGroupingExtensionStates",
                schema: "documents",
                columns: table => new
                {
                    MimeTypeGroupingId = table.Column<int>(nullable: false),
                    Extension = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MimeTypeGroupingExtensionStates", x => new { x.MimeTypeGroupingId, x.Extension })
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_Grouping_Extension",
                        column: x => x.MimeTypeGroupingId,
                        principalSchema: "documents",
                        principalTable: "MimeTypeGroupingStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FileRecords",
                schema: "documents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false, defaultValueSql: "NEXT VALUE FOR assets.FilesSQC"),
                    CreationDate = table.Column<DateTime>(nullable: false, defaultValueSql: "GetUtcDate()"),
                    Extension = table.Column<string>(maxLength: 64, nullable: false, defaultValue: ""),
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
                    table.ForeignKey(
                        name: "FK_MimeTypes_Records",
                        column: x => x.Extension,
                        principalSchema: "documents",
                        principalTable: "MimeTypeRecords",
                        principalColumn: "Extension",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VersionRecords",
                schema: "documents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false, defaultValueSql: "NEXT VALUE FOR assets.VersionsSQC"),
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
                        principalSchema: "documents",
                        principalTable: "AssetRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Version_File",
                        column: x => x.FileRecordId,
                        principalSchema: "documents",
                        principalTable: "FileRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentCheckOutState_CheckedOutOn",
                schema: "documents",
                table: "DocumentCheckOutStates",
                column: "CheckedOutOn");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentOwnerState_AssociatedOn",
                schema: "documents",
                table: "DocumentOwnerStates",
                column: "AssociatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_FileRecord_Extension",
                schema: "documents",
                table: "FileRecords",
                column: "Extension");

            migrationBuilder.CreateIndex(
                name: "IX_File_FullPath",
                schema: "documents",
                table: "FileRecords",
                column: "FullPath");

            migrationBuilder.CreateIndex(
                name: "IX_MimeTypeGrouping_Name",
                schema: "documents",
                table: "MimeTypeGroupingStates",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Version_FileRecordId",
                schema: "documents",
                table: "VersionRecords",
                column: "FileRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_Version_Version",
                schema: "documents",
                table: "VersionRecords",
                column: "Version");

            migrationBuilder.CreateIndex(
                name: "PK_Version_Asset",
                schema: "documents",
                table: "VersionRecords",
                columns: new[] { "AssetRecordId", "Version", "FileRecordId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentCheckOutStates",
                schema: "documents");

            migrationBuilder.DropTable(
                name: "DocumentOwnerStates",
                schema: "documents");

            migrationBuilder.DropTable(
                name: "MimeTypeGroupingExtensionStates",
                schema: "documents");

            migrationBuilder.DropTable(
                name: "VersionRecords",
                schema: "documents");

            migrationBuilder.DropTable(
                name: "DocumentStates",
                schema: "documents");

            migrationBuilder.DropTable(
                name: "MimeTypeGroupingStates",
                schema: "documents");

            migrationBuilder.DropTable(
                name: "AssetRecords",
                schema: "documents");

            migrationBuilder.DropTable(
                name: "FileRecords",
                schema: "documents");

            migrationBuilder.DropTable(
                name: "MimeTypeRecords",
                schema: "documents");

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
