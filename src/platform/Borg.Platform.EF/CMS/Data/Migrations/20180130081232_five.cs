using Microsoft.EntityFrameworkCore.Migrations;

namespace Borg.Platform.EF.CMS.Data.Migrations
{
    public partial class five : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ComponentDeviceState",
                schema: "cms",
                columns: table => new
                {
                    ComponentId = table.Column<int>(nullable: false),
                    DeviceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentDeviceState", x => new { x.ComponentId, x.DeviceId })
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_ComponentDeviceState_ComponentStates_ComponentId",
                        column: x => x.ComponentId,
                        principalSchema: "cms",
                        principalTable: "ComponentStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComponentDeviceState_DeviceStates_DeviceId",
                        column: x => x.DeviceId,
                        principalSchema: "cms",
                        principalTable: "DeviceStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComponentDeviceState_ComponentId",
                schema: "cms",
                table: "ComponentDeviceState",
                column: "ComponentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ComponentDeviceState_DeviceId",
                schema: "cms",
                table: "ComponentDeviceState",
                column: "DeviceId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComponentDeviceState",
                schema: "cms");
        }
    }
}