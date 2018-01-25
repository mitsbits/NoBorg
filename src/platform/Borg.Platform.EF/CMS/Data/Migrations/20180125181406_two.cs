using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Borg.Platform.EF.CMS.Data.Migrations
{
    public partial class two : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ArticleId",
                schema: "cms",
                table: "ComponentStates",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                schema: "cms",
                table: "ComponentStates",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Taxonomy_ArticleId",
                schema: "cms",
                table: "ComponentStates",
                column: "ArticleId",
                unique: true,
                filter: "[ArticleId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Taxonomy_ParentId",
                schema: "cms",
                table: "ComponentStates",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ComponentStates_ComponentStates_ArticleId",
                schema: "cms",
                table: "ComponentStates",
                column: "ArticleId",
                principalSchema: "cms",
                principalTable: "ComponentStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction,
                onUpdate:ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComponentStates_ComponentStates_ArticleId",
                schema: "cms",
                table: "ComponentStates");

            migrationBuilder.DropIndex(
                name: "IX_Taxonomy_ArticleId",
                schema: "cms",
                table: "ComponentStates");

            migrationBuilder.DropIndex(
                name: "IX_Taxonomy_ParentId",
                schema: "cms",
                table: "ComponentStates");

            migrationBuilder.DropColumn(
                name: "ArticleId",
                schema: "cms",
                table: "ComponentStates");

            migrationBuilder.DropColumn(
                name: "ParentId",
                schema: "cms",
                table: "ComponentStates");
        }
    }
}
