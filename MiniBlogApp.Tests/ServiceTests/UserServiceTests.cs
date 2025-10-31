using MiniBlogApp.Services;
using Xunit;

namespace MiniBlogApp.Tests.ServiceTests
{
    public class UserServiceTests
    {
        private readonly UserService _userService = new();

        private const string CorrectUsername = "serhii";
        private const string CorrectPassword = "1234";
        private const string WrongPassword = "wrongpassword";
        private const string AnotherUser = "maria";

        [Fact]
        public void Authenticate_ShouldReturnUser_WhenCredentialsAreCorrect()
        {
            var user = _userService.Authenticate(CorrectUsername, CorrectPassword);

            Assert.NotNull(user);
            Assert.Equal(CorrectUsername, user.Username);
        }

        [Fact]
        public void Authenticate_ShouldReturnNull_WhenCredentialsAreIncorrect()
        {
            var user = _userService.Authenticate(CorrectUsername, WrongPassword);

            Assert.Null(user);
        }

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
