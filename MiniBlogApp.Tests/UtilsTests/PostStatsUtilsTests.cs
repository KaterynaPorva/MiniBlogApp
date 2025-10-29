using System.Collections.Generic;
using MiniBlogApp.Models;
using MiniBlogApp.Services;
using Xunit;

namespace MiniBlogApp.Tests.UtilsTests
{
    public class PostStatsUtilsTests
    {
        private const string Author1 = "author1";
        private const string Author2 = "author2";
        private const string UnknownAuthor = "unknown";

        private const string User1 = "user1";
        private const string User2 = "user2";
        private const string User3 = "user3";
        private const string User4 = "user4";
        private const string User5 = "user5";

        private const string Comment1Text = "Comment 1";
        private const string Comment2Text = "Comment 2";
        private const string Comment3Text = "Comment 3";

        private const string Post1Title = "Post 1";
        private const string Post2Title = "Post 2";
        private const string Post3Title = "Post 3";

        private List<Post> GetSamplePosts()
        {
            var post1 = new Post
            {
                Id = 1,
                Author = Author1,
                Title = Post1Title,
                Likes = { new Like { Username = User1 } },
                Comments = {
                    new Comment { Author = User2, Text = Comment1Text },
                    new Comment { Author = User3, Text = Comment2Text }
                }
            };

            var post2 = new Post
            {
                Id = 2,
                Author = Author2,
                Title = Post2Title,
                Likes = { new Like { Username = User4 }, new Like { Username = User5 } },
                Comments = { new Comment { Author = User1, Text = Comment3Text } }
            };

            var post3 = new Post
            {
                Id = 3,
                Author = Author1,
                Title = Post3Title,
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

            Assert.Contains("3", result); 
            Assert.Contains("3", result); 
            Assert.Contains("3", result); 
        }

        [Fact]
        public void Summarize_ByAuthor_ShouldReturnCorrectTotals()
        {
            var posts = GetSamplePosts();
            var result = PostStatsUtils.Summarize(posts, Author1);

            Assert.Contains("author1", result);
            Assert.Contains("2", result);       
            Assert.Contains("1", result);       
            Assert.Contains("2", result);       
        }

        [Fact]
        public void Summarize_ShouldHandleEmptyList()
        {
            var posts = new List<Post>();
            var result = PostStatsUtils.Summarize(posts);

            Assert.Contains("0", result); 
            Assert.Contains("0", result); 
            Assert.Contains("0", result); 
        }

        [Fact]
        public void Summarize_ByAuthor_ShouldHandleNoPostsForAuthor()
        {
            var posts = GetSamplePosts();
            var result = PostStatsUtils.Summarize(posts, UnknownAuthor);

            Assert.Contains("unknown", result);
            Assert.Contains("0", result);       
            Assert.Contains("0", result);       
            Assert.Contains("0", result);      
        }
    }
}
