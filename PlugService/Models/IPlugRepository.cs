namespace PlugService.Models
{
    public interface IPlugRepository
    {
        IEnumerable<Plug> All { get; }
        Task<Plug> CreatePlug(string name, DateTime date, Guid user);
        Task<Plug> GetbyId(Guid id);
        Task<List<Plug>> GetbyUserId(Guid id);
        Task<Plug> ChangeName(Guid id, string name);
        void AddPlug(Plug plug);

        Task<Plug> ChangePowerStatus(Guid id, bool status);
        Task<Plug> ChangeConnectionStatus(Guid id, bool status);

    }
}
