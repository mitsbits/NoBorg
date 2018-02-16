using Microsoft.EntityFrameworkCore.Migrations;

namespace Borg.Platform.EF.CMS.Data.Migrations
{
    public partial class ten : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ComponentDeviceStates_ComponentId",
                schema: "cms",
                table: "ComponentDeviceStates");

            migrationBuilder.DropIndex(
                name: "IX_ComponentDeviceStates_DeviceId",
                schema: "cms",
                table: "ComponentDeviceStates");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentDeviceState_ComponentId",
                schema: "cms",
                table: "ComponentDeviceStates",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentDeviceState_DeviceId",
                schema: "cms",
                table: "ComponentDeviceStates",
                column: "DeviceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_PageMetadatas",
                schema: "cms",
                table: "PageMetadataStates",
                column: "Id",
                principalSchema: "cms",
                principalTable: "ArticleStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_PageMetadatas",
                schema: "cms",
                table: "PageMetadataStates");

            migrationBuilder.DropIndex(
                name: "IX_ComponentDeviceState_ComponentId",
                schema: "cms",
                table: "ComponentDeviceStates");

            migrationBuilder.DropIndex(
                name: "IX_ComponentDeviceState_DeviceId",
                schema: "cms",
                table: "ComponentDeviceStates");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentDeviceStates_ComponentId",
                schema: "cms",
                table: "ComponentDeviceStates",
                column: "ComponentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ComponentDeviceStates_DeviceId",
                schema: "cms",
                table: "ComponentDeviceStates",
                column: "DeviceId",
                unique: true);
        }
    }
}