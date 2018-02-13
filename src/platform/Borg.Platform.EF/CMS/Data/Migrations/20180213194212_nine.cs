using Microsoft.EntityFrameworkCore.Migrations;

namespace Borg.Platform.EF.CMS.Data.Migrations
{
    public partial class nine : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PageMetadataStates",
                schema: "cms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    HtmlMetaJsonText = table.Column<string>(nullable: true, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageMetadataStates", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_Components_PageMetadatas",
                        column: x => x.Id,
                        principalSchema: "cms",
                        principalTable: "ComponentStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PageMetadataStates",
                schema: "cms");
        }
    }
}