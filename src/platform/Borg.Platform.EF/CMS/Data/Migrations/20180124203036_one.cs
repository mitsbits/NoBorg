using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Borg.Platform.EF.CMS.Data.Migrations
{
    public partial class one : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "cms");

            migrationBuilder.CreateSequence<int>(
                name: "ComponentStatesSQC",
                schema: "cms");

            migrationBuilder.CreateTable(
                name: "ComponentStates",
                schema: "cms",
                columns: table => new
                {
                    Body = table.Column<string>(nullable: true),
                    Slug = table.Column<string>(maxLength: 1024, nullable: true, defaultValue: ""),
                    Title = table.Column<string>(maxLength: 1024, nullable: true, defaultValue: ""),
                    Id = table.Column<int>(nullable: false, defaultValueSql: "NEXT VALUE FOR cms.ComponentStatesSQC"),
                    Discriminator = table.Column<string>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsPublished = table.Column<bool>(nullable: false),
                    Code = table.Column<string>(unicode: false, maxLength: 1024, nullable: true, defaultValue: ""),
                    HtmlSnippet = table.Column<string>(nullable: true, defaultValue: ""),
                    Tag = table.Column<string>(maxLength: 1024, nullable: true, defaultValue: ""),
                    TagNormalized = table.Column<string>(maxLength: 1024, nullable: true, defaultValue: ""),
                    TagSlug = table.Column<string>(unicode: false, maxLength: 1024, nullable: true, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentStates", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateTable(
                name: "ArticleTagStates",
                schema: "cms",
                columns: table => new
                {
                    ArticleId = table.Column<int>(nullable: false),
                    TagId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleTagStates", x => new { x.ArticleId, x.TagId })
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_ArticleTagStates_ComponentStates_ArticleId",
                        column: x => x.ArticleId,
                        principalSchema: "cms",
                        principalTable: "ComponentStates",
                        principalColumn: "Id",
                        onUpdate: ReferentialAction.NoAction,
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ArticleTagStates_ComponentStates_TagId",
                        column: x => x.TagId,
                        principalSchema: "cms",
                        principalTable: "ComponentStates",
                        principalColumn: "Id",
                        onUpdate: ReferentialAction.NoAction,
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticleTagStates_TagId",
                schema: "cms",
                table: "ArticleTagStates",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Article_Title",
                schema: "cms",
                table: "ComponentStates",
                column: "Title",
                unique: true,
                filter: "[Title] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_HtmlSnippet_Code",
                schema: "cms",
                table: "ComponentStates",
                column: "Code",
                unique: true,
                filter: "[Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Tag_Tag",
                schema: "cms",
                table: "ComponentStates",
                column: "Tag",
                unique: true,
                filter: "[Tag] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Tag_TagNormalized",
                schema: "cms",
                table: "ComponentStates",
                column: "TagNormalized",
                unique: true,
                filter: "[TagNormalized] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleTagStates",
                schema: "cms");

            migrationBuilder.DropTable(
                name: "ComponentStates",
                schema: "cms");

            migrationBuilder.DropSequence(
                name: "ComponentStatesSQC",
                schema: "cms");
        }
    }
}
