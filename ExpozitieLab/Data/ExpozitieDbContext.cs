using Microsoft.EntityFrameworkCore;
using ExpozitieLab.Models;

namespace ExpozitieLab.Data
{
    public class ExpozitieDbContext : DbContext
    {
        public ExpozitieDbContext(DbContextOptions<ExpozitieDbContext> options) : base(options)
        {
        }

        public DbSet<Lucrare> Lucrari { get; set; }
    }
}
