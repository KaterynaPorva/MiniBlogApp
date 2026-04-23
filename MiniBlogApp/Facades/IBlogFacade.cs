using MiniBlogApp.Models;
using MiniBlogApp.Observers;
using MiniBlogApp.Strategies; 
using System.Collections.Generic;

namespace MiniBlogApp.Facades
{
    /**
     * @interface IBlogFacade
     * @brief Контракт для Фасаду, що об'єднує всі патерни.
     */
    public interface IBlogFacade
    {
        // Оновлений метод: приймає і стратегію сортування, і рядок пошуку
        IEnumerable<Post> GetAllPosts(IPostSortStrategy strategy, string? searchQuery = null);

        Post? GetPostForView(int id);
        void AddLike(int postId, string username);
        void UndoLastAction();
        void AddComment(int postId, string username, string text, int? parentCommentId = null);
        void Subscribe(IBlogObserver observer);
    }
}