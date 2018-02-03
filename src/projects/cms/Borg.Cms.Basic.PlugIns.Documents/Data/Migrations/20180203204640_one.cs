using Microsoft.EntityFrameworkCore.Migrations;

namespace Borg.Cms.Basic.PlugIns.Documents.Data.Migrations
{
    public partial class one : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "documents");

            migrationBuilder.CreateTable(
                name: "DocumentOwnerStates",
                schema: "documents",
                columns: table => new
                {
                    DoecumentId = table.Column<int>(nullable: false),
                    Owner = table.Column<string>(unicode: false, maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentOwnerStates", x => new { x.DoecumentId, x.Owner })
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateTable(
                name: "DocumentStates",
                schema: "documents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentStates", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentOwnerStates",
                schema: "documents");

            migrationBuilder.DropTable(
                name: "DocumentStates",
                schema: "documents");
        }
    }
}