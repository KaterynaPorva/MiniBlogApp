using MiniBlogApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace MiniBlogApp.Strategies
{
    /**
     * @interface IPostSortStrategy
     * @brief Інтерфейс для патерну Strategy. Визначає загальний метод для сортування постів.
     */
    public interface IPostSortStrategy
    {
        IEnumerable<Post> Sort(IEnumerable<Post> posts);
    }

    /**
     * @class DateSortStrategy
     * @brief Конкретна стратегія: сортування постів за датою створення (найновіші перші).
     */
    public class DateSortStrategy : IPostSortStrategy
    {
        public IEnumerable<Post> Sort(IEnumerable<Post> posts)
        {
            return posts.OrderByDescending(p => p.CreatedAt);
        }
    }

    /**
     * @class PopularitySortStrategy
     * @brief Конкретна стратегія: сортування постів за кількістю лайків (найпопулярніші перші).
     */
    public class PopularitySortStrategy : IPostSortStrategy
    {
        public IEnumerable<Post> Sort(IEnumerable<Post> posts)
        {
            return posts.OrderByDescending(p => p.Likes.Count);
        }
    }
}