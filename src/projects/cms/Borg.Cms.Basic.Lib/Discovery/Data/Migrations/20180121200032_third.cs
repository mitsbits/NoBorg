using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Borg.Cms.Basic.Lib.Discovery.Data.Migrations
{
    public partial class third : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "discovery");

            migrationBuilder.RenameTable(
                name: "Blogpost",
                newSchema: "discovery");

            migrationBuilder.RenameTable(
                name: "BloggerBlog",
                newSchema: "discovery");

            migrationBuilder.RenameTable(
                name: "Blogger",
                newSchema: "discovery");

            migrationBuilder.RenameTable(
                name: "Blog",
                newSchema: "discovery");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Blogpost",
                schema: "discovery");

            migrationBuilder.RenameTable(
                name: "BloggerBlog",
                schema: "discovery");

            migrationBuilder.RenameTable(
                name: "Blogger",
                schema: "discovery");

            migrationBuilder.RenameTable(
                name: "Blog",
                schema: "discovery");
        }
    }
}
