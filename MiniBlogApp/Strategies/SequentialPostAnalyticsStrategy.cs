using System.Collections.Generic;
using System.Linq;
using MiniBlogApp.Models;

namespace MiniBlogApp.Strategies
{
    /// <summary>
    /// Послідовна стратегія обчислення аналітики.
    /// </summary>
    public class SequentialPostAnalyticsStrategy : IPostAnalyticsStrategy
    {
        public int CalculateTotalLikes(IEnumerable<Post> posts)
        {
            return posts.Sum(p => p.Likes?.Count ?? 0);
        }

        public int CalculateTotalComments(IEnumerable<Post> posts)
        {
            return posts.Sum(p => p.Comments?.Count ?? 0);
        }

        public int CalculateTotalContentLength(IEnumerable<Post> posts)
        {
            return posts.Sum(p => p.Content?.Length ?? 0);
        }
    }
}