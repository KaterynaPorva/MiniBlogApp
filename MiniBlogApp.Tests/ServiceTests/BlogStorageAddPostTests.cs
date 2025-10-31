using System.Linq;
using MiniBlogApp.Services;
using Xunit;
using MiniBlogApp.Models;
using System.Collections.Generic;

namespace MiniBlogApp.Tests.ServiceTests
{
    [Collection("BlogStorageTests")]
    public class BlogStorageAddPostTests
    {
        private const string Author = "testuser";
        private const string Title = "Тестовий пост";
        private const string Content = "Це контент для тесту.";

        public BlogStorageAddPostTests()
        {
            BlogStorage.Posts.Clear();
            LoggerService.ClearAll();
        }

        [Fact]
        public void AddPost_ShouldAddPostCorrectly()
        {
            var initialCount = BlogStorage.GetAllPosts().Count();

            var post = BlogStorage.AddPost(Author, Title, Content);
            var allPosts = BlogStorage.GetAllPosts().ToList();

            Assert.Equal(initialCount + 1, allPosts.Count);
            Assert.Contains(allPosts, p =>
                p.Id == post.Id &&
                p.Author == Author &&
                p.Title == Title &&
                p.Content == Content);
        }
    }
}
