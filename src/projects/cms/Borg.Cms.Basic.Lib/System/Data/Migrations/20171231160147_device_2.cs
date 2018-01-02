using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Borg.Cms.Basic.Lib.System.Data.Migrations
{
    public partial class device_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RenderScheme",
                schema: "borg",
                table: "DeviceRecords",
                maxLength: 512,
                nullable: false,
                defaultValue: "UnSet");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RenderScheme",
                schema: "borg",
                table: "DeviceRecords");
        }
    }
}
