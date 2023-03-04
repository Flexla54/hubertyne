namespace ConnectorService.Models
{
    public class ProvisionRepository : IProvisionRepository
    {
        private readonly ConnectorDbContext _context;

        public ProvisionRepository(ConnectorDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Provision> All => _context.Provisions;

        public Provision CreateProvision(Guid userId, Model model, string? description)
        {
            var provision = new Provision()
            {
                UserId = userId,
                Description = description,
                Model = model
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
    }
}
