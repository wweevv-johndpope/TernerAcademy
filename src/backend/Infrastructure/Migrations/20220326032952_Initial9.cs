using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Initial9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BuyTx",
                schema: "Course",
                table: "Subscriptions",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuyTx",
                schema: "Course",
                table: "Subscriptions");
        }
    }
}
