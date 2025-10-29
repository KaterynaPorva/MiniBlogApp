using MiniBlogApp.Models;
using MiniBlogApp.Services;
using Xunit;
using System.Collections.Generic;

namespace MiniBlogApp.Tests.UtilsTests
{
    public class PostAnalyzerTests
    {
        private const int PostIdWithNull = 3;
        private const int PostIdPopular = 4;
        private const string NullContent = "Content here";
        private const string PopularAuthor = "popularUser";
        private const string PopularTitle = "Viral Post";
        private const string PopularContent = "Lots of engagement";
        private static readonly List<Like> PopularLikes = new()
        {
            new Like { Username = "u1" },
            new Like { Username = "u2" },
            new Like { Username = "u3" },
            new Like { Username = "u4" }
        };
        private static readonly List<Comment> PopularComments = new()
        {
            new Comment { Author = "c1", Text = "Great!" },
            new Comment { Author = "c2", Text = "Amazing!" }
        };

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
