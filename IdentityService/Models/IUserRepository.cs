namespace IdentityService.Models
{
    public interface IUserRepository
    {
        IEnumerable<User> All { get; }

        User CreateUser(string username, string? email, string password);

        User? GetById(Guid id);

        void AddUser(User user);

        User? VerifyUser(string username, string password);
    }
}
