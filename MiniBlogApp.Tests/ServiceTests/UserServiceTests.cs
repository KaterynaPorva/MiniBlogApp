using MiniBlogApp.Services;
using Xunit;

namespace MiniBlogApp.Tests.ServiceTests
{
    /**
     * @file UserServiceTests.cs
     * @brief Basic unit tests for the UserService class.
     * @details 
     * This test class verifies the fundamental functionality of the UserService, 
     * which handles user authentication and retrieval. The tests ensure that:
     * - Users with correct credentials are authenticated successfully.
     * - Users with incorrect credentials are not authenticated.
     * - GetAll() correctly returns all predefined users.
     * 
     * Each test uses the in-memory user collection provided by UserService.
     */

    /**
     * @class UserServiceTests
     * @brief Contains unit tests for basic UserService functionality.
     * @details Focuses on verifying authentication and user retrieval
     *          under normal conditions using predefined users.
     */
    public class UserServiceTests
    {
        /** @brief Instance of the UserService under test. */
        private readonly UserService _userService = new();

        /** @brief Correct username for authentication tests. */
        private const string CorrectUsername = "serhii";

        /** @brief Correct password for authentication tests. */
        private const string CorrectPassword = "1234";

        /** @brief Incorrect password to test failed authentication. */
        private const string WrongPassword = "wrongpassword";

        /** @brief Another predefined username to verify GetAll() functionality. */
        private const string AnotherUser = "maria";

        /**
         * @brief Tests successful authentication with correct credentials.
         * @details Ensures that when both username and password match a 
         *          predefined user, the Authenticate method returns a valid 
         *          BlogUser instance with the expected username.
         */
        [Fact]
        public void Authenticate_ShouldReturnUser_WhenCredentialsAreCorrect()
        {
            var user = _userService.Authenticate(CorrectUsername, CorrectPassword);

            Assert.NotNull(user);
            Assert.Equal(CorrectUsername, user.Username);
        }

        /**
         * @brief Tests failed authentication with incorrect credentials.
         * @details Confirms that Authenticate returns null if the password
         *          does not match the stored value for the given username.
         */
        [Fact]
        public void Authenticate_ShouldReturnNull_WhenCredentialsAreIncorrect()
        {
            var user = _userService.Authenticate(CorrectUsername, WrongPassword);

            Assert.Null(user);
        }

        /**
         * @brief Tests that GetAll() returns all predefined users.
         * @details Verifies that the returned collection contains both
         *          "serhii" and "maria", confirming that the in-memory
         *          user store is correctly exposed by the service.
         */
        [Fact]
        public void GetAll_ShouldReturnAllUsers()
        {
            var users = _userService.GetAll();

            Assert.NotEmpty(users);
            Assert.Contains(users, u => u.Username == CorrectUsername);
            Assert.Contains(users, u => u.Username == AnotherUser);
        }
    }
}
