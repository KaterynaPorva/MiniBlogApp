using System.Linq;
using MiniBlogApp.Services;
using Xunit;

namespace MiniBlogApp.Tests.ServiceTests
{
    [Collection("LoggerServiceTests")]
    public class LoggerServiceAdvancedTests
    {
        private const string User = "u";
        private const string PostContent = "Post";
        private const string CommentContent = "C";
        private const int ExpectedLogCount = 3;

        public LoggerServiceAdvancedTests()
        {
            LoggerService.ClearAll();
        }

        [Fact]
        public void AddLog_ShouldStoreMessagesForDifferentActions()
        {
            LoggerService.AddLog(new PostLogger(User, PostContent));
            LoggerService.AddLog(new LikeLogger(User, PostContent));
            LoggerService.AddLog(new CommentLogger(User, CommentContent));

            var messages = LoggerService.GetLogs().Select(l => l.GetMessage()).ToList();

            Assert.Equal(ExpectedLogCount, messages.Count);
            Assert.Contains(messages, msg => msg.Contains("створив пост") || msg.Contains("Post"));
            Assert.Contains(messages, msg => msg.Contains("лайк") || msg.Contains("Post"));
            Assert.Contains(messages, msg => msg.Contains("залишив коментар") || msg.Contains("C"));
        }
    }

    [CollectionDefinition("LoggerServiceTests", DisableParallelization = true)]
    public class LoggerServiceTestsCollection { }
}
