using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Timesheets.Web.Domain.Migrations
{
    public partial class tagcolumnrename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn("IsEnables", "Tags", "IsEnabled");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn("IsEnabled", "Tags", "IsEnables");
        }
    }
}
