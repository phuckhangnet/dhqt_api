using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectBE.Migrations
{
    public partial class updateStaffTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IDCOURSE",
                table: "JoinGroups");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Staffs",
                newName: "PASSWORDHASH");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Staffs",
                newName: "USERNAME");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Courses",
                newName: "TYPE");

            migrationBuilder.AddColumn<string>(
                name: "PASSWORDSALT",
                table: "Staffs",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IDMEMBER",
                table: "JoinGroups",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IDGROUP",
                table: "JoinGroups",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CourseFeedBacks",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IDUSER = table.Column<int>(type: "INTEGER", nullable: false),
                    IDCOURSE = table.Column<int>(type: "INTEGER", nullable: false),
                    FEEDBACK = table.Column<string>(type: "TEXT", nullable: true),
                    CREATEDDATE = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Rating = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseFeedBacks", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseFeedBacks");

            migrationBuilder.DropColumn(
                name: "PASSWORDSALT",
                table: "Staffs");

            migrationBuilder.DropColumn(
                name: "IDGROUP",
                table: "JoinGroups");

            migrationBuilder.RenameColumn(
                name: "PASSWORDHASH",
                table: "Staffs",
                newName: "PasswordHash");

            migrationBuilder.RenameColumn(
                name: "USERNAME",
                table: "Staffs",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "TYPE",
                table: "Courses",
                newName: "Type");

            migrationBuilder.AlterColumn<string>(
                name: "IDMEMBER",
                table: "JoinGroups",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IDCOURSE",
                table: "JoinGroups",
                type: "TEXT",
                nullable: true);
        }
    }
}
