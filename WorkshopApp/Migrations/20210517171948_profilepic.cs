using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkshopApp.Migrations
{
    public partial class profilepic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "profilePicture",
                table: "Student",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Year",
                table: "Enrollment",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "profilePicture",
                table: "Student");

            migrationBuilder.AlterColumn<int>(
                name: "Year",
                table: "Enrollment",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
