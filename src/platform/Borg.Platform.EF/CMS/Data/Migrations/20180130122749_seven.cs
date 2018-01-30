using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Borg.Platform.EF.CMS.Data.Migrations
{
    public partial class seven : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComponentDeviceState_ComponentStates_ComponentId",
                schema: "cms",
                table: "ComponentDeviceState");

            migrationBuilder.DropForeignKey(
                name: "FK_ComponentDeviceState_DeviceStates_DeviceId",
                schema: "cms",
                table: "ComponentDeviceState");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ComponentDeviceState",
                schema: "cms",
                table: "ComponentDeviceState");

            migrationBuilder.RenameTable(
                name: "ComponentDeviceState",
                schema: "cms",
                newName: "ComponentDeviceStates");

            migrationBuilder.RenameIndex(
                name: "IX_ComponentDeviceState_DeviceId",
                schema: "cms",
                table: "ComponentDeviceStates",
                newName: "IX_ComponentDeviceStates_DeviceId");

            migrationBuilder.RenameIndex(
                name: "IX_ComponentDeviceState_ComponentId",
                schema: "cms",
                table: "ComponentDeviceStates",
                newName: "IX_ComponentDeviceStates_ComponentId");

            migrationBuilder.AlterColumn<string>(
                name: "Theme",
                schema: "cms",
                table: "DeviceStates",
                unicode: false,
                maxLength: 256,
                nullable: true,
                defaultValue: "",
                oldClrType: typeof(string),
                oldMaxLength: 512,
                oldNullable: true,
                oldDefaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ComponentDeviceStates",
                schema: "cms",
                table: "ComponentDeviceStates",
                columns: new[] { "ComponentId", "DeviceId" })
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.AddForeignKey(
                name: "FK_ComponentDeviceStates_ComponentStates_ComponentId",
                schema: "cms",
                table: "ComponentDeviceStates",
                column: "ComponentId",
                principalSchema: "cms",
                principalTable: "ComponentStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ComponentDeviceStates_DeviceStates_DeviceId",
                schema: "cms",
                table: "ComponentDeviceStates",
                column: "DeviceId",
                principalSchema: "cms",
                principalTable: "DeviceStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComponentDeviceStates_ComponentStates_ComponentId",
                schema: "cms",
                table: "ComponentDeviceStates");

            migrationBuilder.DropForeignKey(
                name: "FK_ComponentDeviceStates_DeviceStates_DeviceId",
                schema: "cms",
                table: "ComponentDeviceStates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ComponentDeviceStates",
                schema: "cms",
                table: "ComponentDeviceStates");

            migrationBuilder.RenameTable(
                name: "ComponentDeviceStates",
                schema: "cms",
                newName: "ComponentDeviceState");

            migrationBuilder.RenameIndex(
                name: "IX_ComponentDeviceStates_DeviceId",
                schema: "cms",
                table: "ComponentDeviceState",
                newName: "IX_ComponentDeviceState_DeviceId");

            migrationBuilder.RenameIndex(
                name: "IX_ComponentDeviceStates_ComponentId",
                schema: "cms",
                table: "ComponentDeviceState",
                newName: "IX_ComponentDeviceState_ComponentId");

            migrationBuilder.AlterColumn<string>(
                name: "Theme",
                schema: "cms",
                table: "DeviceStates",
                maxLength: 512,
                nullable: true,
                defaultValue: "",
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 256,
                oldNullable: true,
                oldDefaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ComponentDeviceState",
                schema: "cms",
                table: "ComponentDeviceState",
                columns: new[] { "ComponentId", "DeviceId" })
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.AddForeignKey(
                name: "FK_ComponentDeviceState_ComponentStates_ComponentId",
                schema: "cms",
                table: "ComponentDeviceState",
                column: "ComponentId",
                principalSchema: "cms",
                principalTable: "ComponentStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ComponentDeviceState_DeviceStates_DeviceId",
                schema: "cms",
                table: "ComponentDeviceState",
                column: "DeviceId",
                principalSchema: "cms",
                principalTable: "DeviceStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
