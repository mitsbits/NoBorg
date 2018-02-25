using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Borg.Platform.EF.CMS.Data.Migrations
{
    public partial class cms1 : Migration
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
                name: "DeviceStates",
                schema: "cms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FriendlyName = table.Column<string>(maxLength: 512, nullable: false, defaultValue: ""),
                    Layout = table.Column<string>(maxLength: 512, nullable: false, defaultValue: ""),
                    RenderScheme = table.Column<string>(maxLength: 512, nullable: false, defaultValue: "UnSet"),
                    Theme = table.Column<string>(unicode: false, maxLength: 256, nullable: true, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceStates", x => x.Id)
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
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "CategoryGroupingStates",
                schema: "cms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    FriendlyName = table.Column<string>(maxLength: 512, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryGroupingStates", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_CategoryGroupingStates_ComponentStates_Id",
                        column: x => x.Id,
                        principalSchema: "cms",
                        principalTable: "ComponentStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ComponentDocumentAssociationStates",
                schema: "cms",
                columns: table => new
                {
                    ComponentId = table.Column<int>(nullable: false),
                    DocumentId = table.Column<int>(nullable: false),
                    FileId = table.Column<int>(nullable: false),
                    MimeType = table.Column<string>(maxLength: 256, nullable: false, defaultValue: "application/octet-stream"),
                    Uri = table.Column<string>(maxLength: 1024, nullable: false, defaultValue: ""),
                    Version = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentDocumentAssociationStates", x => new { x.ComponentId, x.DocumentId })
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_Components_Documents",
                        column: x => x.ComponentId,
                        principalSchema: "cms",
                        principalTable: "ComponentStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
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
                        onDelete: ReferentialAction.NoAction);
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
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ComponentDeviceStates",
                schema: "cms",
                columns: table => new
                {
                    ComponentId = table.Column<int>(nullable: false),
                    DeviceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentDeviceStates", x => new { x.ComponentId, x.DeviceId })
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_ComponentDeviceStates_ComponentStates_ComponentId",
                        column: x => x.ComponentId,
                        principalSchema: "cms",
                        principalTable: "ComponentStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ComponentDeviceStates_DeviceStates_DeviceId",
                        column: x => x.DeviceId,
                        principalSchema: "cms",
                        principalTable: "DeviceStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "SectionStates",
                schema: "cms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DeviceId = table.Column<int>(nullable: false),
                    FriendlyName = table.Column<string>(maxLength: 512, nullable: false, defaultValue: ""),
                    Identifier = table.Column<string>(maxLength: 512, nullable: false, defaultValue: ""),
                    RenderScheme = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SectionStates", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_Device_Section",
                        column: x => x.DeviceId,
                        principalSchema: "cms",
                        principalTable: "DeviceStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "PageMetadataStates",
                schema: "cms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    HtmlMetaJsonText = table.Column<string>(nullable: true, defaultValue: ""),
                    PrimaryImageDocumentId = table.Column<int>(nullable: true),
                    PrimaryImageFileId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageMetadataStates", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_Articles_PageMetadatas",
                        column: x => x.Id,
                        principalSchema: "cms",
                        principalTable: "ArticleStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Components_PageMetadatas",
                        column: x => x.Id,
                        principalSchema: "cms",
                        principalTable: "ComponentStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "CategoryStates",
                schema: "cms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    FriendlyName = table.Column<string>(nullable: true),
                    GroupingId = table.Column<int>(nullable: false),
                    Slug = table.Column<string>(unicode: false, maxLength: 1024, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryStates", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_CategoryGroupings_Categories",
                        column: x => x.GroupingId,
                        principalSchema: "cms",
                        principalTable: "CategoryGroupingStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_CategoryStates_ComponentStates_Id",
                        column: x => x.Id,
                        principalSchema: "cms",
                        principalTable: "ComponentStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
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
                        name: "FK_Articles_ArticleTags",
                        column: x => x.ArticleId,
                        principalSchema: "cms",
                        principalTable: "ArticleStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Tags_ArticleTags",
                        column: x => x.TagId,
                        principalSchema: "cms",
                        principalTable: "TagStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "SlotStates",
                schema: "cms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IsEnabled = table.Column<bool>(nullable: false),
                    ModuleDecriptorJson = table.Column<string>(nullable: false, defaultValue: ""),
                    ModuleGender = table.Column<string>(maxLength: 64, nullable: false, defaultValue: ""),
                    ModuleTypeName = table.Column<string>(maxLength: 1024, nullable: false, defaultValue: ""),
                    Ordinal = table.Column<int>(nullable: false),
                    SectionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SlotStates", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_Section_Slot",
                        column: x => x.SectionId,
                        principalSchema: "cms",
                        principalTable: "SectionStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "TaxonomyStates",
                schema: "cms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    ArticleId = table.Column<int>(nullable: false),
                    ParentId = table.Column<int>(nullable: false, defaultValue: 0),
                    Weight = table.Column<double>(nullable: false)
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
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_TaxonomyStates_CategoryStates_Id",
                        column: x => x.Id,
                        principalSchema: "cms",
                        principalTable: "CategoryStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_TaxonomyStates_ComponentStates_Id",
                        column: x => x.Id,
                        principalSchema: "cms",
                        principalTable: "ComponentStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "NavigationItemStates",
                schema: "cms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Display = table.Column<string>(maxLength: 1024, nullable: false, defaultValue: ""),
                    GroupCode = table.Column<string>(maxLength: 256, nullable: false, defaultValue: ""),
                    NavigationItemType = table.Column<int>(nullable: false),
                    Path = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NavigationItemStates", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_NavigationItemStates_TaxonomyStates_Id",
                        column: x => x.Id,
                        principalSchema: "cms",
                        principalTable: "TaxonomyStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
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
                name: "IX_CategoryStates_GroupingId",
                schema: "cms",
                table: "CategoryStates",
                column: "GroupingId");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentDeviceState_ComponentId",
                schema: "cms",
                table: "ComponentDeviceStates",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentDeviceState_DeviceId",
                schema: "cms",
                table: "ComponentDeviceStates",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentDocumentAssociationStates_FileId",
                schema: "cms",
                table: "ComponentDocumentAssociationStates",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentDocumentAssociationStates_Version",
                schema: "cms",
                table: "ComponentDocumentAssociationStates",
                column: "Version");

            migrationBuilder.CreateIndex(
                name: "IX_HtmlSnippet_Code",
                schema: "cms",
                table: "HtmlSnippetStates",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Navigation_GroupCode",
                schema: "cms",
                table: "NavigationItemStates",
                column: "GroupCode");

            migrationBuilder.CreateIndex(
                name: "IX_Section_DeviceId",
                schema: "cms",
                table: "SectionStates",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Section_Identifier",
                schema: "cms",
                table: "SectionStates",
                column: "Identifier");

            migrationBuilder.CreateIndex(
                name: "IX_Slot_ModuleGender",
                schema: "cms",
                table: "SlotStates",
                column: "ModuleGender");

            migrationBuilder.CreateIndex(
                name: "IX_Slot_ModuleTypeName",
                schema: "cms",
                table: "SlotStates",
                column: "ModuleTypeName");

            migrationBuilder.CreateIndex(
                name: "IX_Slot_Ordinal",
                schema: "cms",
                table: "SlotStates",
                column: "Ordinal");

            migrationBuilder.CreateIndex(
                name: "IX_Slot_SectionId",
                schema: "cms",
                table: "SlotStates",
                column: "SectionId");

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
                name: "ComponentDeviceStates",
                schema: "cms");

            migrationBuilder.DropTable(
                name: "ComponentDocumentAssociationStates",
                schema: "cms");

            migrationBuilder.DropTable(
                name: "HtmlSnippetStates",
                schema: "cms");

            migrationBuilder.DropTable(
                name: "NavigationItemStates",
                schema: "cms");

            migrationBuilder.DropTable(
                name: "PageMetadataStates",
                schema: "cms");

            migrationBuilder.DropTable(
                name: "SlotStates",
                schema: "cms");

            migrationBuilder.DropTable(
                name: "TagStates",
                schema: "cms");

            migrationBuilder.DropTable(
                name: "TaxonomyStates",
                schema: "cms");

            migrationBuilder.DropTable(
                name: "SectionStates",
                schema: "cms");

            migrationBuilder.DropTable(
                name: "ArticleStates",
                schema: "cms");

            migrationBuilder.DropTable(
                name: "CategoryStates",
                schema: "cms");

            migrationBuilder.DropTable(
                name: "DeviceStates",
                schema: "cms");

            migrationBuilder.DropTable(
                name: "CategoryGroupingStates",
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
