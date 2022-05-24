using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkshopApp.Migrations
{
    public partial class TeacherPicture : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "profilePicture",
                table: "Teacher",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "profilePicture",
                table: "Teacher");
        }
    }
}
