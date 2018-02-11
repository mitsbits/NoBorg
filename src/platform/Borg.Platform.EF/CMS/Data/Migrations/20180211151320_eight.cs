using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Borg.Platform.EF.CMS.Data.Migrations
{
    public partial class eight : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleTagStates_ArticleStates_ArticleId",
                schema: "cms",
                table: "ArticleTagStates");

            migrationBuilder.DropForeignKey(
                name: "FK_ArticleTagStates_TagStates_TagId",
                schema: "cms",
                table: "ArticleTagStates");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_ArticleTags",
                schema: "cms",
                table: "ArticleTagStates",
                column: "ArticleId",
                principalSchema: "cms",
                principalTable: "ArticleStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_ArticleTags",
                schema: "cms",
                table: "ArticleTagStates",
                column: "TagId",
                principalSchema: "cms",
                principalTable: "TagStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_ArticleTags",
                schema: "cms",
                table: "ArticleTagStates");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_ArticleTags",
                schema: "cms",
                table: "ArticleTagStates");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleTagStates_ArticleStates_ArticleId",
                schema: "cms",
                table: "ArticleTagStates",
                column: "ArticleId",
                principalSchema: "cms",
                principalTable: "ArticleStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleTagStates_TagStates_TagId",
                schema: "cms",
                table: "ArticleTagStates",
                column: "TagId",
                principalSchema: "cms",
                principalTable: "TagStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
