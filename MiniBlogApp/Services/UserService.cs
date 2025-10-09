using MiniBlogApp.Models;

namespace MiniBlogApp.Services
{
    public class UserService
    {
        private readonly List<BlogUser> _users = new()
        {
            new BlogUser { Username = "serhii", Password = "1234" },
            new BlogUser { Username = "maria",  Password = "qwerty" }
        };

        public BlogUser? Authenticate(string username, string password)
        {
            return _users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }

        public IEnumerable<BlogUser> GetAll() => _users;
    }
}
