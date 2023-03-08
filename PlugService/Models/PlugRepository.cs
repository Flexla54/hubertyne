using Microsoft.EntityFrameworkCore;

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
        public async Task<Plug> CreatePlug(string name, DateTime date, Guid user)
        {
            Plug plug = new Plug()
            {
                Name = name,
                AddedDate = date,
                UserId = user,
                IsConnected = false,
                IsTurnedOn = false,
            };
            AddPlug(plug);
            await _context.SaveChangesAsync();
            return plug;
        }

        public async Task<Plug> ChangeConnectionStatus(Guid id, bool status)
        {
            Plug newPlug = await _context.Plugs.FirstOrDefaultAsync(p => p.Id == id);

            if (newPlug != null)
            {
                newPlug.IsConnected = status;
                
                await _context.SaveChangesAsync();
                return newPlug;
            }
            return null;
        }

        public async Task<Plug> ChangeName(Guid id, string name)
        {
            Plug newPlug = await _context.Plugs.FirstOrDefaultAsync(p => p.Id == id);

            if (newPlug != null)
            {
                newPlug.Name = name;

                await _context.SaveChangesAsync();
                return newPlug;
            }
            return null;
        }

        public async Task<Plug> ChangePowerStatus(Guid id, bool status)
        {
            Plug newPlug = await _context.Plugs.FirstOrDefaultAsync(p => p.Id == id);

            if (newPlug != null)
            {
                newPlug.IsTurnedOn = status;

                await _context.SaveChangesAsync();
                return newPlug;
            }
            return null;
        }

        public async Task<Plug> GetbyId(Guid id)
        {
            return await _context.Plugs.FirstOrDefaultAsync(_ => _.Id == id);
        }

        public async Task<List<Plug>> GetbyUserId(Guid id)
        {
            return await _context.Plugs.Where(_ => _.UserId == id).ToListAsync();
        }
    }
}
