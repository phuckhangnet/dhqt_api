using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectBE.Migrations
{
    public partial class update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseDocuments");

            migrationBuilder.DropTable(
                name: "CourseFeedBacks");

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

            migrationBuilder.DropColumn(
                name: "IDENTIFY",
                table: "Staffs");

            migrationBuilder.DropColumn(
                name: "LEVEL",
                table: "Staffs");

            migrationBuilder.DropColumn(
                name: "SEX",
                table: "Staffs");

            migrationBuilder.DropColumn(
                name: "STARTWORKDATE",
                table: "Staffs");

            migrationBuilder.DropColumn(
                name: "STATUS",
                table: "Staffs");

            migrationBuilder.RenameColumn(
                name: "TITLE",
                table: "Staffs",
                newName: "PASSWORD");

            migrationBuilder.AddColumn<int>(
                name: "ROLE_ID",
                table: "Staffs",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TITLE = table.Column<string>(type: "TEXT", nullable: true),
                    SUMMARY = table.Column<string>(type: "TEXT", nullable: true),
                    ARTICLE_CONTENT = table.Column<string>(type: "TEXT", nullable: true),
                    HASTAG = table.Column<string>(type: "TEXT", nullable: true),
                    AVATAR = table.Column<string>(type: "TEXT", nullable: true),
                    CREATE_DATE = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LATEST_EDIT_DATE = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CREATE_USER = table.Column<int>(type: "INTEGER", nullable: false),
                    LATEST_EDIT_USER = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Hastag",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CODE = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hastag", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Menu",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DESCRIPTION = table.Column<string>(type: "TEXT", nullable: true),
                    HASTAG = table.Column<string>(type: "TEXT", nullable: true),
                    MENU_LEVEL = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menu", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CODE = table.Column<string>(type: "TEXT", nullable: true),
                    RULE_LIST = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Rule",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CODE = table.Column<string>(type: "TEXT", nullable: true),
                    DESCRIPTION = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rule", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "Hastag");

            migrationBuilder.DropTable(
                name: "Menu");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Rule");

            migrationBuilder.DropColumn(
                name: "ROLE_ID",
                table: "Staffs");

            migrationBuilder.RenameColumn(
                name: "PASSWORD",
                table: "Staffs",
                newName: "TITLE");

            migrationBuilder.AddColumn<string>(
                name: "IDENTIFY",
                table: "Staffs",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LEVEL",
                table: "Staffs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SEX",
                table: "Staffs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "STARTWORKDATE",
                table: "Staffs",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "STATUS",
                table: "Staffs",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CourseDocuments",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CODE = table.Column<string>(type: "TEXT", nullable: true),
                    CREATEDDATE = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CREATEDUSER = table.Column<int>(type: "INTEGER", nullable: true),
                    DESCRIPTION = table.Column<string>(type: "TEXT", nullable: true),
                    DOCUMENT = table.Column<string>(type: "TEXT", nullable: true),
                    FILETYPE = table.Column<string>(type: "TEXT", nullable: true),
                    IDCourse = table.Column<int>(type: "INTEGER", nullable: false),
                    NAME = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseDocuments", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CourseFeedBacks",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CREATEDDATE = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FEEDBACK = table.Column<string>(type: "TEXT", nullable: true),
                    IDCOURSE = table.Column<int>(type: "INTEGER", nullable: false),
                    IDUSER = table.Column<int>(type: "INTEGER", nullable: false),
                    Rating = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseFeedBacks", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CODE = table.Column<string>(type: "TEXT", nullable: true),
                    CREATEDDATE = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CREATEDUSER = table.Column<string>(type: "TEXT", nullable: true),
                    DESCRIPTION = table.Column<string>(type: "TEXT", nullable: true),
                    ENDDATE = table.Column<DateTime>(type: "TEXT", nullable: false),
                    NAME = table.Column<string>(type: "TEXT", nullable: true),
                    STARTDATE = table.Column<DateTime>(type: "TEXT", nullable: false),
                    STATUS = table.Column<string>(type: "TEXT", nullable: true),
                    TYPE = table.Column<string>(type: "TEXT", nullable: true)
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
                    CODE = table.Column<string>(type: "TEXT", nullable: true),
                    CREATEDDATE = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EMAIL = table.Column<string>(type: "TEXT", nullable: true),
                    IDENTIFY = table.Column<string>(type: "TEXT", nullable: true),
                    NAME = table.Column<string>(type: "TEXT", nullable: true),
                    PASSWORDHASH = table.Column<string>(type: "TEXT", nullable: true),
                    PASSWORDSALT = table.Column<string>(type: "TEXT", nullable: true),
                    PHONE = table.Column<string>(type: "TEXT", nullable: true),
                    SEX = table.Column<int>(type: "INTEGER", nullable: false),
                    STATUS = table.Column<string>(type: "TEXT", nullable: true),
                    USERNAME = table.Column<string>(type: "TEXT", nullable: true)
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
                    CREATEDDATE = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CREATEDUSER = table.Column<string>(type: "TEXT", nullable: true),
                    DESCRIPTION = table.Column<string>(type: "TEXT", nullable: true),
                    NAME = table.Column<string>(type: "TEXT", nullable: true),
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
                    IDGROUP = table.Column<int>(type: "INTEGER", nullable: false),
                    IDMEMBER = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JoinGroups", x => x.ID);
                });
        }
    }
}
