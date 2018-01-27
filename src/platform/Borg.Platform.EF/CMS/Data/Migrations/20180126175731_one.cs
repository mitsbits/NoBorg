using Microsoft.EntityFrameworkCore.Migrations;

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
                    Id = table.Column<int>(nullable: false, defaultValueSql: "NEXT VALUE FOR cms.ComponentStatesSQC"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsPublished = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentStates", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateTable(
                name: "ArticleStates",
                schema: "cms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Body = table.Column<string>(nullable: true),
                    Slug = table.Column<string>(maxLength: 1024, nullable: false, defaultValue: ""),
                    Title = table.Column<string>(maxLength: 1024, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleStates", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_ArticleStates_ComponentStates_Id",
                        column: x => x.Id,
                        principalSchema: "cms",
                        principalTable: "ComponentStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HtmlSnippetStates",
                schema: "cms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Code = table.Column<string>(unicode: false, maxLength: 1024, nullable: false, defaultValue: ""),
                    HtmlSnippet = table.Column<string>(nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HtmlSnippetStates", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_HtmlSnippetStates_ComponentStates_Id",
                        column: x => x.Id,
                        principalSchema: "cms",
                        principalTable: "ComponentStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TagStates",
                schema: "cms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Tag = table.Column<string>(maxLength: 1024, nullable: false, defaultValue: ""),
                    TagNormalized = table.Column<string>(maxLength: 1024, nullable: false, defaultValue: ""),
                    TagSlug = table.Column<string>(unicode: false, maxLength: 1024, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagStates", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_TagStates_ComponentStates_Id",
                        column: x => x.Id,
                        principalSchema: "cms",
                        principalTable: "ComponentStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaxonomyStates",
                schema: "cms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    ArticleId = table.Column<int>(nullable: false),
                    ParentId = table.Column<int>(nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxonomyStates", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_TaxonomyStates_ArticleStates_ArticleId",
                        column: x => x.ArticleId,
                        principalSchema: "cms",
                        principalTable: "ArticleStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaxonomyStates_ComponentStates_Id",
                        column: x => x.Id,
                        principalSchema: "cms",
                        principalTable: "ComponentStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction,
                        onUpdate: ReferentialAction.NoAction);
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
                        name: "FK_ArticleTagStates_ArticleStates_ArticleId",
                        column: x => x.ArticleId,
                        principalSchema: "cms",
                        principalTable: "ArticleStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArticleTagStates_TagStates_TagId",
                        column: x => x.TagId,
                        principalSchema: "cms",
                        principalTable: "TagStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Article_Title",
                schema: "cms",
                table: "ArticleStates",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleTagStates_TagId",
                schema: "cms",
                table: "ArticleTagStates",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_HtmlSnippet_Code",
                schema: "cms",
                table: "HtmlSnippetStates",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tag_Tag",
                schema: "cms",
                table: "TagStates",
                column: "Tag",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tag_TagNormalized",
                schema: "cms",
                table: "TagStates",
                column: "TagNormalized",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Taxonomy_ArticleId",
                schema: "cms",
                table: "TaxonomyStates",
                column: "ArticleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Taxonomy_ParentId",
                schema: "cms",
                table: "TaxonomyStates",
                column: "ParentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleTagStates",
                schema: "cms");

            migrationBuilder.DropTable(
                name: "HtmlSnippetStates",
                schema: "cms");

            migrationBuilder.DropTable(
                name: "TaxonomyStates",
                schema: "cms");

            migrationBuilder.DropTable(
                name: "TagStates",
                schema: "cms");

            migrationBuilder.DropTable(
                name: "ArticleStates",
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