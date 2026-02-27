using MiniBlogApp.Models;
using System.Collections.Generic;

namespace MiniBlogApp.Services
{
    /**
     * @brief Інтерфейс для роботи зі сховищем дописів.
     * @details Дозволяє використовувати Dependency Injection та писати Unit-тести.
     */
    public interface IBlogStorage
    {
        List<Post> Posts { get; }

        Post AddPost(string author, string title, string content);
        Post? UpdatePost(int id, string title, string content);
        void DeletePost(int id);
        IEnumerable<Post> GetAllPosts();
        IEnumerable<Post> GetPostsByUser(string username);
        Post? GetPostById(int id);
        void AddLike(int postId, string username);
        void AddComment(int postId, string author, string text);
    }
}