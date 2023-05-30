using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DIPTERV.Migrations
{
    /// <inheritdoc />
    public partial class initnew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Rooms_RoomId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Courses_SubjectDivisions_SubjectDivisinId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Courses_TimeBlocks_TimeBlockId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_RoomId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_SubjectDivisinId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_TimeBlockId",
                table: "Courses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Courses_RoomId",
                table: "Courses",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_SubjectDivisinId",
                table: "Courses",
                column: "SubjectDivisinId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_TimeBlockId",
                table: "Courses",
                column: "TimeBlockId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Rooms_RoomId",
                table: "Courses",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_SubjectDivisions_SubjectDivisinId",
                table: "Courses",
                column: "SubjectDivisinId",
                principalTable: "SubjectDivisions",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_TimeBlocks_TimeBlockId",
                table: "Courses",
                column: "TimeBlockId",
                principalTable: "TimeBlocks",
                principalColumn: "ID");
        }
    }
}
