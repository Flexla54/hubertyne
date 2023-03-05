namespace ConnectorService.Models
{
    public interface IProvisionRepository
    {
        IEnumerable<Provision> All { get; }

        Provision CreateProvision(Guid userId, string? description);

        void DeleteProvision(Guid id);

        Provision? HasConnected(Guid id);

        Provision? GetById(Guid id);

        IEnumerable<Provision> GetByUserId(Guid userId);
    }
}
