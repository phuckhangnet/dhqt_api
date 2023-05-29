using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectBE.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CODE = table.Column<string>(type: "TEXT", nullable: true),
                    NAME = table.Column<string>(type: "TEXT", nullable: true),
                    DESCRIPTION = table.Column<string>(type: "TEXT", nullable: true),
                    CREATEDDATE = table.Column<DateTime>(type: "TEXT", nullable: false),
                    STARTDATE = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ENDDATE = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CREATEDUSER = table.Column<string>(type: "TEXT", nullable: true),
                    STATUS = table.Column<string>(type: "TEXT", nullable: true),
                    Type = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    USERNAME = table.Column<string>(type: "TEXT", nullable: true),
                    CODE = table.Column<string>(type: "TEXT", nullable: true),
                    NAME = table.Column<string>(type: "TEXT", nullable: true),
                    SEX = table.Column<int>(type: "INTEGER", nullable: false),
                    IDENTIFY = table.Column<string>(type: "TEXT", nullable: true),
                    EMAIL = table.Column<string>(type: "TEXT", nullable: true),
                    PHONE = table.Column<string>(type: "TEXT", nullable: true),
                    CREATEDDATE = table.Column<DateTime>(type: "TEXT", nullable: false),
                    STATUS = table.Column<string>(type: "TEXT", nullable: true),
                    Password = table.Column<string>(type: "TEXT", nullable: true),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CODE = table.Column<string>(type: "TEXT", nullable: true),
                    NAME = table.Column<string>(type: "TEXT", nullable: true),
                    DESCRIPTION = table.Column<string>(type: "TEXT", nullable: true),
                    CREATEDDATE = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CREATEDUSER = table.Column<string>(type: "TEXT", nullable: true),
                    STATUS = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "JoinCourses",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IDCOURSE = table.Column<string>(type: "TEXT", nullable: true),
                    IDMEMBER = table.Column<string>(type: "TEXT", nullable: true),
                    TITLE = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JoinCourses", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "JoinGroups",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IDCOURSE = table.Column<string>(type: "TEXT", nullable: true),
                    IDMEMBER = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JoinGroups", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Staffs",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CODE = table.Column<string>(type: "TEXT", nullable: true),
                    NAME = table.Column<string>(type: "TEXT", nullable: true),
                    SEX = table.Column<int>(type: "INTEGER", nullable: false),
                    IDENTIFY = table.Column<string>(type: "TEXT", nullable: true),
                    EMAIL = table.Column<string>(type: "TEXT", nullable: true),
                    PHONE = table.Column<string>(type: "TEXT", nullable: true),
                    TITLE = table.Column<string>(type: "TEXT", nullable: true),
                    STARTWORKDATE = table.Column<DateTime>(type: "TEXT", nullable: false),
                    STATUS = table.Column<string>(type: "TEXT", nullable: true),
                    LEVEL = table.Column<int>(type: "INTEGER", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: true),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staffs", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "JoinCourses");

            migrationBuilder.DropTable(
                name: "JoinGroups");

            migrationBuilder.DropTable(
                name: "Staffs");
        }
    }
}
