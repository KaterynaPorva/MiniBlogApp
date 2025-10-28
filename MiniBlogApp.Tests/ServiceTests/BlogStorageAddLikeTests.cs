using System.Linq;
using MiniBlogApp.Services;
using Xunit;

namespace MiniBlogApp.Tests.ServiceTests
{
    public class BlogStorageAddLikeTests
    {
        public BlogStorageAddLikeTests()
        {
            BlogStorage.Posts.Clear();
        }

        [Fact]
        public void AddLike_ShouldAddLike_WhenUserHasNotLikedBefore()
        {
            var post = BlogStorage.AddPost("author1", "title1", "content1");

            BlogStorage.AddLike(post.Id, "user1");

            Assert.Single(post.Likes);
            Assert.Equal("user1", post.Likes.First().Username);
        }

        [Fact]
        public void AddLike_ShouldNotAddDuplicateLikes()
        {
            var post = BlogStorage.AddPost("author1", "title1", "content1");
            BlogStorage.AddLike(post.Id, "user1");

            BlogStorage.AddLike(post.Id, "user1");

            Assert.Single(post.Likes);
        }

        [Fact]
        public void AddLike_ShouldDoNothing_WhenPostDoesNotExist()
        {
            BlogStorage.AddLike(999, "user1");

            Assert.Empty(BlogStorage.Posts);
        }
    }
}
