using MiniBlogApp.Models;

namespace MiniBlogApp.Facades
{
    /// <summary>
    /// Інтерфейс Фасаду, що спрощує доступ до бізнес-логіки блогу.
    /// </summary>
    public interface IBlogFacade
    {
        Post? GetPostForView(int id); // ? означає, що може повернути null
        void AddLike(int postId, string username);
        void AddComment(int postId, string username, string text);
    }
}