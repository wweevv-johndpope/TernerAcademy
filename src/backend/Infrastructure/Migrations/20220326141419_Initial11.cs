using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Initial11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WalletAddress",
                schema: "Instructor",
                table: "Instructors",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WalletAddress",
                schema: "Instructor",
                table: "Instructors");
        }
    }
}
