using System.Linq;
using MiniBlogApp.Models;
using MiniBlogApp.Services;
using Xunit;
using System.Collections.Generic;

namespace MiniBlogApp.Tests.ServiceTests
{
    /**
     * @file BlogStorageEdgeCaseTests.cs
     * @brief Edge case unit tests for BlogStorage functionality.
     * @details Contains tests covering unusual or extreme scenarios:
     *          - Adding comments with empty text
     *          - Case sensitivity of usernames when adding likes
     *          - Adding posts with very long content
     *          - Updating non-existent posts
     *          - Summarizing posts with no likes or comments
     *          Each test clears the in-memory storage and logs to ensure isolation.
     */

    /**
     * @class BlogStorageEdgeCaseTests
     * @brief Edge case tests for BlogStorage operations.
     * @details Ensures that BlogStorage correctly handles unusual scenarios
     *          and extreme input values.
     */
    [Collection("BlogStorageTests")]
    public class BlogStorageEdgeCaseTests
    {
        private const int VeryLongContentLength = 10000;
        private const int NonExistentPostId = 999;
        private const string EmptyText = "";
        private const string AuthorUser1 = "user1";
        private const string AuthorUser2 = "user2";
        private const string PostTitle = "Title";
        private const string PostContent = "Content";
        private const string LongContentAuthor = "author";
        private const string NewPostTitle = "NewTitle";
        private const string NewPostContent = "NewContent";
        private const string EmptyPostTitle = "EmptyPost";
        private const string LikesSummary = "всього лайків: 0";
        private const string CommentsSummary = "всього коментарів: 0";

        /**
         * @brief Constructor runs before each test.
         * @details Clears BlogStorage posts and LoggerService logs to guarantee test isolation.
         */
        public BlogStorageEdgeCaseTests()
        {
            BlogStorage.Posts.Clear();
            LoggerService.ClearAll();
        }

        /**
         * @brief Tests adding a comment with empty text.
         * @details Ensures that the system allows comments with empty content
         *          and stores them correctly.
         */
        [Fact]
        public void AddComment_ShouldHandleEmptyText()
        {
            var post = BlogStorage.AddPost(AuthorUser1, PostTitle, PostContent);

            BlogStorage.AddComment(post.Id, AuthorUser2, EmptyText);

            var updatedPost = BlogStorage.GetPostById(post.Id);
            Assert.Single(updatedPost.Comments);
            Assert.Equal(EmptyText, updatedPost.Comments.First().Text);
        }

        /**
         * @brief Tests case sensitivity for usernames when adding likes.
         * @details Ensures that "Bob" and "bob" are treated as distinct users
         *          and can both like the same post independently.
         */
        [Fact]
        public void AddLike_ShouldBeCaseSensitive_Usernames()
        {
            var post = BlogStorage.AddPost(AuthorUser1, PostTitle, PostContent);

            BlogStorage.AddLike(post.Id, "Bob");
            BlogStorage.AddLike(post.Id, "bob");

            Assert.Equal(2, post.Likes.Count);
        }

        /**
         * @brief Tests adding a post with very long content.
         * @details Verifies that the system correctly stores and handles
         *          posts with extremely large text content.
         */
        [Fact]
        public void AddPost_ShouldHandleVeryLongContent()
        {
            var longContent = new string('x', VeryLongContentLength);
            var post = BlogStorage.AddPost(LongContentAuthor, PostTitle, longContent);

            Assert.Equal(VeryLongContentLength, post.Content.Length);
        }

        /**
         * @brief Tests updating a post that does not exist.
         * @details Ensures that UpdatePost gracefully returns null
         *          when attempting to update a non-existent post.
         */
        [Fact]
        public void UpdatePost_ShouldFailGracefully_WhenPostDoesNotExist()
        {
            var result = BlogStorage.UpdatePost(NonExistentPostId, NewPostTitle, NewPostContent);

            Assert.Null(result);
        }

        /**
         * @brief Tests summarizing posts with no likes or comments.
         * @details Verifies that PostStatsUtils.Summarize correctly handles
         *          posts without any likes or comments and returns expected summary text.
         */
        [Fact]
        public void Summarize_ShouldHandlePostsWithNoLikesOrComments()
        {
            var posts = new List<Post>
            {
                new Post { Author = AuthorUser1, Title = EmptyPostTitle, Likes = new List<Like>(), Comments = new List<Comment>() }
            };

            var summary = PostStatsUtils.Summarize(posts);

            Assert.Contains(LikesSummary, summary);
            Assert.Contains(CommentsSummary, summary);
        }
    }
}
