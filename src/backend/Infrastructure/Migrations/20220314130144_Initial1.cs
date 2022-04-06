using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Initial1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Category");

            migrationBuilder.EnsureSchema(
                name: "Student");

            migrationBuilder.EnsureSchema(
                name: "Instructor");

            migrationBuilder.EnsureSchema(
                name: "Course");

            migrationBuilder.CreateTable(
                name: "Categories",
                schema: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameNormalize = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Instructors",
                schema: "Instructor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EmailNormalize = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ProfilePictureFilename = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    EmailConfirmedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateRegistered = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instructors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                schema: "Course",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NameNormalize = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                schema: "Student",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EmailNormalize = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ProfilePictureFilename = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    EmailConfirmedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateRegistered = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Topics",
                schema: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameNormalize = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Topics_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "Category",
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Communities",
                schema: "Instructor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InstructorId = table.Column<int>(type: "int", nullable: false),
                    Platform = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DestinationUri = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Communities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Communities_Instructors_InstructorId",
                        column: x => x.InstructorId,
                        principalSchema: "Instructor",
                        principalTable: "Instructors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Passwords",
                schema: "Instructor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InstructorId = table.Column<int>(type: "int", nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Digest = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    IsCurrent = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passwords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Passwords_Instructors_InstructorId",
                        column: x => x.InstructorId,
                        principalSchema: "Instructor",
                        principalTable: "Instructors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                schema: "Course",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InstructorId = table.Column<int>(type: "int", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ShortDescription = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    Level = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PriceInUSD = table.Column<double>(type: "float", nullable: false),
                    ThumbnailImageUri = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ListingStatus = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courses_Instructors_InstructorId",
                        column: x => x.InstructorId,
                        principalSchema: "Instructor",
                        principalTable: "Instructors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Courses_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalSchema: "Course",
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CategoryPreferences",
                schema: "Student",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryPreferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoryPreferences_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "Category",
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryPreferences_Students_StudentId",
                        column: x => x.StudentId,
                        principalSchema: "Student",
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Passwords",
                schema: "Student",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Digest = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    IsCurrent = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passwords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Passwords_Students_StudentId",
                        column: x => x.StudentId,
                        principalSchema: "Student",
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lessons",
                schema: "Course",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPreviewable = table.Column<bool>(type: "bit", nullable: false),
                    VideoPathUri = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ThetaVideoId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ThetaVideoPlayerUri = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    ThetaVideoPlaybackUri = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    ThetaVideoDuration = table.Column<long>(type: "bigint", nullable: false),
                    ChildLessonId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lessons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lessons_Courses_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "Course",
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                schema: "Course",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Courses_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "Course",
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Students_StudentId",
                        column: x => x.StudentId,
                        principalSchema: "Student",
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Topics",
                schema: "Course",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    TopicId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Topics_Courses_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "Course",
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Topics_Topics_TopicId",
                        column: x => x.TopicId,
                        principalSchema: "Category",
                        principalTable: "Topics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                schema: "Category",
                table: "Categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_NameNormalize",
                schema: "Category",
                table: "Categories",
                column: "NameNormalize",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CategoryPreferences_CategoryId",
                schema: "Student",
                table: "CategoryPreferences",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryPreferences_StudentId",
                schema: "Student",
                table: "CategoryPreferences",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Communities_InstructorId",
                schema: "Instructor",
                table: "Communities",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_InstructorId",
                schema: "Course",
                table: "Courses",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_LanguageId",
                schema: "Course",
                table: "Courses",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_Level",
                schema: "Course",
                table: "Courses",
                column: "Level");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_Name",
                schema: "Course",
                table: "Courses",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Instructors_Email",
                schema: "Instructor",
                table: "Instructors",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Instructors_EmailNormalize",
                schema: "Instructor",
                table: "Instructors",
                column: "EmailNormalize",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Languages_Name",
                schema: "Course",
                table: "Languages",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Languages_NameNormalize",
                schema: "Course",
                table: "Languages",
                column: "NameNormalize",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_CourseId",
                schema: "Course",
                table: "Lessons",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_Name",
                schema: "Course",
                table: "Lessons",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Passwords_InstructorId",
                schema: "Instructor",
                table: "Passwords",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_Passwords_StudentId",
                schema: "Student",
                table: "Passwords",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_Email",
                schema: "Student",
                table: "Students",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_EmailNormalize",
                schema: "Student",
                table: "Students",
                column: "EmailNormalize",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_CourseId",
                schema: "Course",
                table: "Subscriptions",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_StudentId",
                schema: "Course",
                table: "Subscriptions",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Topics_CategoryId",
                schema: "Category",
                table: "Topics",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Topics_Name",
                schema: "Category",
                table: "Topics",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Topics_NameNormalize",
                schema: "Category",
                table: "Topics",
                column: "NameNormalize",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Topics_CourseId",
                schema: "Course",
                table: "Topics",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Topics_TopicId",
                schema: "Course",
                table: "Topics",
                column: "TopicId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryPreferences",
                schema: "Student");

            migrationBuilder.DropTable(
                name: "Communities",
                schema: "Instructor");

            migrationBuilder.DropTable(
                name: "Lessons",
                schema: "Course");

            migrationBuilder.DropTable(
                name: "Passwords",
                schema: "Instructor");

            migrationBuilder.DropTable(
                name: "Passwords",
                schema: "Student");

            migrationBuilder.DropTable(
                name: "Subscriptions",
                schema: "Course");

            migrationBuilder.DropTable(
                name: "Topics",
                schema: "Course");

            migrationBuilder.DropTable(
                name: "Students",
                schema: "Student");

            migrationBuilder.DropTable(
                name: "Courses",
                schema: "Course");

            migrationBuilder.DropTable(
                name: "Topics",
                schema: "Category");

            migrationBuilder.DropTable(
                name: "Instructors",
                schema: "Instructor");

            migrationBuilder.DropTable(
                name: "Languages",
                schema: "Course");

            migrationBuilder.DropTable(
                name: "Categories",
                schema: "Category");
        }
    }
}
