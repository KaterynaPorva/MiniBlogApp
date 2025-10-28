using System.Linq;
using MiniBlogApp.Models;
using MiniBlogApp.Services;
using Xunit;
using System.Collections.Generic;

namespace MiniBlogApp.Tests.ServiceTests
{
    public class BlogStorageEdgeCaseTests
    {
        public BlogStorageEdgeCaseTests()
        {
            BlogStorage.Posts.Clear();
            LoggerService.ClearAll();
        }

        [Fact]
        public void AddComment_ShouldHandleEmptyText()
        {
            var post = BlogStorage.AddPost("user1", "Title", "Content");

            BlogStorage.AddComment(post.Id, "user2", "");

            var updatedPost = BlogStorage.GetPostById(post.Id);
            Assert.Single(updatedPost.Comments);
            Assert.Equal("", updatedPost.Comments.First().Text);
        }

        [Fact]
        public void AddLike_ShouldBeCaseInsensitive_Usernames()
        {
            var post = BlogStorage.AddPost("user1", "Title", "Content");

            BlogStorage.AddLike(post.Id, "Bob");
            BlogStorage.AddLike(post.Id, "bob");

            Assert.Equal(2, post.Likes.Count);
        }

        [Fact]
        public void AddPost_ShouldHandleVeryLongContent()
        {
            var longContent = new string('x', 10000);
            var post = BlogStorage.AddPost("author", "Title", longContent);

            Assert.Equal(10000, post.Content.Length);
        }

        [Fact]
        public void UpdatePost_ShouldFailGracefully_WhenPostDoesNotExist()
        {
            var result = BlogStorage.UpdatePost(999, "NewTitle", "NewContent");

            Assert.Null(result);
        }

        [Fact]
        public void Summarize_ShouldHandlePostsWithNoLikesOrComments()
        {
            var posts = new List<Post>
            {
                new Post { Author = "user1", Title = "EmptyPost", Likes = new List<Like>(), Comments = new List<Comment>() }
            };

            var summary = PostStatsUtils.Summarize(posts);

            Assert.Contains("всього лайків: 0", summary);
            Assert.Contains("всього коментарів: 0", summary);
        }
    }
}
