using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Initial12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LessonOrders",
                schema: "Course",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LessonId = table.Column<int>(type: "int", nullable: false),
                    ChildLessonId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LessonOrders_Lessons_LessonId",
                        column: x => x.LessonId,
                        principalSchema: "Course",
                        principalTable: "Lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LessonOrders_LessonId",
                schema: "Course",
                table: "LessonOrders",
                column: "LessonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LessonOrders",
                schema: "Course");
        }
    }
}
