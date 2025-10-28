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
            var posts = GetSamplePosts();
            var result = PostStatsUtils.Summarize(posts);
            Assert.Equal("Всього постів: 3, всього лайків: 3, всього коментарів: 3", result);
        }

        [Fact]
        public void Summarize_ByAuthor_ShouldReturnCorrectTotals()
        {
            var posts = GetSamplePosts();
            var result = PostStatsUtils.Summarize(posts, "author1");
            Assert.Equal("Пости користувача author1: 2, лайки: 1, коментарі: 2", result);
        }


        [Fact]
        public void Summarize_ShouldHandleEmptyList()
        {
            var posts = new List<Post>();
            var result = PostStatsUtils.Summarize(posts);
            Assert.Equal("Всього постів: 0, всього лайків: 0, всього коментарів: 0", result);
        }

        [Fact]
        public void Summarize_ByAuthor_ShouldHandleNoPostsForAuthor()
        {
            var posts = GetSamplePosts();
            var result = PostStatsUtils.Summarize(posts, "unknown");
            Assert.Equal("Пости користувача unknown: 0, лайки: 0, коментарі: 0", result);
        }

        [Fact]
        public void Summarize_ShouldHandlePostsWithNoLikesOrComments()
        {
            var posts = new List<Post>
            {
                new Post { Id = 10, Author = "authorX", Title = "Empty", Likes = new List<Like>(), Comments = new List<Comment>() }
            };
            var result = PostStatsUtils.Summarize(posts);
            Assert.Contains("всього лайків: 0", result);
            Assert.Contains("всього коментарів: 0", result);
        }
    }
}
