using Microsoft.EntityFrameworkCore.Migrations;

namespace Borg.Platform.EF.CMS.Data.Migrations
{
    public partial class two : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Weight",
                schema: "cms",
                table: "TaxonomyStates",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "NavigationItems",
                schema: "cms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    GroupCode = table.Column<string>(maxLength: 256, nullable: false, defaultValue: ""),
                    NavigationItemType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NavigationItems", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_NavigationItems_TaxonomyStates_Id",
                        column: x => x.Id,
                        principalSchema: "cms",
                        principalTable: "TaxonomyStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Navigation_GroupCode",
                schema: "cms",
                table: "NavigationItems",
                column: "GroupCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NavigationItems",
                schema: "cms");

            migrationBuilder.DropColumn(
                name: "Weight",
                schema: "cms",
                table: "TaxonomyStates");
        }
    }
}