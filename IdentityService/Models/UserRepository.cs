using Microsoft.AspNetCore.Identity;

namespace IdentityService.Models
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManagementDbContext _context;

        public UserRepository(UserManagementDbContext context)
        {
            _context = context;
        }

        public IEnumerable<User> All => _context.Users;

        public void AddUser(User user)
        {
            _context.Users.Add(user);
        }

        public User CreateUser(string username, string? email, string password) 
        {

            PasswordHasher<string> hasher = new PasswordHasher<string>();

            string hash = hasher.HashPassword(username, password);

            User user = new User()
            {
                Email = email,
                Username = username,
                PasswordHash = hash
            };

            AddUser(user);

            _context.SaveChanges();

            return user;
        }

        public User? GetById(Guid id)
        {
            return _context.Users.Where(_ => _.Id == id).First();
        }

        public User? VerifyUser(string username, string password)
        {
            User? user = _context.Users.Where(_ => _.Username == username).FirstOrDefault();

            if (user is null)
            {
                return null;
            }

            PasswordHasher<string> hasher = new PasswordHasher<string>();

            var result = hasher.VerifyHashedPassword(user.Username, user.PasswordHash, password);

            if (result == PasswordVerificationResult.Success )
            {
                return user;
            }

            return null;
        }
    }
}
