using System.Linq;
using MiniBlogApp.Services;
using Xunit;
using MiniBlogApp.Models;

namespace MiniBlogApp.Tests.ServiceTests
{
    /**
     * @file BlogStorageAddLikeTests.cs
     * @brief Unit tests for BlogStorage.AddLike functionality.
     * @details Ensures that likes are correctly added to posts, prevents duplicate likes,
     *          and handles the case when the post does not exist.
     *          Tests use in-memory storage and clear logs before each test.
     */

    /**
     * @class BlogStorageAddLikeTests
     * @brief Tests for adding likes to posts in BlogStorage.
     * @details Includes tests for adding a first like, preventing duplicate likes,
     *          and ensuring correct behavior when the post ID does not exist.
     */
    [Collection("BlogStorageTests")]
    public class BlogStorageAddLikeTests
    {
        private const string Author = "author1";
        private const string Title = "title1";
        private const string Content = "content1";
        private const string User = "user1";
        private const int NonExistentPostId = 999;

        /**
         * @brief Constructor runs before each test.
         * @details Clears the in-memory Posts list and LoggerService logs
         *          to ensure a clean state for testing.
         */
        public BlogStorageAddLikeTests()
        {
            BlogStorage.Posts.Clear();
            LoggerService.ClearAll();
        }

        /**
         * @brief Verifies that a like is added when the user has not liked the post before.
         * @details Creates a new post and adds a like from 'user1'.
         *          Asserts that the post has exactly one like and that the like belongs to 'user1'.
         * @return void
         */
        [Fact]
        public void AddLike_ShouldAddLike_WhenUserHasNotLikedBefore()
        {
            var post = BlogStorage.AddPost(Author, Title, Content);

            BlogStorage.AddLike(post.Id, User);

            Assert.Single(post.Likes);
            Assert.Equal(User, post.Likes.First().Username);
        }

        /**
         * @brief Ensures that duplicate likes from the same user are not added.
         * @details Adds a like from 'user1' twice and asserts that only one like is stored.
         * @return void
         */
        [Fact]
        public void AddLike_ShouldNotAddDuplicateLikes()
        {
            var post = BlogStorage.AddPost(Author, Title, Content);
            BlogStorage.AddLike(post.Id, User);

            BlogStorage.AddLike(post.Id, User);

            Assert.Single(post.Likes);
            Assert.Equal(User, post.Likes.First().Username);
        }

        /**
         * @brief Verifies behavior when attempting to add a like to a non-existent post.
         * @details Calls AddLike with a post ID that does not exist.
         *          Asserts that no posts or likes are added to the storage.
         * @return void
         */
        [Fact]
        public void AddLike_ShouldDoNothing_WhenPostDoesNotExist()
        {
            BlogStorage.AddLike(NonExistentPostId, User);

            Assert.Empty(BlogStorage.Posts);
        }
    }
}
