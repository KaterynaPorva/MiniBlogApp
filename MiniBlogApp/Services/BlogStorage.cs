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
     * @details Очищено від логування завдяки патерну Decorator. 
     * Тепер клас займається лише бізнес-логікою зберігання даних.
     */
    public class BlogStorage : IBlogStorage
    {
        private readonly object _lock = new object();
        private int _nextId = 1;
        public List<Post> Posts { get; } = new();

        public BlogStorage()
        {
        }

        public Post AddPost(Post post)
        {
            lock (_lock)
            {
                // Призначаємо унікальний ідентифікатор
                post.Id = _nextId++;

                // Якщо будівельник не встановив дату
                if (post.CreatedAt == default)
                {
                    post.CreatedAt = DateTime.Now;
                }

                Posts.Add(post);


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