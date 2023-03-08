namespace ProvisionService.Models
{
    public class ProvisionRepository : IProvisionRepository
    {
        private readonly ProvisionDbContext _context;

        public ProvisionRepository(ProvisionDbContext context)
        {
            _context = context;
        }

        public IQueryable<Provision> All => _context.Provisions;

        public Provision CreateProvision(Guid userId, string? description)
        {
            var provision = new Provision()
            {
                UserId = userId,
                Description = description,
            };

            _context.Provisions.Add(provision);
            _context.SaveChanges();

            return provision;
        }

        public void DeleteProvision(Guid id)
        {
            var provision = new Provision() { Id = id };

            _context.Provisions.Attach(provision);
            _context.Provisions.Remove(provision);
            _context.SaveChanges();
        }

        public Provision? GetById(Guid id)
        {
            return _context.Provisions.Where(_ => _.Id == id).FirstOrDefault();
        }

        public IEnumerable<Provision> GetByUserId(Guid userId)
        {
            return _context.Provisions.Where(_ => _.UserId == userId);
        }

        public Provision? HasConnected(Guid id)
        {
            var provision = _context.Provisions.Where(p => p.Id == id).FirstOrDefault();

            if (provision == null)
            {
                return null;
            }

            provision.HasConnected = true;

            _context.SaveChanges();

            return provision;
        }
    }
}
