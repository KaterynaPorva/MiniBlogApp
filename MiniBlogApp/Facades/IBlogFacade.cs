using MiniBlogApp.Models;
using MiniBlogApp.Observers; 

namespace MiniBlogApp.Facades
{
    /**
     * @interface IBlogFacade
     * @brief Контракт для Фасаду, що об'єднує всі патерни.
     */
    public interface IBlogFacade
    {
        // Базовий функціонал (Facade + Adapter)
        Post? GetPostForView(int id);

        // Робота з лайками (Command Pattern)
        void AddLike(int postId, string username);
        void UndoLastAction(); // Метод для відкату лайка

        // Робота з коментарями (Composite Pattern)
        void AddComment(int postId, string username, string text, int? parentCommentId = null);

        // Керування подіями (Observer Pattern)
        void Subscribe(IBlogObserver observer);
    }
}