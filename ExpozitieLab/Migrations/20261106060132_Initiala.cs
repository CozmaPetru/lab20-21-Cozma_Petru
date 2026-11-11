using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpozitieLab.Migrations
{
    /// <inheritdoc />
    public partial class Initiala : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lucrari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Titlu = table.Column<string>(type: "TEXT", maxLength: 60, nullable: false),
                    Autor = table.Column<string>(type: "TEXT", maxLength: 60, nullable: false),
                    Grupa = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    An = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lucrari", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Lucrari");
        }
    }
}
