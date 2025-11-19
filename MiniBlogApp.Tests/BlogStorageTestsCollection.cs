using Xunit;

namespace MiniBlogApp.Tests.ServiceTests
{
    /**
     * @file BlogStorageTestsCollection.cs
     * @brief Defines a test collection for BlogStorage-related unit tests.
     * @details Ensures that all tests in the "BlogStorageTests" collection are executed sequentially.
     *          This prevents interference between tests that share in-memory storage or static state
     *          such as BlogStorage.Posts or LoggerService logs.
     */

    /**
     * @class BlogStorageTestsCollection
     * @brief Collection definition for BlogStorage tests.
     * @details 
     * - Disables parallel execution to ensure test isolation.
     * - Tests that rely on shared static state (in-memory lists) are grouped under this collection.
     * 
     * Usage:
     * - Apply [Collection("BlogStorageTests")] to test classes that manipulate shared BlogStorage state.
     */
    [CollectionDefinition("BlogStorageTests", DisableParallelization = true)]
    public class BlogStorageTestsCollection
    {
        // Empty class, used only for the CollectionDefinition attribute.
    }
}
