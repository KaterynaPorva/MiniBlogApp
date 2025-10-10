using MiniBlogApp.Models;
using MiniBlogApp.Services;
using Xunit;

namespace MiniBlogApp.Tests.UtilsTests
{
    public class PostAnalyzerTests
    {
        [Fact]
        public void Analyze_ShouldReturnCorrectSummary()
        {
            // Arrange
            var post = new Post
            {
                Id = 1,
                Author = "author1",
                Title = "Test Post",
                Content = "Some content",
                Likes = { new Like { Username = "user1" }, new Like { Username = "user2" } },
                Comments = { new Comment { Author = "commenter", Text = "Nice post!" } }
            };

            var analyzer = new PostAnalyzer<Post>();

            // Act
            var result = analyzer.Analyze(post);

            // Assert
            Assert.Equal(
                "Пост 'Test Post' створений користувачем author1, має 2 лайків і 1 коментарів.",
                result
            );
        }
    }
}
