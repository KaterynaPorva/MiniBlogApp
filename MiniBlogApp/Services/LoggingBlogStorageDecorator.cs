using MiniBlogApp.Models;
using MiniBlogApp.Strategies;
using System.Collections.Generic;

namespace MiniBlogApp.Services
{
    /**
     * @file LoggingBlogStorageDecorator.cs
     * @brief Реалізація патерну Decorator для IBlogStorage.
     * @details Додає функціонал логування до базового сховища без зміни його коду.
     */
    public class LoggingBlogStorageDecorator : IBlogStorage
    {
        private readonly IBlogStorage _innerStorage;
        private readonly IActivityLogger _logger;

        // Приймаємо базове сховище та логер через конструктор
        public LoggingBlogStorageDecorator(IBlogStorage innerStorage, IActivityLogger logger)
        {
            _innerStorage = innerStorage;
            _logger = logger;
        }

        public List<Post> Posts => _innerStorage.Posts;

        // --- МЕТОДИ З ДОДАНИМ ЛОГУВАННЯМ ---

        public Post AddPost(Post post)
        {
            // 1. Спочатку викликаємо метод оригінального сховища
            var createdPost = _innerStorage.AddPost(post);

            // 2. Додаємо поведінку декоратора (логування)
            string authorName = post.Author?.ToString() ?? "Unknown";
            _logger.AddLog(new PostLogger(authorName, post.Title));

            return createdPost;
        }

        public void AddLike(int postId, string username)
        {
            _innerStorage.AddLike(postId, username);

            var post = _innerStorage.GetPostById(postId);
            if (post != null)
            {
                _logger.AddLog(new LikeLogger(username, post.Title));
            }
        }

        public void AddComment(int postId, string author, string text)
        {
            _innerStorage.AddComment(postId, author, text);
            _logger.AddLog(new CommentLogger(author, text));
        }

        // --- МЕТОДИ БЕЗ ЗМІН (ДЕЛЕГУЄМО ВИКЛИК) ---

        public Post? UpdatePost(int id, string title, string content) => _innerStorage.UpdatePost(id, title, content);
        public void DeletePost(int id) => _innerStorage.DeletePost(id);
        public IEnumerable<Post> GetAllPosts() => _innerStorage.GetAllPosts();
        public IEnumerable<Post> GetPostsByUser(string username) => _innerStorage.GetPostsByUser(username);
        public Post? GetPostById(int id) => _innerStorage.GetPostById(id);
        public IEnumerable<Post> GetAllPosts(IPostSortStrategy sortStrategy) => _innerStorage.GetAllPosts(sortStrategy);
    }
}