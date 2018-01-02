using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Borg.Cms.Basic.Lib.System.Data.Migrations
{
    public partial class device_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeviceRecords",
                schema: "borg",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FriendlyName = table.Column<string>(maxLength: 512, nullable: false, defaultValue: ""),
                    Layout = table.Column<string>(maxLength: 512, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceRecords", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateTable(
                name: "SectionRecords",
                schema: "borg",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DeviceId = table.Column<int>(nullable: false),
                    FriendlyName = table.Column<string>(maxLength: 512, nullable: false, defaultValue: ""),
                    Identifier = table.Column<string>(maxLength: 512, nullable: false, defaultValue: ""),
                    RenderScheme = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SectionRecords", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_Device_Section",
                        column: x => x.DeviceId,
                        principalSchema: "borg",
                        principalTable: "DeviceRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SlotRecord",
                schema: "borg",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IsEnabled = table.Column<bool>(nullable: false),
                    ModuleDecriptorJson = table.Column<string>(nullable: false, defaultValue: ""),
                    Ordinal = table.Column<int>(nullable: false),
                    SectionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SlotRecord", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_Section_Slot",
                        column: x => x.SectionId,
                        principalSchema: "borg",
                        principalTable: "SectionRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Section_DeviceId",
                schema: "borg",
                table: "SectionRecords",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Section_Identifier",
                schema: "borg",
                table: "SectionRecords",
                column: "Identifier");

            migrationBuilder.CreateIndex(
                name: "IX_Slot_Ordinal",
                schema: "borg",
                table: "SlotRecord",
                column: "Ordinal");

            migrationBuilder.CreateIndex(
                name: "IX_Slot_SectionId",
                schema: "borg",
                table: "SlotRecord",
                column: "SectionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SlotRecord",
                schema: "borg");

            migrationBuilder.DropTable(
                name: "SectionRecords",
                schema: "borg");

            migrationBuilder.DropTable(
                name: "DeviceRecords",
                schema: "borg");
        }
    }
}