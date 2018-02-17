using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Borg.Platform.EF.CMS.Data.Migrations
{
    public partial class eleven : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Display",
                schema: "cms",
                table: "NavigationItemStates",
                maxLength: 1024,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Display",
                schema: "cms",
                table: "NavigationItemStates");
        }
    }
}
