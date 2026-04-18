using MiniBlogApp.Models;
using MiniBlogApp.Strategies;
using MiniBlogApp.Factories; 
using System.Collections.Generic;

namespace MiniBlogApp.Services
{
    /**
     * @file LoggingBlogStorageDecorator.cs
     * @brief Реалізація патерну Decorator для IBlogStorage.
     * @details Додає функціонал логування до базового сховища. 
     * Для створення об'єктів логів використовується патерн Factory Method, 
     * що дозволяє уникнути жорсткої залежності від конкретних класів логерів.
     */
    public class LoggingBlogStorageDecorator : IBlogStorage
    {
        private readonly IBlogStorage _innerStorage;
        private readonly IActivityLogger _logger;

        /**
         * @brief Конструктор декоратора.
         * @param innerStorage Об'єкт сховища, який ми обгортаємо.
         * @param logger Сервіс логування активності.
         */
        public LoggingBlogStorageDecorator(IBlogStorage innerStorage, IActivityLogger logger)
        {
            _innerStorage = innerStorage;
            _logger = logger;
        }

        public List<Post> Posts => _innerStorage.Posts;

        // --- МЕТОДИ З ДОДАНИМ ЛОГУВАННЯМ ---

        /**
         * @brief Додає пост та створює запис у лог через PostLogFactory.
         * @details Використання Factory Method дозволяє інкапсулювати логіку створення PostLogger.
         */
        public Post AddPost(Post post)
        {
            var createdPost = _innerStorage.AddPost(post);
            string authorName = post.Author?.ToString() ?? "Unknown";

            // ПАТЕРН FACTORY METHOD
            LogFactory factory = new PostLogFactory();
            _logger.AddLog(factory.CreateLog(authorName, post.Title));

            return createdPost;
        }

        /**
         * @brief Додає лайк та створює запис через LikeLogFactory.
         */
        public void AddLike(int postId, string username)
        {
            _innerStorage.AddLike(postId, username);
            var post = _innerStorage.GetPostById(postId);
            if (post != null)
            {
                // ПАТЕРН FACTORY METHOD
                LogFactory factory = new LikeLogFactory();
                _logger.AddLog(factory.CreateLog(username, post.Title));
            }
        }

        /**
         * @brief ПАТЕРН DECORATOR + COMMAND: Логуємо видалення лайку (Undo).
         * @details Для створення логу використовується та сама фабрика LikeLogFactory.
         */
        public void RemoveLike(int postId, string username)
        {
            // 1. Виконуємо дію в основному сховищі
            _innerStorage.RemoveLike(postId, username);

            // 2. Додаємо запис у лог про скасування лайку
            var post = _innerStorage.GetPostById(postId);
            if (post != null)
            {
                // ПАТЕРН FACTORY METHOD
                LogFactory factory = new LikeLogFactory();
                _logger.AddLog(factory.CreateLog(username, $"[UNDONE] {post.Title}"));
            }
        }

        /**
         * @brief Додає коментар та створює лог через CommentLogFactory.
         */
        public void AddComment(int postId, string author, string text)
        {
            _innerStorage.AddComment(postId, author, text);

            // ПАТЕРН FACTORY METHOD
            LogFactory factory = new CommentLogFactory();
            _logger.AddLog(factory.CreateLog(author, text));
        }

        /**
         * @brief ПАТЕРН DECORATOR: Додаємо логування для методу відповідей.
         * @details Оскільки IBlogStorage отримав новий метод для Composite, 
         * Декоратор обов'язково має його реалізувати.
         */
        public void AddReply(int postId, int parentCommentId, string author, string text)
        {
            // Передаємо виклик основному сховищу
            _innerStorage.AddReply(postId, parentCommentId, author, text);

            // Логуємо дію через фабрику коментарів
            LogFactory factory = new CommentLogFactory();
            _logger.AddLog(factory.CreateLog(author, $"[Відповідь]: {text}"));
        }

        // --- МЕТОДИ БЕЗ ЗМІН (ДЕЛЕГУЄМО ВИКЛИК) ---

        /** @brief Делегування оновлення поста без додаткової логіки. */
        public Post? UpdatePost(int id, string title, string content) => _innerStorage.UpdatePost(id, title, content);

        /** @brief Делегування видалення поста. */
        public void DeletePost(int id) => _innerStorage.DeletePost(id);

        /** @brief Отримання списку всіх постів. */
        public IEnumerable<Post> GetAllPosts() => _innerStorage.GetAllPosts();

        /** @brief Фільтрація постів за автором. */
        public IEnumerable<Post> GetPostsByUser(string username) => _innerStorage.GetPostsByUser(username);

        /** @brief Пошук поста за ідентифікатором. */
        public Post? GetPostById(int id) => _innerStorage.GetPostById(id);

        /** @brief Отримання відсортованих постів (Pattern Strategy). */
        public IEnumerable<Post> GetAllPosts(IPostSortStrategy sortStrategy) => _innerStorage.GetAllPosts(sortStrategy);
    }
}