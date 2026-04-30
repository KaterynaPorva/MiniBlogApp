using System.Collections.Generic;
using MiniBlogApp.Models;

namespace MiniBlogApp.Strategies
{
    /// <summary>
    /// Інтерфейс стратегії для аналітики постів блогу.
    /// </summary>
    public interface IPostAnalyticsStrategy
    {
        int CalculateTotalLikes(IEnumerable<Post> posts);
        int CalculateTotalComments(IEnumerable<Post> posts);
        int CalculateTotalContentLength(IEnumerable<Post> posts);
    }
}