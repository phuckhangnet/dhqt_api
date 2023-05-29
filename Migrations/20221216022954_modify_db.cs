using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectBE.Migrations
{
    public partial class modify_db : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Customers",
                newName: "PASSWORDHASH");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Customers",
                newName: "PASSWORDSALT");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PASSWORDHASH",
                table: "Customers",
                newName: "PasswordHash");

            migrationBuilder.RenameColumn(
                name: "PASSWORDSALT",
                table: "Customers",
                newName: "Password");
        }
    }
}
