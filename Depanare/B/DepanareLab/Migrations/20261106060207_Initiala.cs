using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DepanareLab.Migrations
{
    /// <inheritdoc />
    public partial class Initiala : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Echipamente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Denumire = table.Column<string>(type: "TEXT", maxLength: 60, nullable: false),
                    Serie = table.Column<string>(type: "TEXT", maxLength: 30, nullable: true),
                    Pret = table.Column<decimal>(type: "TEXT", nullable: false),
                    An = table.Column<int>(type: "INTEGER", nullable: false),
                    Imagine = table.Column<string>(type: "TEXT", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Echipamente", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Echipamente");
        }
    }
}
