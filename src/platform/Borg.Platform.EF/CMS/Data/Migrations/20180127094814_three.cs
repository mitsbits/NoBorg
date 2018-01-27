using Microsoft.EntityFrameworkCore.Migrations;

namespace Borg.Platform.EF.CMS.Data.Migrations
{
    public partial class three : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Path",
                schema: "cms",
                table: "NavigationItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Path",
                schema: "cms",
                table: "NavigationItems");
        }
    }
}