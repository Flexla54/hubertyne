using Microsoft.EntityFrameworkCore;

namespace ConnectorService.Models
{
    public class ConnectorDbContext : DbContext
    {
        public ConnectorDbContext(DbContextOptions<ConnectorDbContext> options) : base(options) { }

        public DbSet<Provision> Provisions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Provision>()
                .ToTable("Provisions");

            modelBuilder.Entity<Provision>()
                .Property(c => c.Model)
                .HasConversion<string>();
        }
    }
}
