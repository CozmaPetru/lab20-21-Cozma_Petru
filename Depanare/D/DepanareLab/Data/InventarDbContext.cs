using DepanareLab.Models;

namespace DepanareLab.Data
{
    public class InventarDbContext : DbContext
    {
        public InventarDbContext(DbContextOptions<InventarDbContext> options) : base(options)
        {
        }

        public DbSet<Echipament> Echipamente { get; set; }
    }
}
