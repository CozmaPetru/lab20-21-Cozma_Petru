using LibrarieLab.Models;
using Microsoft.EntityFrameworkCore;

namespace LibrarieLab.Data
{
    // Legătura dintre aplicație și baza de date SQLite
    public class LibrarieContext : DbContext
    {
        public LibrarieContext(DbContextOptions<LibrarieContext> options)
            : base(options)
        {
        }

        // Tabelul „Carti”
        public DbSet<Carte> Carti { get; set; }
    }
}