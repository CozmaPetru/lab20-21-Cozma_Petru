using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibrarieLab.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Carti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Titlu = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    Autor = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    AnAparitie = table.Column<int>(type: "INTEGER", nullable: false),
                    Pret = table.Column<decimal>(type: "TEXT", nullable: false),
                    Descriere = table.Column<string>(type: "TEXT", nullable: true),
                    Coperta = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carti", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Carti");
        }
    }
}
