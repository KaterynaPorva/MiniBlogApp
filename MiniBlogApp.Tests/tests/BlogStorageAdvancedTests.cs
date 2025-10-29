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
        public void AddComment_ShouldAddToPostAndLogMessage()
        {
            var post = BlogStorage.AddPost("u1", "Title", "Content");

            BlogStorage.AddComment(post.Id, "user1", "Nice post!");

            var updatedPost = BlogStorage.GetPostById(post.Id);
            Assert.Single(updatedPost.Comments);
            Assert.Equal("user1", updatedPost.Comments.First().Author);
            Assert.Equal("Nice post!", updatedPost.Comments.First().Text);

            var logs = LoggerService.GetLogs().Select(l => l.GetMessage()).ToList();
            Assert.Contains(logs, msg => msg.Contains("залишив коментар"));
        }

        [Fact]
        public void AddLike_ShouldLogMessageOnlyOncePerUser()
        {
            var post = BlogStorage.AddPost("u1", "T", "Content");

            BlogStorage.AddLike(post.Id, "bob");
            BlogStorage.AddLike(post.Id, "bob");

            var logs = LoggerService.GetLogs().Select(l => l.GetMessage()).ToList();
            var likeMessages = logs.Where(msg => msg.Contains("bob") && msg.Contains("лайк")).ToList();
            Assert.Single(likeMessages);

            Assert.Contains(logs, msg => msg.Contains("створив пост"));
        }

        [Fact]
        public void AddPost_ShouldAssignUniqueIdsAndLogMessages()
        {
            var post1 = BlogStorage.AddPost("alice", "P1", "C1");
            var post2 = BlogStorage.AddPost("bob", "P2", "C2");

            Assert.NotEqual(post1.Id, post2.Id);

            var logs = LoggerService.GetLogs().Select(l => l.GetMessage()).ToList();
            Assert.Contains(logs, msg => msg.Contains("P1"));
            Assert.Contains(logs, msg => msg.Contains("P2"));
        }

        [Fact]
        public void UpdatePost_ShouldChangeContent()
        {
            var post = BlogStorage.AddPost("u1", "Title", "OldContent");

            BlogStorage.UpdatePost(post.Id, "Title", "NewContent");

            var updatedPost = BlogStorage.GetPostById(post.Id);
            Assert.NotNull(updatedPost);
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
