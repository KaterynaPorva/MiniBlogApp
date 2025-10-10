using System.Collections.Generic;
using MiniBlogApp.Models;
using MiniBlogApp.Services;
using Xunit;

namespace MiniBlogApp.Tests.UtilsTests
{
    public class PostStatsUtilsTests
    {
        private List<Post> GetSamplePosts()
        {
            var post1 = new Post
            {
                Id = 1,
                Author = "author1",
                Title = "Post 1",
                Likes = { new Like { Username = "user1" } },
                Comments = { new Comment { Author = "user2", Text = "Comment 1" }, new Comment { Author = "user3", Text = "Comment 2" } }
            };

            var post2 = new Post
            {
                Id = 2,
                Author = "author2",
                Title = "Post 2",
                Likes = { new Like { Username = "user4" }, new Like { Username = "user5" } },
                Comments = { new Comment { Author = "user1", Text = "Comment 3" } }
            };

            var post3 = new Post
            {
                Id = 3,
                Author = "author1",
                Title = "Post 3",
                Likes = { },
                Comments = { }
            };

            return new List<Post> { post1, post2, post3 };
        }

        [Fact]
        public void Summarize_ShouldReturnCorrectTotals()
        {
            // Arrange
            var posts = GetSamplePosts();

            // Act
            var result = PostStatsUtils.Summarize(posts);

            // Assert
            Assert.Equal("Всього постів: 3, всього лайків: 3, всього коментарів: 3", result);
        }

        [Fact]
        public void Summarize_ByAuthor_ShouldReturnCorrectTotals()
        {
            // Arrange
            var posts = GetSamplePosts();

            // Act
            var result = PostStatsUtils.Summarize(posts, "author1");

            // Assert
            Assert.Equal("Пости користувача author1: 2, лайки: 1, коментарі: 2", result);
        }
    }
}
