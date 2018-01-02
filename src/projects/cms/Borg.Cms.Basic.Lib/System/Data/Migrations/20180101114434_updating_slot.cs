using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Borg.Cms.Basic.Lib.System.Data.Migrations
{
    public partial class updating_slot : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ModuleGender",
                schema: "borg",
                table: "SlotRecord",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ModuleTypeName",
                schema: "borg",
                table: "SlotRecord",
                maxLength: 1024,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Slot_ModuleGender",
                schema: "borg",
                table: "SlotRecord",
                column: "ModuleGender");

            migrationBuilder.CreateIndex(
                name: "IX_Slot_ModuleTypeName",
                schema: "borg",
                table: "SlotRecord",
                column: "ModuleTypeName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Slot_ModuleGender",
                schema: "borg",
                table: "SlotRecord");

            migrationBuilder.DropIndex(
                name: "IX_Slot_ModuleTypeName",
                schema: "borg",
                table: "SlotRecord");

            migrationBuilder.DropColumn(
                name: "ModuleGender",
                schema: "borg",
                table: "SlotRecord");

            migrationBuilder.DropColumn(
                name: "ModuleTypeName",
                schema: "borg",
                table: "SlotRecord");
        }
    }
}
