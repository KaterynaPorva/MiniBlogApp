using System.Collections.Generic;
using System.Linq;
using MiniBlogApp.Models;

namespace MiniBlogApp.Strategies
{
    /// <summary>
    /// Паралельна мультипоточна стратегія обчислення аналітики за допомогою PLINQ.
    /// </summary>
    public class ParallelPostAnalyticsStrategy : IPostAnalyticsStrategy
    {
        public int CalculateTotalLikes(IEnumerable<Post> posts)
        {
            return posts.AsParallel().Sum(p => p.Likes?.Count ?? 0);
        }

        public int CalculateTotalComments(IEnumerable<Post> posts)
        {
            return posts.AsParallel().Sum(p => p.Comments?.Count ?? 0);
        }

        public int CalculateTotalContentLength(IEnumerable<Post> posts)
        {
            return posts.AsParallel().Sum(p => p.Content?.Length ?? 0);
        }
    }
}