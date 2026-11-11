using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CaminLab.Migrations
{
    /// <inheritdoc />
    public partial class Initiala : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Camere",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Numar = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    Etaj = table.Column<int>(type: "INTEGER", nullable: false),
                    Locuri = table.Column<int>(type: "INTEGER", nullable: false),
                    Tarif = table.Column<decimal>(type: "TEXT", nullable: false),
                    Libera = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Camere", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cereri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CameraId = table.Column<int>(type: "INTEGER", nullable: false),
                    NumeStudent = table.Column<string>(type: "TEXT", maxLength: 60, nullable: true),
                    TelefonStudent = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    Oras = table.Column<string>(type: "TEXT", maxLength: 40, nullable: true),
                    Strada = table.Column<string>(type: "TEXT", maxLength: 60, nullable: true),
                    NumeTutore = table.Column<string>(type: "TEXT", maxLength: 60, nullable: true),
                    TelefonTutore = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cereri", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Camere");

            migrationBuilder.DropTable(
                name: "Cereri");
        }
    }
}
