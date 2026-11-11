using Microsoft.EntityFrameworkCore;
using CaminLab.Models;

namespace CaminLab.Data
{
    public class CaminDbContext : DbContext
    {
        public CaminDbContext(DbContextOptions<CaminDbContext> options) : base(options)
        {
        }

        public DbSet<Camera> Camere { get; set; }

        public DbSet<Cerere> Cereri { get; set; }
    }
}
