using MiniBlogApp.Models;
using MiniBlogApp.Services;
using Xunit;
using System.Collections.Generic;

namespace MiniBlogApp.Tests.UtilsTests
{
    public class PostAnalyzerTests
    {
        [Fact]
        public void Analyze_ShouldHandlePostWithNullAuthorOrTitle()
        {
            var post = new Post
            {
                Id = 3,
                Author = null,
                Title = null,
                Content = "Content here",
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
                Id = 4,
                Author = "popularUser",
                Title = "Viral Post",
                Content = "Lots of engagement",
                Likes = new List<Like>
                {
                    new Like { Username = "u1" },
                    new Like { Username = "u2" },
                    new Like { Username = "u3" },
                    new Like { Username = "u4" }
                },
                Comments = new List<Comment>
                {
                    new Comment { Author = "c1", Text = "Great!" },
                    new Comment { Author = "c2", Text = "Amazing!" }
                }
            };

            var analyzer = new PostAnalyzer<Post>();
            var result = analyzer.Analyze(post);

            Assert.Contains("4 лайків", result);
            Assert.Contains("2 коментарів", result);
        }
    }
}
