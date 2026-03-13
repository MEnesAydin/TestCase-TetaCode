using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestCase.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserGradeRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Grades",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Grades_UserId",
                table: "Grades",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_Users_UserId",
                table: "Grades",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grades_Users_UserId",
                table: "Grades");

            migrationBuilder.DropIndex(
                name: "IX_Grades_UserId",
                table: "Grades");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Grades");
        }
    }
}
