using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Borg.Platform.EF.Assets.Data.Migrations
{
    public partial class assets2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentState",
                schema: "assets",
                table: "AssetRecords");

            migrationBuilder.AddColumn<int>(
                name: "DocumentBehaviourState",
                schema: "assets",
                table: "AssetRecords",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentBehaviourState",
                schema: "assets",
                table: "AssetRecords");

            migrationBuilder.AddColumn<int>(
                name: "DocumentState",
                schema: "assets",
                table: "AssetRecords",
                nullable: false,
                defaultValue: 0);
        }
    }
}
