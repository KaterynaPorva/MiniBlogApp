using MiniBlogApp.Models;

namespace MiniBlogApp.Services
{
    /**
     * @file UserService.cs
     * @brief Provides user management and authentication services for the blog.
     * @details Contains UserService class with in-memory predefined users.
     *          Allows authenticating users and retrieving the list of all users.
     * @example UserService.cs
     * @code
     * var userService = new UserService();
     * var user = userService.Authenticate("serhii", "1234");
     * // user.Username == "serhii"
     * var invalidUser = userService.Authenticate("serhii", "wrong");
     * // invalidUser == null
     * @endcode
     */

    /**
     * @class UserService
     * @brief Service for managing blog users and authentication.
     * @details Provides methods for authenticating users and retrieving all registered users.
     *          Currently, the service has two predefined users: 'serhii' and 'maria'.
     */
    public class UserService
    {
        /**
         * @brief List of registered users.
         * @details Predefined in-memory users for demonstration purposes.
         */
        private readonly List<BlogUser> _users = new()
        {
            new BlogUser { Username = "serhii", Password = "1234" },
            new BlogUser { Username = "maria",  Password = "qwerty" }
        };

        /**
         * @brief Authenticates a user by username and password.
         * @param username Username provided by the user.
         * @param password Password provided by the user.
         * @return BlogUser? Returns the authenticated user if credentials match; otherwise, null.
         * @details Checks the provided username and password against the predefined list of users.
         */
        public BlogUser? Authenticate(string username, string password)
        {
            return _users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }

        /**
         * @brief Retrieves all registered users.
         * @return IEnumerable<BlogUser> Collection of all users.
         * @details Returns the in-memory list of all predefined users.
         */
        public IEnumerable<BlogUser> GetAll() => _users;
    }
}
