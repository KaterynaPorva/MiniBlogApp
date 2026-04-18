using MiniBlogApp.Models;
using MiniBlogApp.Strategies;
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

        // Метод приймає готовий об'єкт Post (наслідок використання Builder)
        Post AddPost(Post post);

        Post? UpdatePost(int id, string title, string content);
        void DeletePost(int id);
        IEnumerable<Post> GetAllPosts();
        IEnumerable<Post> GetPostsByUser(string username);
        Post? GetPostById(int id);
        void AddLike(int postId, string username);
        void AddComment(int postId, string author, string text);
        IEnumerable<Post> GetAllPosts(IPostSortStrategy sortStrategy);
        void AddReply(int postId, int parentCommentId, string author, string text);
        void RemoveLike(int postId, string username);
    }
}