using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Initial6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PriceInUSD",
                schema: "Course",
                table: "Courses",
                newName: "PriceInTFuel");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PriceInTFuel",
                schema: "Course",
                table: "Courses",
                newName: "PriceInUSD");
        }
    }
}
