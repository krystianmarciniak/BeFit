using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeFit.Migrations
{
    /// <inheritdoc />
    public partial class Add_Duration_Notes_to_TrainingSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Weight",
                table: "PerformedExercises",
                newName: "WeightKg");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartTime",
                table: "TrainingSessions",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AddColumn<int>(
                name: "DurationMinutes",
                table: "TrainingSessions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "TrainingSessions",
                type: "TEXT",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SessionDate",
                table: "TrainingSessions",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DurationMinutes",
                table: "TrainingSessions");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "TrainingSessions");

            migrationBuilder.DropColumn(
                name: "SessionDate",
                table: "TrainingSessions");

            migrationBuilder.RenameColumn(
                name: "WeightKg",
                table: "PerformedExercises",
                newName: "Weight");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartTime",
                table: "TrainingSessions",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);
        }
    }
}
