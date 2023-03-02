namespace PlugService.Models
{
    public class PlugRepository : IPlugRepository
    {
        private readonly PlugManagementDbContext _context;
        public PlugRepository(PlugManagementDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Plug> All => _context.Plugs;

        public void AddPlug(Plug plug)
        {
            _context.Plugs.Add(plug);
        }
        public Plug CreatePlug(string name, Guid user)
        {
            Plug plug = new Plug()
            {
                Name = name,
                UserId = user,
                IsConnected = false,
                IsTurnedOn = false,
            };
            AddPlug(plug);
            _context.SaveChanges();
            return plug;
        }

        public Plug ChangeConnectionStatus(Guid id, bool status)
        {
            throw new NotImplementedException();
        }

        public Plug ChangeName(Guid id, string name)
        {
            throw new NotImplementedException();
        }

        public Plug ChangePowerStatus(Guid id, bool status)
        {
            throw new NotImplementedException();
        }

        public Plug GetbyId(Guid id)
        {
            return _context.Plugs.Where(_ => _.Id == id).First();
        }

        public List<Plug> GetbyUserId(Guid id)
        {
            return _context.Plugs.Where(_ => _.UserId == id).ToList();
        }
    }
}
