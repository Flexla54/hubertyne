using Microsoft.EntityFrameworkCore;
using EntityFramework.Exceptions.PostgreSQL;
namespace PlugService.Models
{
    public class PlugManagementDbContext : DbContext
    {
        public PlugManagementDbContext(DbContextOptions<PlugManagementDbContext> options) : base(options) { }
        public DbSet<Plug> Plugs { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseExceptionProcessor();
        }
    }
}
