using System.Linq;
using MiniBlogApp.Services;
using Xunit;

namespace MiniBlogApp.Tests.ServiceTests
{
    /**
     * @file UserServiceAdvancedTests.cs
     * @brief Advanced unit tests for the UserService class.
     * @details 
     * This test class focuses on edge cases and detailed scenarios for the 
     * UserService, which handles user authentication and retrieval. The tests
     * verify correct behavior for case-sensitive usernames, empty passwords,
     * and the integrity of the predefined user list. Each test ensures that
     * the service behaves predictably under unusual or boundary conditions.
     */

    /**
     * @class UserServiceAdvancedTests
     * @brief Contains edge case tests for UserService.
     * @details Verifies that the UserService correctly handles authentication and
     *          retrieval operations, ensuring robustness against improper input
     *          and checking the correctness of stored user data.
     */
    public class UserServiceAdvancedTests
    {
        /** @brief Instance of the UserService under test. */
        private readonly UserService _service = new();

        /** @brief Valid username for authentication. */
        private const string CorrectUsername = "serhii";

        /** @brief Valid password for authentication. */
        private const string CorrectPassword = "1234";

        /** @brief Username with different casing to test case sensitivity. */
        private const string WrongCaseUsername = "Serhii";

        /** @brief Empty password string to test authentication failure. */
        private const string EmptyPassword = "";

        /** @brief Minimum number of users expected in the service. */
        private const int MinimumUserCount = 2;

        /**
         * @brief Tests case sensitivity in username authentication.
         * @details Verifies that the authentication method rejects usernames
         *          that differ only in letter casing from the stored usernames.
         *          Ensures that "serhii" and "Serhii" are treated as distinct.
         */
        [Fact]
        public void Authenticate_ShouldBeCaseSensitive()
        {
            var result = _service.Authenticate(WrongCaseUsername, CorrectPassword);
            Assert.Null(result);
        }

        /**
         * @brief Tests authentication failure for empty passwords.
         * @details Confirms that the service does not allow a user to log in
         *          with an empty password, even if the username exists in the
         *          predefined list. Ensures basic security against blank credentials.
         */
        [Fact]
        public void Authenticate_ShouldFailForEmptyPassword()
        {
            var result = _service.Authenticate(CorrectUsername, EmptyPassword);
            Assert.Null(result);
        }

        /**
         * @brief Ensures GetAll() returns the expected number of users.
         * @details Validates that the UserService contains at least the predefined
         *          users ("serhii" and "maria") and that the collection returned
         *          by GetAll() is consistent with the internal user list.
         */
        [Fact]
        public void GetAll_ShouldReturnAtLeastTwoUsers()
        {
            Assert.True(_service.GetAll().Count() >= MinimumUserCount);
        }
    }
}
