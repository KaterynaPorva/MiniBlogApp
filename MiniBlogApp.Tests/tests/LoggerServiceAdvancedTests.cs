using System.Linq;
using MiniBlogApp.Services;
using Xunit;

namespace MiniBlogApp.Tests.ServiceTests
{
    [Collection("LoggerServiceTests")]
    public class LoggerServiceAdvancedTests
    {
        public LoggerServiceAdvancedTests()
        {
            LoggerService.ClearAll();
        }

        [Fact]
        public void AddLog_ShouldStoreDifferentTypes()
        {
            LoggerService.AddLog(new PostLogger("u", "Post"));
            LoggerService.AddLog(new LikeLogger("u", "Post"));
            LoggerService.AddLog(new CommentLogger("u", "C"));

            var logs = LoggerService.GetLogs().ToList();

            Assert.Equal(3, logs.Count);

            Assert.Contains(logs, l => l is PostLogger);
            Assert.Contains(logs, l => l is LikeLogger);
            Assert.Contains(logs, l => l is CommentLogger);
        }
    }

    [CollectionDefinition("LoggerServiceTests", DisableParallelization = true)]
    public class LoggerServiceTestsCollection { }
}
