namespace ConnectorService.Models
{
    public interface IProvisionRepository
    {
        IEnumerable<Provision> All { get; }

        Provision CreateProvision(Guid userId, Model model, string? description);

        void DeleteProvision(Guid id);

        Provision? GetById(Guid id);

        IEnumerable<Provision> GetByUserId(Guid userId);
    }
}
