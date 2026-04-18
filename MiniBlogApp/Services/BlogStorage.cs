using MiniBlogApp.Models;
using MiniBlogApp.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniBlogApp.Services
{
    /**
     * @file BlogStorage.cs
     * @class BlogStorage
     * @brief In-memory storage for blog posts and related operations.
     */
    public class BlogStorage : IBlogStorage
    {
        private readonly object _lock = new object();
        private int _nextId = 1;
        public List<Post> Posts { get; } = new();
        private readonly IActivityLogger _logger;

        public BlogStorage(IActivityLogger logger)
        {
            _logger = logger;
        }

        /**
         * @brief Adds a new pre-built post to the storage safely.
         * @details Оновлено для використання патерну Builder. Тепер метод приймає 
         * готовий об'єкт Post замість окремих параметрів.
         */
        public Post AddPost(Post post)
        {
            lock (_lock)
            {
                // Призначаємо унікальний ідентифікатор
                post.Id = _nextId++;

                // Якщо будівельник не встановив дату (на всякий випадок)
                if (post.CreatedAt == default)
                {
                    post.CreatedAt = DateTime.Now;
                }

                Posts.Add(post);

                // Отримуємо ім'я автора для логування (залежить від того, чи Author це string, чи об'єкт BlogUser)
                // Якщо у твоїй моделі Author це string, залишаємо post.Author
                // Якщо об'єкт - використовуємо post.Author.UserName
                string authorName = post.Author?.ToString() ?? "Unknown";

                // Логуємо створення поста
                _logger.AddLog(new PostLogger(authorName, post.Title));

                return post;
            }
        }

        public Post? UpdatePost(int id, string title, string content)
        {
            lock (_lock)
            {
                var post = GetPostByIdInternal(id);
                if (post != null)
                {
                    post.Title = title;
                    post.Content = content;
                    return post;
                }
                return null;
            }
        }

        public void DeletePost(int id)
        {
            lock (_lock)
            {
                var post = GetPostByIdInternal(id);
                if (post != null)
                    Posts.Remove(post);
            }
        }

        public IEnumerable<Post> GetAllPosts()
        {
            lock (_lock)
            {
                return Posts.OrderByDescending(p => p.CreatedAt).ToList();
            }
        }

        public IEnumerable<Post> GetPostsByUser(string username)
        {
            lock (_lock)
            {
                // Тут логіка теж може трохи змінитися, якщо Author тепер об'єкт
                // Якщо Author це string: p.Author == username
                // Якщо BlogUser: p.Author.UserName == username
                return Posts.Where(p => p.Author?.ToString() == username)
                            .OrderByDescending(p => p.CreatedAt)
                            .ToList();
            }
        }

        public Post? GetPostById(int id)
        {
            lock (_lock)
            {
                return GetPostByIdInternal(id);
            }
        }

        public void AddLike(int postId, string username)
        {
            lock (_lock)
            {
                var post = GetPostByIdInternal(postId);
                if (post == null) return;

                if (!post.Likes.Any(l => l.Username == username))
                {
                    post.Likes.Add(new Like { Username = username });
                    _logger.AddLog(new LikeLogger(username, post.Title));
                }
            }
        }

        public void AddComment(int postId, string author, string text)
        {
            lock (_lock)
            {
                var post = GetPostByIdInternal(postId);
                if (post == null) return;

                post.Comments.Add(new Comment { Author = author, Text = text });
                _logger.AddLog(new CommentLogger(author, text));
            }
        }

        private Post? GetPostByIdInternal(int id)
        {
            return Posts.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Post> GetAllPosts(IPostSortStrategy sortStrategy)
        {
            lock (_lock)
            {
                var currentPosts = Posts.ToList();
                return sortStrategy.Sort(currentPosts).ToList();
            }
        }
    }
}