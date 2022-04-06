using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Initial10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AmountCashout",
                schema: "Course",
                table: "Subscriptions",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CashoutPaymentTx",
                schema: "Course",
                table: "Subscriptions",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PriceBurn",
                schema: "Course",
                table: "Subscriptions",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PriceDev",
                schema: "Course",
                table: "Subscriptions",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SendToBurnTx",
                schema: "Course",
                table: "Subscriptions",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SendToDevTx",
                schema: "Course",
                table: "Subscriptions",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountCashout",
                schema: "Course",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "CashoutPaymentTx",
                schema: "Course",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "PriceBurn",
                schema: "Course",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "PriceDev",
                schema: "Course",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "SendToBurnTx",
                schema: "Course",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "SendToDevTx",
                schema: "Course",
                table: "Subscriptions");
        }
    }
}
