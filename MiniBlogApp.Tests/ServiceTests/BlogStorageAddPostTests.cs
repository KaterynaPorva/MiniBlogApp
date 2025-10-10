using System.Linq;
using MiniBlogApp.Models;
using MiniBlogApp.Services;
using Xunit;

namespace MiniBlogApp.Tests.ServiceTests
{
    public class BlogStorageAddPostTests 
    {
        public BlogStorageAddPostTests()
        {
            BlogStorage.Posts.Clear();
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
