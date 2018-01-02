using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Borg.Cms.Basic.Lib.System.Data.Migrations
{
    public partial class NavigationItemRecord : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "borg");

            migrationBuilder.CreateTable(
                name: "NavigationItemRecords",
                schema: "borg",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Display = table.Column<string>(maxLength: 512, nullable: false),
                    Group = table.Column<string>(maxLength: 3, nullable: false),
                    IsPublished = table.Column<bool>(nullable: false),
                    ItemType = table.Column<int>(nullable: false),
                    ParentId = table.Column<int>(nullable: false),
                    Weight = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NavigationItemRecords", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NavigationItemRecords",
                schema: "borg");
        }
    }
}
