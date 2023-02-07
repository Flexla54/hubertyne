using Microsoft.EntityFrameworkCore;

namespace IdentityService.Models
{
    public class OpenIddictDbContext : DbContext
    {
        public OpenIddictDbContext(DbContextOptions<OpenIddictDbContext> options) : base(options) { }
    }
}
