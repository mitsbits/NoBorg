using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Borg.Platform.EStore.Data.Migrations
{
    public partial class estore4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PriceListState",
                schema: "estore",
                table: "PriceListState");

            migrationBuilder.DropIndex(
                name: "IX_PriceListState_Id",
                schema: "estore",
                table: "PriceListState");

            migrationBuilder.DropColumn(
                name: "LanguageCode",
                schema: "estore",
                table: "PriceListState");

            migrationBuilder.EnsureSchema(
                name: "borg");

            migrationBuilder.RenameTable(
                name: "TaxonomyState",
                schema: "estore",
                newSchema: "borg");

            migrationBuilder.RenameTable(
                name: "ComponentState",
                schema: "estore",
                newSchema: "borg");

            migrationBuilder.AddColumn<string>(
                name: "FriendlyName",
                schema: "estore",
                table: "PriceListState",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PriceListState",
                schema: "estore",
                table: "PriceListState",
                column: "Id")
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateTable(
                name: "PriceState",
                schema: "estore",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CataloguePrice = table.Column<decimal>(nullable: false),
                    DiscountPrice = table.Column<decimal>(nullable: false),
                    FinalPrice = table.Column<decimal>(nullable: false),
                    PriceListId = table.Column<int>(nullable: false),
                    SKU = table.Column<string>(nullable: true),
                    StrikeOutPrice = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceState", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_PriceState_PriceListState_PriceListId",
                        column: x => x.PriceListId,
                        principalSchema: "estore",
                        principalTable: "PriceListState",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PriceState_PriceListId",
                schema: "estore",
                table: "PriceState",
                column: "PriceListId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PriceState",
                schema: "estore");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PriceListState",
                schema: "estore",
                table: "PriceListState");

            migrationBuilder.DropColumn(
                name: "FriendlyName",
                schema: "estore",
                table: "PriceListState");

            migrationBuilder.RenameTable(
                name: "TaxonomyState",
                schema: "borg",
                newSchema: "estore");

            migrationBuilder.RenameTable(
                name: "ComponentState",
                schema: "borg",
                newSchema: "estore");

            migrationBuilder.AddColumn<string>(
                name: "LanguageCode",
                schema: "estore",
                table: "PriceListState",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PriceListState",
                schema: "estore",
                table: "PriceListState",
                columns: new[] { "Id", "LanguageCode" })
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_PriceListState_Id",
                schema: "estore",
                table: "PriceListState",
                column: "Id",
                unique: true);
        }
    }
}
