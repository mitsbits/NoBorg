using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Borg.Cms.Basic.Lib.System.Data.Migrations
{
    public partial class addimg_pat_to_menu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ParentId",
                schema: "borg",
                table: "NavigationItemRecords",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "Group",
                schema: "borg",
                table: "NavigationItemRecords",
                maxLength: 3,
                nullable: false,
                defaultValue: "BSE",
                oldClrType: typeof(string),
                oldMaxLength: 3);

            migrationBuilder.AlterColumn<string>(
                name: "Display",
                schema: "borg",
                table: "NavigationItemRecords",
                maxLength: 512,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldMaxLength: 512);

            migrationBuilder.AddColumn<string>(
                name: "Path",
                schema: "borg",
                table: "NavigationItemRecords",
                maxLength: 512,
                nullable: true,
                defaultValue: "/");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Path",
                schema: "borg",
                table: "NavigationItemRecords");

            migrationBuilder.AlterColumn<int>(
                name: "ParentId",
                schema: "borg",
                table: "NavigationItemRecords",
                nullable: false,
                oldClrType: typeof(int),
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Group",
                schema: "borg",
                table: "NavigationItemRecords",
                maxLength: 3,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 3,
                oldDefaultValue: "BSE");

            migrationBuilder.AlterColumn<string>(
                name: "Display",
                schema: "borg",
                table: "NavigationItemRecords",
                maxLength: 512,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 512,
                oldDefaultValue: "");
        }
    }
}
