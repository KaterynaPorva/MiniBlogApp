using System.Collections.Generic;
using System.Linq;
using Xunit;
using MiniBlogApp.Models;
using MiniBlogApp.Strategies;

namespace MiniBlogApp.Tests.StrategiesTests
{
    public class PostAnalyticsStrategyTests
    {
        private readonly List<Post> _testPosts;

        // В xUnit замість [SetUp] використовується конструктор
        public PostAnalyticsStrategyTests()
        {
            // Генеруємо 10 000 постів з різною кількістю лайків та коментарів
            _testPosts = Enumerable.Range(1, 10000).Select(i => new Post
            {
                Content = new string('A', i % 100), // Різна довжина контенту
                Likes = Enumerable.Range(0, i % 10).Select(_ => new Like()).ToList(),
                Comments = Enumerable.Range(0, i % 5).Select(_ => new Comment()).ToList()
            }).ToList();
        }

        [Fact] // В xUnit замість [Test] використовується [Fact]
        public void Strategies_ShouldReturnIdenticalResults()
        {
            // Arrange
            IPostAnalyticsStrategy sequential = new SequentialPostAnalyticsStrategy();
            IPostAnalyticsStrategy parallel = new ParallelPostAnalyticsStrategy();

            // Act
            var seqLikes = sequential.CalculateTotalLikes(_testPosts);
            var parLikes = parallel.CalculateTotalLikes(_testPosts);

            var seqComments = sequential.CalculateTotalComments(_testPosts);
            var parComments = parallel.CalculateTotalComments(_testPosts);

            var seqLength = sequential.CalculateTotalContentLength(_testPosts);
            var parLength = parallel.CalculateTotalContentLength(_testPosts);

            // Assert (В xUnit використовується Assert.Equal)
            Assert.Equal(seqLikes, parLikes);
            Assert.Equal(seqComments, parComments);
            Assert.Equal(seqLength, parLength);
        }
    }
}