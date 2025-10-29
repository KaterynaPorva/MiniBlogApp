using MiniBlogApp.Services;
using Xunit;
using System.Linq;
using System.Collections.Generic;
using MiniBlogApp.Models;

namespace MiniBlogApp.Tests.ServiceTests
{
    [Collection("BlogStorageTests")]
    public class BlogStorageAdvancedTests
    {
        public BlogStorageAdvancedTests()
        {
            BlogStorage.Posts.Clear();
            LoggerService.ClearAll();
        }

        [Fact]
        public void AddComment_ShouldAddToPostAndLog()
        {
            var post = BlogStorage.AddPost("u1", "Title", "Content");

            BlogStorage.AddComment(post.Id, "user1", "Nice post!");

            var updatedPost = BlogStorage.GetPostById(post.Id);
            Assert.Single(updatedPost.Comments);
            Assert.Equal("user1", updatedPost.Comments.First().Author);
            Assert.Equal("Nice post!", updatedPost.Comments.First().Text);

            var logs = LoggerService.GetLogs().ToList();
            Assert.Contains(logs, l => l is CommentLogger && l.GetMessage().Contains("залишив коментар"));
        }

        [Fact]
        public void AddLike_ShouldAddLogOnlyOncePerUser()
        {
            var post = BlogStorage.AddPost("u1", "T", "Content");

            BlogStorage.AddLike(post.Id, "bob");
            BlogStorage.AddLike(post.Id, "bob");

            var logs = LoggerService.GetLogs().ToList();
            Assert.Single(logs.OfType<LikeLogger>().Where(l => l.Username == "bob"));
            Assert.Contains(logs, l => l.GetMessage().Contains("створив пост"));
        }

        [Fact]
        public void AddPost_ShouldAssignUniqueIdsAndLog()
        {
            var post1 = BlogStorage.AddPost("alice", "P1", "C1");
            var post2 = BlogStorage.AddPost("bob", "P2", "C2");

            Assert.NotEqual(post1.Id, post2.Id);

            var logs = LoggerService.GetLogs().ToList();
            Assert.Equal(2, logs.OfType<PostLogger>().Count());
            Assert.Contains(logs, l => l.GetMessage().Contains("P1"));
            Assert.Contains(logs, l => l.GetMessage().Contains("P2"));
        }

        [Fact]
        public void UpdatePost_ShouldChangeContent()
        {
            var post = BlogStorage.AddPost("u1", "Title", "OldContent");

            BlogStorage.UpdatePost(post.Id, "Title", "NewContent");

            var updatedPost = BlogStorage.GetPostById(post.Id);
            Assert.Equal("NewContent", updatedPost.Content);
        }

        [Fact]
        public void DeletePost_ShouldRemovePost()
        {
            var post = BlogStorage.AddPost("u1", "Title", "Content");

            BlogStorage.DeletePost(post.Id);

            Assert.Null(BlogStorage.GetPostById(post.Id));
        }
    }
}
