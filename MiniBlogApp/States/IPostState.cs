using MiniBlogApp.Models;

namespace MiniBlogApp.States
{
    /**
     * @interface IPostState
     * @brief Патерн State: Загальний інтерфейс для всіх станів поста.
     */
    public interface IPostState
    {
        void SubmitForReview(Post post); // Відправити на перевірку
        void Approve(Post post);         // Опублікувати
        void Reject(Post post);          // Відхилити (повернути в чернетки)
        string GetStatusName();          // Отримати назву статусу для UI
    }
}