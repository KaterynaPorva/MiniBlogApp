using System.Linq;
using MiniBlogApp.Models;
using MiniBlogApp.Services;
using Xunit;
using System.Collections.Generic;

namespace MiniBlogApp.Tests.ServiceTests
{
    [Collection("BlogStorageTests")]
    public class BlogStorageEdgeCaseTests
    {
        private const int VeryLongContentLength = 10000;
        private const int NonExistentPostId = 999;
        private const string EmptyText = "";
        private const string AuthorUser1 = "user1";
        private const string AuthorUser2 = "user2";
        private const string PostTitle = "Title";
        private const string PostContent = "Content";
        private const string LongContentAuthor = "author";
        private const string NewPostTitle = "NewTitle";
        private const string NewPostContent = "NewContent";
        private const string EmptyPostTitle = "EmptyPost";
        private const string LikesSummary = "всього лайків: 0";
        private const string CommentsSummary = "всього коментарів: 0";

        public BlogStorageEdgeCaseTests()
        {
            BlogStorage.Posts.Clear();
            LoggerService.ClearAll();
        }

        [Fact]
        public void AddComment_ShouldHandleEmptyText()
        {
            var post = BlogStorage.AddPost(AuthorUser1, PostTitle, PostContent);

            BlogStorage.AddComment(post.Id, AuthorUser2, EmptyText);

            var updatedPost = BlogStorage.GetPostById(post.Id);
            Assert.Single(updatedPost.Comments);
            Assert.Equal(EmptyText, updatedPost.Comments.First().Text);
        }

        [Fact]
        public void AddLike_ShouldBeCaseSensitive_Usernames()
        {
            var post = BlogStorage.AddPost(AuthorUser1, PostTitle, PostContent);

            BlogStorage.AddLike(post.Id, "Bob");
            BlogStorage.AddLike(post.Id, "bob");

            Assert.Equal(2, post.Likes.Count);
        }

        [Fact]
        public void AddPost_ShouldHandleVeryLongContent()
        {
            var longContent = new string('x', VeryLongContentLength);
            var post = BlogStorage.AddPost(LongContentAuthor, PostTitle, longContent);

            Assert.Equal(VeryLongContentLength, post.Content.Length);
        }

        [Fact]
        public void UpdatePost_ShouldFailGracefully_WhenPostDoesNotExist()
        {
            var result = BlogStorage.UpdatePost(NonExistentPostId, NewPostTitle, NewPostContent);

            Assert.Null(result);
        }

        [Fact]
        public void Summarize_ShouldHandlePostsWithNoLikesOrComments()
        {
            var posts = new List<Post>
            {
                new Post { Author = AuthorUser1, Title = EmptyPostTitle, Likes = new List<Like>(), Comments = new List<Comment>() }
            };

            var summary = PostStatsUtils.Summarize(posts);

            Assert.Contains(LikesSummary, summary);
            Assert.Contains(CommentsSummary, summary);
        }
    }
}
