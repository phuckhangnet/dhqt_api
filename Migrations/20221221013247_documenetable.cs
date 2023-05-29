using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectBE.Migrations
{
    public partial class documenetable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CourseDocuments",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IDCourse = table.Column<int>(type: "INTEGER", nullable: false),
                    NAME = table.Column<string>(type: "TEXT", nullable: true),
                    DESCRIPTION = table.Column<string>(type: "TEXT", nullable: true),
                    CREATEDDATE = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CREATEDUSER = table.Column<int>(type: "INTEGER", nullable: true),
                    DOCUMENT = table.Column<string>(type: "TEXT", nullable: true),
                    FILETYPE = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseDocuments", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseDocuments");
        }
    }
}
