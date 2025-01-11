using Microsoft.EntityFrameworkCore;

namespace NZWalksAPI.Data
{
    public class NZWalksDbContext : DbContext
    {

        public NZWalksDbContext(DbContextOptions<NZWalksDbContext> options) : base(options)
        {
        }

        public DbSet<Walks> Walks { get; set; }
        public DbSet<Region> Region { get; set; }
        public DbSet<Difficulty> Difficulty { get; set; }
    }
}
