using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DIPTERV.Migrations
{
    /// <inheritdoc />
    public partial class Ini2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TimeBlocks_Day_LessonNumber",
                table: "TimeBlocks",
                columns: new[] { "Day", "LessonNumber" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TimeBlocks_Day_LessonNumber",
                table: "TimeBlocks");
        }
    }
}
