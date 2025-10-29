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
        public BlogStorageAddPostTests()
        {
            BlogStorage.Posts.Clear();
            LoggerService.ClearAll();
        }

        [Fact]
        public void AddPost_ShouldAddPostCorrectly()
        {
            var initialCount = BlogStorage.GetAllPosts().Count();
            var author = "testuser";
            var title = "Тестовий пост";
            var content = "Це контент для тесту.";

            var post = BlogStorage.AddPost(author, title, content);
            var allPosts = BlogStorage.GetAllPosts().ToList();

            Assert.Equal(initialCount + 1, allPosts.Count);
            Assert.Contains(allPosts, p =>
                p.Id == post.Id &&
                p.Author == author &&
                p.Title == title &&
                p.Content == content);
        }
    }
}
