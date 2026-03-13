using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestCase.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFilePathFromGrade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "Grades");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "Grades",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
