using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Initial5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Rating",
                schema: "Course",
                table: "Subscriptions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<double>(
                name: "AverageRating",
                schema: "Course",
                table: "Courses",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "EnrolledCount",
                schema: "Course",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AverageRating",
                schema: "Course",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "EnrolledCount",
                schema: "Course",
                table: "Courses");

            migrationBuilder.AlterColumn<int>(
                name: "Rating",
                schema: "Course",
                table: "Subscriptions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
