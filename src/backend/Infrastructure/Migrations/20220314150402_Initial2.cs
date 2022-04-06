using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Initial2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DestinationUri",
                schema: "Instructor",
                table: "Communities",
                newName: "HandleName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HandleName",
                schema: "Instructor",
                table: "Communities",
                newName: "DestinationUri");
        }
    }
}
