using System.Linq;
using MiniBlogApp.Services;
using Xunit;
using MiniBlogApp.Models;
using System.Collections.Generic;

namespace MiniBlogApp.Tests.ServiceTests
{
    /**
     * @file BlogStorageAddPostTests.cs
     * @brief Unit tests for BlogStorage.AddPost functionality.
     * @details Ensures that new posts are correctly added to the in-memory storage.
     *          Clears existing posts and logs before each test to guarantee isolation.
     */

    /**
     * @class BlogStorageAddPostTests
     * @brief Tests for adding posts to BlogStorage.
     * @details Includes tests for verifying that posts are correctly added and stored,
     *          and that their properties match the expected values.
     */
    [Collection("BlogStorageTests")]
    public class BlogStorageAddPostTests
    {
        private const string Author = "testuser";
        private const string Title = "Тестовий пост";
        private const string Content = "Це контент для тесту.";

        /**
         * @brief Constructor runs before each test.
         * @details Clears the in-memory Posts list and LoggerService logs
         *          to ensure a clean state for testing.
         */
        public BlogStorageAddPostTests()
        {
            BlogStorage.Posts.Clear();
            LoggerService.ClearAll();
        }

        /**
         * @brief Verifies that a new post is correctly added to storage.
         * @details Adds a post and asserts that the total post count increases by one.
         *          Also checks that the post's properties (Id, Author, Title, Content)
         *          match the expected values.
         * @return void
         */
        [Fact]
        public void AddPost_ShouldAddPostCorrectly()
        {
            var initialCount = BlogStorage.GetAllPosts().Count();

            var post = BlogStorage.AddPost(Author, Title, Content);
            var allPosts = BlogStorage.GetAllPosts().ToList();

            Assert.Equal(initialCount + 1, allPosts.Count);
            Assert.Contains(allPosts, p =>
                p.Id == post.Id &&
                p.Author == Author &&
                p.Title == Title &&
                p.Content == Content);
        }
    }
}
