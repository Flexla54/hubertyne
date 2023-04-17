namespace PlugService.Models
{
    public interface IPlugRepository
    {
        IEnumerable<Plug> All { get; }
        Task<Plug> CreatePlug(PlugDto dto);
        Task<Plug> CreatePlug(Guid id, DateTime connectedDate);
        Task<Plug> GetbyId(Guid id);
        Task<IEnumerable<Plug>> GetAllPlugs();
        Task<List<Plug>> GetbyUserId(Guid id);
        Task<Plug> ChangeName(Guid id, UpdatePlugDto dto);
        void AddPlug(Plug plug);
        Task<Plug> ChangePowerStatus(Guid id, bool status);
    }
}
