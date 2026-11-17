using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpozitieLab.Migrations
{
    /// <inheritdoc />
    public partial class ImagineLucrare : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Imagine",
                table: "Lucrari",
                type: "TEXT",
                maxLength: 60,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Imagine",
                table: "Lucrari");
        }
    }
}
