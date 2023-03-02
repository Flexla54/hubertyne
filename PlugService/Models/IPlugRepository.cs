namespace PlugService.Models
{
    public interface IPlugRepository
    {
        IEnumerable<Plug> All { get; }
        Plug CreatePlug(string name, Guid user);
        Plug GetbyId(Guid id);
        List<Plug> GetbyUserId(Guid id);
        Plug ChangeName(Guid id, string name);
        void AddPlug(Plug plug);

        Plug ChangePowerStatus(Guid id, bool status);
        Plug ChangeConnectionStatus(Guid id, bool status);

    }
}
