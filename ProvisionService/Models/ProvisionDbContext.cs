using Microsoft.EntityFrameworkCore;

namespace ProvisionService.Models
{
    public class ProvisionDbContext : DbContext
    {
        public ProvisionDbContext(DbContextOptions<ProvisionDbContext> options) : base(options) { }

        public DbSet<Provision> Provisions { get; set; }
    }
}
