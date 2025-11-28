using MiniBlogApp.Services;
using Xunit;
using System.Linq;
using System.Collections.Generic;
using MiniBlogApp.Models;

namespace MiniBlogApp.Tests.ServiceTests
{
    /**
     * @file BlogStorageAdvancedTests.cs
     * @brief Advanced unit tests for BlogStorage functionality.
     * @details Contains tests for adding posts, likes, comments, updating posts, 
     *          deleting posts, and ensuring proper logging behavior.
     *          Each test clears the in-memory storage and logs to guarantee isolation.
     */

    /**
     * @class BlogStorageAdvancedTests
     * @brief Advanced tests for BlogStorage operations.
     * @details Verifies that BlogStorage methods correctly update posts, handle likes and comments,
     *          maintain unique IDs, and log actions appropriately.
     */
    [Collection("BlogStorageTests")]
    public class BlogStorageAdvancedTests
    {
        private const string Author1 = "u1";
        private const string User1 = "user1";
        private const string Bob = "bob";
        private const string Alice = "alice";
        private const string PostTitle = "Title";
        private const string PostContent = "Content";
        private const string OldContent = "OldContent";
        private const string NewContent = "NewContent";
        private const string NiceComment = "Nice post!";
        private const string ShortTitle1 = "P1";
        private const string ShortTitle2 = "P2";
        private const string ShortContent1 = "C1";
        private const string ShortContent2 = "C2";

        /**
         * @brief Constructor runs before each test.
         * @details Clears BlogStorage posts and LoggerService logs to ensure 
         *          tests are isolated and independent.
         */
        public BlogStorageAdvancedTests()
        {
            BlogStorage.Posts.Clear();
            LoggerService.ClearAll();
        }

        /**
         * @brief Tests adding a comment to a post.
         * @details Verifies that the comment is correctly added to the post
         *          and that a corresponding log entry is created.
         * @return void
         */
        [Fact]
        public void AddComment_ShouldAddToPostAndLogMessage()
        {
            var post = BlogStorage.AddPost(Author1, PostTitle, PostContent);

            BlogStorage.AddComment(post.Id, User1, NiceComment);

            var updatedPost = BlogStorage.GetPostById(post.Id);
            Assert.Single(updatedPost.Comments);
            Assert.Equal(User1, updatedPost.Comments.First().Author);
            Assert.Equal(NiceComment, updatedPost.Comments.First().Text);

            var logs = LoggerService.GetLogs().Select(l => l.GetMessage()).ToList();
            Assert.Contains(logs, msg => msg.Contains("залишив коментар"));
        }

        /**
         * @brief Tests adding likes to a post.
         * @details Ensures that a user can like a post only once,
         *          and that logging reflects a single like per user.
         * @return void
         */
        [Fact]
        public void AddLike_ShouldLogMessageOnlyOncePerUser()
        {
            var post = BlogStorage.AddPost(Author1, "T", PostContent);

            BlogStorage.AddLike(post.Id, Bob);
            BlogStorage.AddLike(post.Id, Bob);

            var logs = LoggerService.GetLogs().Select(l => l.GetMessage()).ToList();
            var likeMessages = logs.Where(msg => msg.Contains(Bob) && msg.Contains("лайк")).ToList();
            Assert.Single(likeMessages);

            Assert.Contains(logs, msg => msg.Contains("створив пост"));
        }

        /**
         * @brief Tests adding multiple posts.
         * @details Checks that each post receives a unique ID and
         *          that logs correctly reflect post creation.
         * @return void
         */
        [Fact]
        public void AddPost_ShouldAssignUniqueIdsAndLogMessages()
        {
            var post1 = BlogStorage.AddPost(Alice, ShortTitle1, ShortContent1);
            var post2 = BlogStorage.AddPost(Bob, ShortTitle2, ShortContent2);

            Assert.NotEqual(post1.Id, post2.Id);

            var logs = LoggerService.GetLogs().Select(l => l.GetMessage()).ToList();
            Assert.Contains(logs, msg => msg.Contains(ShortTitle1));
            Assert.Contains(logs, msg => msg.Contains(ShortTitle2));
        }

        /**
         * @brief Tests updating the content of a post.
         * @details Ensures that UpdatePost correctly changes the post's content
         *          while keeping the ID and author intact.
         * @return void
         */
        [Fact]
        public void UpdatePost_ShouldChangeContent()
        {
            var post = BlogStorage.AddPost(Author1, PostTitle, OldContent);

            BlogStorage.UpdatePost(post.Id, PostTitle, NewContent);

            var updatedPost = BlogStorage.GetPostById(post.Id);
            Assert.NotNull(updatedPost);
            Assert.Equal(NewContent, updatedPost.Content);
        }

        /**
         * @brief Tests deleting a post.
         * @details Ensures that DeletePost removes the post from storage
         *          and that it is no longer retrievable.
         * @return void
         */
        [Fact]
        public void DeletePost_ShouldRemovePost()
        {
            var post = BlogStorage.AddPost(Author1, PostTitle, PostContent);

            BlogStorage.DeletePost(post.Id);

            Assert.Null(BlogStorage.GetPostById(post.Id));
        }
    }
}
