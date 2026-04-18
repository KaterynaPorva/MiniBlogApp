using MiniBlogApp.Models;
using MiniBlogApp.Strategies;
using MiniBlogApp.Composites; // Не забудь додати цей using для інтерфейсу компонувальника
using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniBlogApp.Services
{
    /**
     * @file BlogStorage.cs
     * @class BlogStorage
     * @brief In-memory storage for blog posts and related operations.
     * @details Оновлено для підтримки патерну Composite (вкладені коментарі).
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
                post.Id = _nextId++;
                if (post.CreatedAt == default)
                {
                    post.CreatedAt = DateTime.Now;
                }
                Posts.Add(post);
                return post;
            }
        }

        // --- МЕТОДИ ДЛЯ КОМЕНТАРІВ (ПАТЕРН COMPOSITE) ---

        public void AddComment(int postId, string author, string text)
        {
            lock (_lock)
            {
                var post = GetPostByIdInternal(postId);
                if (post == null) return;

                // Присвоюємо Id коментарю, щоб на нього можна було відповісти
                post.Comments.Add(new Comment
                {
                    Id = _nextId++,
                    Author = author,
                    Text = text
                });
            }
        }

        public void AddReply(int postId, int parentCommentId, string author, string text)
        {
            lock (_lock)
            {
                var post = GetPostByIdInternal(postId);
                if (post == null) return;

                // Шукаємо батьківський коментар рекурсивно по всьому дереву
                var parentComment = FindCommentRecursive(post.Comments, parentCommentId);

                if (parentComment != null)
                {
                    parentComment.AddReply(new Comment
                    {
                        Id = _nextId++,
                        Author = author,
                        Text = text
                    });
                }
            }
        }

        private Comment? FindCommentRecursive(IEnumerable<ICommentComponent> components, int id)
        {
            foreach (var component in components)
            {
                if (component is Comment comment)
                {
                    // Якщо знайшли — повертаємо
                    if (comment.Id == id) return comment;

                    // Якщо ні — йдемо вглиб у відповіді цього коментаря
                    var found = FindCommentRecursive(comment.Replies, id);
                    if (found != null) return found;
                }
            }
            return null;
        }

        // --- ІНШІ МЕТОДИ ---

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
        public void RemoveLike(int postId, string username)
        {
            lock (_lock)
            {
                var post = GetPostByIdInternal(postId);
                if (post == null) return;

                var like = post.Likes.FirstOrDefault(l => l.Username == username);
                if (like != null)
                {
                    post.Likes.Remove(like);
                }
            }
        }
    }
}