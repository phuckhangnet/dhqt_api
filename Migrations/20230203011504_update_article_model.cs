using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectBE.Migrations
{
    public partial class update_article_model : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LANGUAGE",
                table: "Articles",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PRIORITY_LEVEL",
                table: "Articles",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LANGUAGE",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "PRIORITY_LEVEL",
                table: "Articles");
        }
    }
}
