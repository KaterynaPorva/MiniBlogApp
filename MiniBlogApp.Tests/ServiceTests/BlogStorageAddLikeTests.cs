using System.Linq;
using MiniBlogApp.Services;
using Xunit;
using MiniBlogApp.Models;

namespace MiniBlogApp.Tests.ServiceTests
{
    [Collection("BlogStorageTests")]
    public class BlogStorageAddLikeTests
    {
        private const string Author = "author1";
        private const string Title = "title1";
        private const string Content = "content1";
        private const string User = "user1";
        private const int NonExistentPostId = 999;

        public BlogStorageAddLikeTests()
        {
            BlogStorage.Posts.Clear();
            LoggerService.ClearAll();
        }

        [Fact]
        public void AddLike_ShouldAddLike_WhenUserHasNotLikedBefore()
        {
            var post = BlogStorage.AddPost(Author, Title, Content);

            BlogStorage.AddLike(post.Id, User);

            Assert.Single(post.Likes);
            Assert.Equal(User, post.Likes.First().Username);
        }

        [Fact]
        public void AddLike_ShouldNotAddDuplicateLikes()
        {
            var post = BlogStorage.AddPost(Author, Title, Content);
            BlogStorage.AddLike(post.Id, User);

            BlogStorage.AddLike(post.Id, User);

            Assert.Single(post.Likes);
            Assert.Equal(User, post.Likes.First().Username);
        }

        [Fact]
        public void AddLike_ShouldDoNothing_WhenPostDoesNotExist()
        {
            BlogStorage.AddLike(NonExistentPostId, User);

            Assert.Empty(BlogStorage.Posts);
        }
    }
}
