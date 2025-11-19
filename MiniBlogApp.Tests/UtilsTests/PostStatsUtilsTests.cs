using System.Collections.Generic;
using MiniBlogApp.Models;
using MiniBlogApp.Services;
using Xunit;

namespace MiniBlogApp.Tests.UtilsTests
{
    /**
     * @file PostStatsUtilsTests.cs
     * @brief Unit tests for the PostStatsUtils helper class.
     * @details This class tests summarization of posts including totals for likes, comments,
     *          and posts overall. It also verifies filtering by author and handling of empty lists.
     */

    /**
     * @class PostStatsUtilsTests
     * @brief Contains unit tests for PostStatsUtils.
     * @details Verifies that:
     *          - Summarization of all posts returns correct totals.
     *          - Summarization by author correctly filters posts.
     *          - Edge cases, such as empty post lists or authors with no posts, are handled gracefully.
     */
    public class PostStatsUtilsTests
    {
        /// @brief Sample authors used in test posts.
        private const string Author1 = "author1";
        private const string Author2 = "author2";
        private const string UnknownAuthor = "unknown";

        /// @brief Sample users who like posts or comment.
        private const string User1 = "user1";
        private const string User2 = "user2";
        private const string User3 = "user3";
        private const string User4 = "user4";
        private const string User5 = "user5";

        /// @brief Sample comment texts.
        private const string Comment1Text = "Comment 1";
        private const string Comment2Text = "Comment 2";
        private const string Comment3Text = "Comment 3";

        /// @brief Sample post titles.
        private const string Post1Title = "Post 1";
        private const string Post2Title = "Post 2";
        private const string Post3Title = "Post 3";

        /**
         * @brief Returns a sample list of posts for testing.
         * @details Each post includes a combination of likes and comments to cover different scenarios:
         *          - post1: author1, 1 like, 2 comments
         *          - post2: author2, 2 likes, 1 comment
         *          - post3: author1, no likes, no comments
         * @return List<Post> Sample posts used in unit tests.
         */
        private List<Post> GetSamplePosts()
        {
            var post1 = new Post
            {
                Id = 1,
                Author = Author1,
                Title = Post1Title,
                Likes = { new Like { Username = User1 } },
                Comments = {
                    new Comment { Author = User2, Text = Comment1Text },
                    new Comment { Author = User3, Text = Comment2Text }
                }
            };

            var post2 = new Post
            {
                Id = 2,
                Author = Author2,
                Title = Post2Title,
                Likes = { new Like { Username = User4 }, new Like { Username = User5 } },
                Comments = { new Comment { Author = User1, Text = Comment3Text } }
            };

            var post3 = new Post
            {
                Id = 3,
                Author = Author1,
                Title = Post3Title,
                Likes = { },
                Comments = { }
            };

            return new List<Post> { post1, post2, post3 };
        }

        /**
         * @brief Verifies that summarization of all posts returns correct totals.
         * @details Checks that the summary string contains the total number of posts, likes, and comments.
         */
        [Fact]
        public void Summarize_ShouldReturnCorrectTotals()
        {
            var posts = GetSamplePosts();
            var result = PostStatsUtils.Summarize(posts);

            Assert.Contains("3", result); // total posts
            Assert.Contains("3", result); // total likes
            Assert.Contains("3", result); // total comments
        }

        /**
         * @brief Verifies that summarization by a specific author returns correct totals.
         * @details Ensures that only posts from the specified author are included,
         *          and likes/comments counts are accurate.
         */
        [Fact]
        public void Summarize_ByAuthor_ShouldReturnCorrectTotals()
        {
            var posts = GetSamplePosts();
            var result = PostStatsUtils.Summarize(posts, Author1);

            Assert.Contains("author1", result);
            Assert.Contains("2", result); // number of posts by author1
            Assert.Contains("1", result); // total likes by author1
            Assert.Contains("2", result); // total comments by author1
        }

        /**
         * @brief Verifies that summarization handles an empty list of posts.
         * @details Ensures that calling Summarize with no posts returns 0 totals for posts, likes, and comments.
         */
        [Fact]
        public void Summarize_ShouldHandleEmptyList()
        {
            var posts = new List<Post>();
            var result = PostStatsUtils.Summarize(posts);

            Assert.Contains("0", result); // posts
            Assert.Contains("0", result); // likes
            Assert.Contains("0", result); // comments
        }

        /**
         * @brief Verifies that summarization handles an author with no posts.
         * @details Ensures that calling Summarize for an author who has no posts returns 0 totals
         *          and includes the author's name in the summary.
         */
        [Fact]
        public void Summarize_ByAuthor_ShouldHandleNoPostsForAuthor()
        {
            var posts = GetSamplePosts();
            var result = PostStatsUtils.Summarize(posts, UnknownAuthor);

            Assert.Contains("unknown", result);
            Assert.Contains("0", result); // posts
            Assert.Contains("0", result); // likes
            Assert.Contains("0", result); // comments
        }
    }
}
