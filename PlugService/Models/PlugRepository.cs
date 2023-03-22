using Microsoft.AspNetCore.Mvc;
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
        public async Task<Plug> CreatePlug([FromBody] PlugDto dto)
        {
            Plug plugToAdd = new Plug()
            {
                Name = dto.Name,
                AddedDate = DateTime.Now,
                Description = dto.Description,
                UserId = dto.UserId,
                IsConnected = false,
                IsTurnedOn = false,
            };
            AddPlug(plugToAdd);
            await _context.SaveChangesAsync();
            return plugToAdd;
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

        public async Task<Plug> ChangeName(Guid id, UpdatePlugDto dto)
        {
            Plug newPlug = await _context.Plugs.FirstOrDefaultAsync(p => p.Id == id);

            if (newPlug != null)
            {
                newPlug.Name = dto.Name;

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

        public async Task<IEnumerable<Plug>> GetAllPlugs() => await _context.Plugs.OrderBy(c => c.Id).ToListAsync();
    }
}
