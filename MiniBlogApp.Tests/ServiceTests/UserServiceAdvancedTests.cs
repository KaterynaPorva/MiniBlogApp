using System.Linq;
using MiniBlogApp.Services;
using Xunit;

namespace MiniBlogApp.Tests.ServiceTests
{
    public class UserServiceAdvancedTests
    {
        private readonly UserService _service = new();
        private const string CorrectUsername = "serhii";
        private const string CorrectPassword = "1234";
        private const string WrongCaseUsername = "Serhii";
        private const string EmptyPassword = "";
        private const int MinimumUserCount = 2;

        [Fact]
        public void Authenticate_ShouldBeCaseSensitive()
        {
            var result = _service.Authenticate(WrongCaseUsername, CorrectPassword);
            Assert.Null(result);
        }

        [Fact]
        public void Authenticate_ShouldFailForEmptyPassword()
        {
            var result = _service.Authenticate(CorrectUsername, EmptyPassword);
            Assert.Null(result);
        }

        [Fact]
        public void GetAll_ShouldReturnAtLeastTwoUsers()
        {
            Assert.True(_service.GetAll().Count() >= MinimumUserCount);
        }
    }
}
