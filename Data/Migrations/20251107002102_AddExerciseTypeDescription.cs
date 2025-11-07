using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeFit.Migrations
{
    /// <inheritdoc />
    public partial class AddExerciseTypeDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ExerciseTypes",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ExerciseTypes",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseTypes_UserId",
                table: "ExerciseTypes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseTypes_AspNetUsers_UserId",
                table: "ExerciseTypes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseTypes_AspNetUsers_UserId",
                table: "ExerciseTypes");

            migrationBuilder.DropIndex(
                name: "IX_ExerciseTypes_UserId",
                table: "ExerciseTypes");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "ExerciseTypes");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ExerciseTypes");
        }
    }
}
