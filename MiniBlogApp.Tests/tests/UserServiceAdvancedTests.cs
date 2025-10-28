using System.Linq;
using MiniBlogApp.Services;
using Xunit;

namespace MiniBlogApp.Tests.ServiceTests
{
    public class UserServiceAdvancedTests
    {
        private readonly UserService _service = new();

        [Fact]
        public void Authenticate_ShouldBeCaseSensitive()
        {
            var result = _service.Authenticate("Serhii", "1234");
            Assert.Null(result);
        }

        [Fact]
        public void Authenticate_ShouldFailForEmptyPassword()
        {
            var result = _service.Authenticate("serhii", "");
            Assert.Null(result);
        }

        [Fact]
        public void GetAll_ShouldReturnAtLeastTwoUsers()
        {
            Assert.True(_service.GetAll().Count() >= 2);
        }
    }
}
