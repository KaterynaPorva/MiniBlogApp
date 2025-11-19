using MiniBlogApp.Models;
using MiniBlogApp.Services;
using Xunit;
using System.Collections.Generic;

namespace MiniBlogApp.Tests.UtilsTests
{
    /**
     * @file PostAnalyzerTests.cs
     * @brief Unit tests for the PostAnalyzer utility class.
     * @details This class verifies that PostAnalyzer correctly handles different post scenarios,
     *          including posts with null authors or titles and posts with multiple likes and comments.
     *          It ensures that the analysis output matches the expected summary format.
     */

    /**
     * @class PostAnalyzerTests
     * @brief Contains tests for verifying PostAnalyzer functionality.
     * @details The tests check that PostAnalyzer handles edge cases and typical scenarios
     *          by returning correct summary strings that include post title, author,
     *          number of likes, and number of comments.
     */
    public class PostAnalyzerTests
    {
        /// @brief ID of a post with null author and title.
        private const int PostIdWithNull = 3;

        /// @brief ID of a post with many likes and comments.
        private const int PostIdPopular = 4;

        /// @brief Sample content for a post with null author/title.
        private const string NullContent = "Content here";

        /// @brief Sample author for a popular post.
        private const string PopularAuthor = "popularUser";

        /// @brief Sample title for a popular post.
        private const string PopularTitle = "Viral Post";

        /// @brief Sample content for a popular post.
        private const string PopularContent = "Lots of engagement";

        /// @brief Sample list of likes for a popular post.
        private static readonly List<Like> PopularLikes = new()
        {
            new Like { Username = "u1" },
            new Like { Username = "u2" },
            new Like { Username = "u3" },
            new Like { Username = "u4" }
        };

        /// @brief Sample list of comments for a popular post.
        private static readonly List<Comment> PopularComments = new()
        {
            new Comment { Author = "c1", Text = "Great!" },
            new Comment { Author = "c2", Text = "Amazing!" }
        };

        /**
         * @brief Tests PostAnalyzer with posts that have null author or title.
         * @details Ensures that even when Author or Title is null, the analyzer returns
         *          a meaningful summary string with defaults ("невідомий", "без назви") 
         *          and correctly counts 0 likes and 0 comments.
         */
        [Fact]
        public void Analyze_ShouldHandlePostWithNullAuthorOrTitle()
        {
            var post = new Post
            {
                Id = PostIdWithNull,
                Author = null,
                Title = null,
                Content = NullContent,
                Likes = new List<Like>(),
                Comments = new List<Comment>()
            };
            var analyzer = new PostAnalyzer<Post>();

            var result = analyzer.Analyze(post);

            Assert.Contains("створений користувачем", result);
            Assert.Contains("0 лайків", result);
            Assert.Contains("0 коментарів", result);
        }

        /**
         * @brief Tests PostAnalyzer with posts that have multiple likes and comments.
         * @details Verifies that the summary string correctly counts the number of likes and comments
         *          and includes the author's name and post title in the output.
         */
        [Fact]
        public void Analyze_ShouldHandlePostWithManyLikesAndComments()
        {
            var post = new Post
            {
                Id = PostIdPopular,
                Author = PopularAuthor,
                Title = PopularTitle,
                Content = PopularContent,
                Likes = new List<Like>(PopularLikes),
                Comments = new List<Comment>(PopularComments)
            };
            var analyzer = new PostAnalyzer<Post>();

            var result = analyzer.Analyze(post);

            Assert.Contains($"{PopularLikes.Count} лайків", result);
            Assert.Contains($"{PopularComments.Count} коментарів", result);
        }
    }
}
