using MiniBlogApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniBlogApp.Services
{
    /**
     * @file BlogStorage.cs
     * @class BlogStorage
     * @brief In-memory storage for blog posts and related operations.
     * @details
     * This class provides thread-safe in-memory storage for all blog posts, 
     * manages unique IDs for new posts, and handles creation, updating, 
     * deletion, retrieval, adding likes, and comments. 
     * It implements IBlogStorage for Dependency Injection.
     */
    public class BlogStorage : IBlogStorage
    {
        /**
         * @brief Об'єкт для блокування потоків (забезпечує Thread-Safety).
         */
        private readonly object _lock = new object();

        /**
         * @brief Next ID to assign to a new post.
         */
        private int _nextId = 1;

        /**
         * @brief List of all posts in the blog.
         */
        public List<Post> Posts { get; } = new();


        private readonly IActivityLogger _logger;

        public BlogStorage(IActivityLogger logger)
        {
            _logger = logger;
        }

        /**
         * @brief Adds a new post to the storage safely.
         */
        public Post AddPost(string author, string title, string content)
        {
            lock (_lock) 
            {
                var post = new Post
                {
                    Id = _nextId++,
                    Author = author,
                    Title = title,
                    Content = content,
                    CreatedAt = DateTime.Now
                };
                Posts.Add(post);

                // 3. ВИКОРИСТОВУЄМО ІНЖЕКТОВАНИЙ ЛОГЕР ЗАМІСТЬ СТАТИЧНОГО КЛАСУ
                _logger.AddLog(new PostLogger(author, title));

                return post;
            }
        }

        /**
         * @brief Updates an existing post by ID.
         */
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

        /**
         * @brief Deletes a post by ID.
         */
        public void DeletePost(int id)
        {
            lock (_lock)
            {
                var post = GetPostByIdInternal(id);
                if (post != null)
                    Posts.Remove(post);
            }
        }

        /**
         * @brief Returns all posts sorted by creation date (newest first).
         */
        public IEnumerable<Post> GetAllPosts()
        {
            lock (_lock)
            {
                // Використовуємо .ToList(), щоб створити безпечну копію списку 
                // і уникнути помилок, якщо інший користувач додасть пост під час читання
                return Posts.OrderByDescending(p => p.CreatedAt).ToList();
            }
        }

        /**
         * @brief Returns all posts created by a specific user.
         */
        public IEnumerable<Post> GetPostsByUser(string username)
        {
            lock (_lock)
            {
                return Posts.Where(p => p.Author == username)
                            .OrderByDescending(p => p.CreatedAt)
                            .ToList();
            }
        }

        /**
         * @brief Retrieves a post by its ID.
         */
        public Post? GetPostById(int id)
        {
            lock (_lock)
            {
                return GetPostByIdInternal(id);
            }
        }

        /**
         * @brief Adds a like to a post.
         */
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

        /**
         * @brief Adds a comment to a post.
         */
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

        /**
         * @brief Приватний метод для пошуку всередині класу без повторного блокування _lock
         */
        private Post? GetPostByIdInternal(int id)
        {
            return Posts.FirstOrDefault(p => p.Id == id);
        }
    }
}