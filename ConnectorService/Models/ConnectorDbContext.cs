using Microsoft.EntityFrameworkCore;

namespace ConnectorService.Models
{
    public class ConnectorDbContext : DbContext
    {
        public ConnectorDbContext(DbContextOptions<ConnectorDbContext> options) : base(options) { }

        public DbSet<Provision> Provisions { get; set; }
    }
}
