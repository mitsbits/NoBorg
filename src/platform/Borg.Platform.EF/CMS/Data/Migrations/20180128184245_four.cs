using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Borg.Platform.EF.CMS.Data.Migrations
{
    public partial class four : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NavigationItems_TaxonomyStates_Id",
                schema: "cms",
                table: "NavigationItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NavigationItems",
                schema: "cms",
                table: "NavigationItems");

            migrationBuilder.RenameTable(
                name: "NavigationItems",
                schema: "cms",
                newName: "NavigationItemStates");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NavigationItemStates",
                schema: "cms",
                table: "NavigationItemStates",
                column: "Id")
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateTable(
                name: "DeviceStates",
                schema: "cms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FriendlyName = table.Column<string>(maxLength: 512, nullable: false, defaultValue: ""),
                    Layout = table.Column<string>(maxLength: 512, nullable: false, defaultValue: ""),
                    RenderScheme = table.Column<string>(maxLength: 512, nullable: false, defaultValue: "UnSet")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceStates", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
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
                        onDelete: ReferentialAction.Cascade);
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
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.AddForeignKey(
                name: "FK_NavigationItemStates_TaxonomyStates_Id",
                schema: "cms",
                table: "NavigationItemStates",
                column: "Id",
                principalSchema: "cms",
                principalTable: "TaxonomyStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NavigationItemStates_TaxonomyStates_Id",
                schema: "cms",
                table: "NavigationItemStates");

            migrationBuilder.DropTable(
                name: "SlotStates",
                schema: "cms");

            migrationBuilder.DropTable(
                name: "SectionStates",
                schema: "cms");

            migrationBuilder.DropTable(
                name: "DeviceStates",
                schema: "cms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NavigationItemStates",
                schema: "cms",
                table: "NavigationItemStates");

            migrationBuilder.RenameTable(
                name: "NavigationItemStates",
                schema: "cms",
                newName: "NavigationItems");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NavigationItems",
                schema: "cms",
                table: "NavigationItems",
                column: "Id")
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.AddForeignKey(
                name: "FK_NavigationItems_TaxonomyStates_Id",
                schema: "cms",
                table: "NavigationItems",
                column: "Id",
                principalSchema: "cms",
                principalTable: "TaxonomyStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
