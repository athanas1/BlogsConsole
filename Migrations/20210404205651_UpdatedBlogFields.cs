using Microsoft.EntityFrameworkCore.Migrations;

namespace BlogsConsole.Migrations
{
    public partial class UpdatedBlogFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Blogs");

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "Blogs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Blogs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "Blogs");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Blogs",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
