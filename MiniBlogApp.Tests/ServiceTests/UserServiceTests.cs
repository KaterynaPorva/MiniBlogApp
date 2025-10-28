using MiniBlogApp.Services;
using Xunit;

namespace MiniBlogApp.Tests.ServiceTests
{
    public class UserServiceTests
    {
        private readonly UserService _userService = new();

        [Fact]
        public void Authenticate_ShouldReturnUser_WhenCredentialsAreCorrect()
        {
            var user = _userService.Authenticate("serhii", "1234");

            Assert.NotNull(user);
            Assert.Equal("serhii", user.Username);
        }

        [Fact]
        public void Authenticate_ShouldReturnNull_WhenCredentialsAreIncorrect()
        {
            var user = _userService.Authenticate("serhii", "wrongpassword");

            Assert.Null(user);
        }

        [Fact]
        public void GetAll_ShouldReturnAllUsers()
        {
            var users = _userService.GetAll();

            Assert.NotEmpty(users);
            Assert.Contains(users, u => u.Username == "serhii");
            Assert.Contains(users, u => u.Username == "maria");
        }
    }
}
