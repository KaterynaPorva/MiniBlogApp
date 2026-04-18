using System.Collections.Generic;
using System.Linq;
using MiniBlogApp.Models;
using MiniBlogApp.Services;
using MiniBlogApp.Strategies;

namespace MiniBlogApp.Proxies
{
    /**
     * @class CachedBlogStorageProxy
     * @brief ПАТЕРН PROXY (Замісник - Кешуючий).
     * @details Контролює доступ до основного сховища (IBlogStorage). 
     * Зберігає результати запитів у пам'яті (кеші), щоб зменшити навантаження 
     * на "базу даних" при повторних запитах.
     */
    public class CachedBlogStorageProxy : IBlogStorage
    {
        private readonly IBlogStorage _realStorage;

        // Наш кеш: словник, де ключ - це ID поста, а значення - сам пост
        private readonly Dictionary<int, Post> _postCache = new();

        public CachedBlogStorageProxy(IBlogStorage realStorage)
        {
            _realStorage = realStorage;
        }

        public List<Post> Posts => _realStorage.Posts;

        // --- ЛОГІКА PROXY: КЕШУВАННЯ ---

        public Post? GetPostById(int id)
        {
            // 1. Спочатку перевіряємо, чи є пост у нашому швидкому кеші
            if (_postCache.ContainsKey(id))
            {
                // Повертаємо з кешу (швидко!)
                return _postCache[id];
            }

            // 2. Якщо в кеші немає, йдемо до реального сховища (повільно)
            var post = _realStorage.GetPostById(id);

            // 3. Зберігаємо результат у кеш для наступних разів
            if (post != null)
            {
                _postCache[id] = post;
            }

            return post;
        }

        // --- ЛОГІКА PROXY: ІНВАЛІДАЦІЯ КЕШУ ---
        // Якщо хтось змінює дані (видаляє, додає), старий кеш стає неактуальним.
        // Тому перед викликом оригінальних методів ми "чистимо" кеш.

        public Post AddPost(Post post)
        {
            _postCache.Clear(); // Дані змінилися, чистимо кеш
            return _realStorage.AddPost(post);
        }

        public Post? UpdatePost(int id, string title, string content)
        {
            _postCache.Clear(); // Дані змінилися, чистимо кеш
            return _realStorage.UpdatePost(id, title, content);
        }

        public void DeletePost(int id)
        {
            _postCache.Clear(); // Дані змінилися, чистимо кеш
            _realStorage.DeletePost(id);
        }

        // --- МЕТОДИ БЕЗ КЕШУВАННЯ (Просте делегування) ---
        // Ці методи ми просто передаємо далі реальному сховищу.

        public void AddLike(int postId, string username) => _realStorage.AddLike(postId, username);
        public void RemoveLike(int postId, string username) => _realStorage.RemoveLike(postId, username);
        public void AddComment(int postId, string author, string text) => _realStorage.AddComment(postId, author, text);
        public void AddReply(int postId, int parentCommentId, string author, string text) => _realStorage.AddReply(postId, parentCommentId, author, text);
        public IEnumerable<Post> GetAllPosts() => _realStorage.GetAllPosts();
        public IEnumerable<Post> GetPostsByUser(string username) => _realStorage.GetPostsByUser(username);
        public IEnumerable<Post> GetAllPosts(IPostSortStrategy sortStrategy) => _realStorage.GetAllPosts(sortStrategy);
    }
}