using MiniBlogApp.Services;
using Xunit;
using System.Linq;

/**
 * @file LoggerServiceAdvancedTests.cs
 * @brief Advanced tests for LoggerService functionality.
 * @details
 * This test class verifies that LoggerService correctly logs messages for various user actions,
 * including creating posts, liking posts, and commenting. Ensures that messages are stored accurately
 * and that the log collection behaves as expected.
 */

/**
 * @class LoggerServiceAdvancedTests
 * @brief Advanced tests for LoggerService.
 * @details Verifies that LoggerService correctly stores messages for different user actions
 *          and that the messages are in the expected format.
 */
[Collection("LoggerServiceTests")]
public class LoggerServiceAdvancedTests
{
    private const string User = "u";
    private const string PostContent = "Post";
    private const string CommentContent = "C";
    private const int ExpectedLogCount = 3;

    /**
     * @brief Constructor for the test class.
     * @details Clears all logs before each test to ensure a clean state.
     */
    public LoggerServiceAdvancedTests()
    {
        LoggerService.ClearAll();
    }

    /**
     * @brief Verifies that LoggerService stores messages for different types of user actions.
     * @details Adds logs for a post creation, a like, and a comment. Retrieves all log messages and asserts that:
     *          1. The total number of log messages matches the expected count.
     *          2. Each type of action has an appropriate log message.
     * @return void
     * @throws Xunit.Sdk.XunitException If any assertion fails.
     */
    [Fact]
    public void AddLog_ShouldStoreMessagesForDifferentActions()
    {
        LoggerService.AddLog(new PostLogger(User, PostContent));
        LoggerService.AddLog(new LikeLogger(User, PostContent));
        LoggerService.AddLog(new CommentLogger(User, CommentContent));

        var messages = LoggerService.GetLogs().Select(l => l.GetMessage()).ToList();

        Assert.Equal(ExpectedLogCount, messages.Count);

        Assert.Contains(messages, msg => msg.Contains("created post") || msg.Contains(PostContent));
        Assert.Contains(messages, msg => msg.Contains("liked") || msg.Contains(PostContent));
        Assert.Contains(messages, msg => msg.Contains("commented") || msg.Contains(CommentContent));
    }
}

/**
 * @class LoggerServiceTestsCollection
 * @brief Test collection for LoggerService tests.
 * @details Disables parallel test execution to avoid conflicts with the shared LoggerService state.
 */
[CollectionDefinition("LoggerServiceTests", DisableParallelization = true)]
public class LoggerServiceTestsCollection { }
