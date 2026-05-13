using MiniBlogApp.Models;
using MiniBlogApp.Strategies;
using MiniBlogApp.Composites;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Text.Json;
using System;

namespace MiniBlogApp.Services
{
    public class ConcurrentBlogStorage : IBlogStorage
    {
        private readonly List<Post> _posts = new List<Post>();
        private readonly ReaderWriterLockSlim _rwLock = new ReaderWriterLockSlim();
        private int _nextPostId = 1;
        private int _nextCommentId = 1;

        public List<Post> Posts
        {
            get
            {
                _rwLock.EnterReadLock();
                try { return _posts.ToList(); }
                finally { _rwLock.ExitReadLock(); }
            }
        }

        public Post AddPost(Post post)
        {
            _rwLock.EnterWriteLock();
            try
            {
                post.Id = _nextPostId++;
                post.Likes ??= new List<Like>();
                post.Comments ??= new List<Comment>();
                _posts.Add(post);
                return post;
            }
            finally { _rwLock.ExitWriteLock(); }
        }

        public Post? UpdatePost(int id, string title, string content)
        {
            _rwLock.EnterWriteLock();
            try
            {
                var post = _posts.FirstOrDefault(p => p.Id == id);
                if (post != null)
                {
                    post.Title = title;
                    post.Content = content;
                }
                return post;
            }
            finally { _rwLock.ExitWriteLock(); }
        }

        public void DeletePost(int id)
        {
            _rwLock.EnterWriteLock();
            try
            {
                var post = _posts.FirstOrDefault(p => p.Id == id);
                if (post != null) { _posts.Remove(post); }
            }
            finally { _rwLock.ExitWriteLock(); }
        }

        public IEnumerable<Post> GetAllPosts()
        {
            _rwLock.EnterReadLock();
            try { return _posts.ToList(); }
            finally { _rwLock.ExitReadLock(); }
        }

        public IEnumerable<Post> GetPostsByUser(string username)
        {
            _rwLock.EnterReadLock();
            try { return _posts.Where(p => p.Author == username).ToList(); }
            finally { _rwLock.ExitReadLock(); }
        }

        public Post? GetPostById(int id)
        {
            _rwLock.EnterReadLock();
            try { return _posts.FirstOrDefault(p => p.Id == id); }
            finally { _rwLock.ExitReadLock(); }
        }

        public void AddLike(int postId, string username)
        {
            _rwLock.EnterWriteLock();
            try
            {
                var post = _posts.FirstOrDefault(p => p.Id == postId);
                if (post != null)
                {
                    post.Likes ??= new List<Like>();
                    if (!post.Likes.Any(l => l.Username == username))
                    {
                        post.Likes.Add(new Like { Username = username });
                    }
                }
            }
            finally { _rwLock.ExitWriteLock(); }
        }

        public void RemoveLike(int postId, string username)
        {
            _rwLock.EnterWriteLock();
            try
            {
                var post = _posts.FirstOrDefault(p => p.Id == postId);
                if (post != null && post.Likes != null)
                {
                    var like = post.Likes.FirstOrDefault(l => l.Username == username);
                    if (like != null) { post.Likes.Remove(like); }
                }
            }
            finally { _rwLock.ExitWriteLock(); }
        }

        public void AddComment(int postId, string author, string text)
        {
            _rwLock.EnterWriteLock();
            try
            {
                var post = _posts.FirstOrDefault(p => p.Id == postId);
                if (post != null)
                {
                    post.Comments ??= new List<Comment>();
                    post.Comments.Add(new Comment
                    {
                        Id = _nextCommentId++,
                        Author = author,
                        Text = text,
                        Replies = new List<ICommentComponent>()
                    });
                }
            }
            finally { _rwLock.ExitWriteLock(); }
        }

        public void AddReply(int postId, int parentCommentId, string author, string text)
        {
            _rwLock.EnterWriteLock();
            try
            {
                var post = _posts.FirstOrDefault(p => p.Id == postId);
                if (post != null && post.Comments != null)
                {
                    var parentComment = post.Comments.FirstOrDefault(c => c.Id == parentCommentId);
                    if (parentComment != null)
                    {
                        parentComment.Replies ??= new List<ICommentComponent>();
                        parentComment.Replies.Add(new Comment
                        {
                            Id = _nextCommentId++,
                            Author = author,
                            Text = text,
                            Replies = new List<ICommentComponent>()
                        });
                    }
                }
            }
            finally { _rwLock.ExitWriteLock(); }
        }

        public IEnumerable<Post> GetAllPosts(IPostSortStrategy sortStrategy)
        {
            _rwLock.EnterReadLock();
            List<Post> snapshot;
            try { snapshot = _posts.ToList(); }
            finally { _rwLock.ExitReadLock(); }

            return sortStrategy.Sort(snapshot);
        }

        // ==========================================
        // БЕКАПИ З ВИКОРИСТАННЯМ ПАТЕРНУ DTO
        // ==========================================

        public string ExportDataToJson()
        {
            _rwLock.EnterReadLock();
            try
            {
                // Конвертуємо складні моделі у прості DTO без інтерфейсів
                var exportData = _posts.Select(p => new PostBackupDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Content = p.Content,
                    Author = p.Author,
                    CreatedAt = p.CreatedAt,
                    Likes = p.Likes?.ToList(),
                    Comments = p.Comments?.Select(c => new CommentBackupDto
                    {
                        Id = c.Id,
                        Author = c.Author,
                        Text = c.Text
                    }).ToList()
                }).ToList();

                var options = new JsonSerializerOptions { WriteIndented = true };
                return JsonSerializer.Serialize(exportData, options);
            }
            finally { _rwLock.ExitReadLock(); }
        }

        public void ImportDataFromJson(string jsonContent)
        {
            _rwLock.EnterWriteLock();
            try
            {
                // Читаємо прості DTO і збираємо з них повноцінні об'єкти
                var importedDtos = JsonSerializer.Deserialize<List<PostBackupDto>>(jsonContent);
                if (importedDtos != null)
                {
                    foreach (var dto in importedDtos)
                    {
                        var existingPost = _posts.FirstOrDefault(p => p.Id == dto.Id);

                        if (existingPost == null)
                        {
                            // 1. Поста немає - Відновлюємо
                            var newPost = new Post
                            {
                                Id = dto.Id,
                                Title = dto.Title,
                                Content = dto.Content,
                                Author = dto.Author,
                                CreatedAt = dto.CreatedAt,
                                Likes = dto.Likes ?? new List<Like>(),
                                Comments = dto.Comments?.Select(c => new Comment
                                {
                                    Id = c.Id,
                                    Author = c.Author,
                                    Text = c.Text,
                                    Replies = new List<ICommentComponent>()
                                }).ToList() ?? new List<Comment>()
                            };

                            _posts.Add(newPost);
                        }
                        else
                        {
                            // 2. Пост існує - Зливаємо лайки та коментарі (Merge)
                            if (dto.Likes != null)
                            {
                                existingPost.Likes ??= new List<Like>();
                                var existingUsers = existingPost.Likes.Select(l => l.Username).ToHashSet();

                                foreach (var like in dto.Likes)
                                {
                                    if (!existingUsers.Contains(like.Username))
                                    {
                                        existingPost.Likes.Add(like);
                                    }
                                }
                            }

                            if (dto.Comments != null)
                            {
                                existingPost.Comments ??= new List<Comment>();
                                var existingCommentIds = existingPost.Comments.Select(c => c.Id).ToHashSet();

                                foreach (var cDto in dto.Comments)
                                {
                                    if (!existingCommentIds.Contains(cDto.Id))
                                    {
                                        existingPost.Comments.Add(new Comment
                                        {
                                            Id = cDto.Id,
                                            Author = cDto.Author,
                                            Text = cDto.Text,
                                            Replies = new List<ICommentComponent>()
                                        });
                                    }
                                }
                            }
                        }
                    }

                    // Оновлюємо лічильники
                    if (_posts.Any())
                    {
                        _nextPostId = _posts.Max(p => p.Id) + 1;
                        var allComments = _posts.Where(p => p.Comments != null).SelectMany(p => p.Comments);
                        if (allComments.Any())
                        {
                            _nextCommentId = allComments.Max(c => c.Id) + 1;
                        }
                    }
                }
            }
            catch (JsonException ex)
            {
                throw new Exception("Не вдалося імпортувати дані. Перевірте цілісність JSON.", ex);
            }
            finally { _rwLock.ExitWriteLock(); }
        }

        // ==========================================
        // ДОПОМІЖНІ КЛАСИ DTO (Data Transfer Objects)
        // ==========================================
        private class PostBackupDto
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }
            public string Author { get; set; }
            public DateTime CreatedAt { get; set; }
            public List<Like> Likes { get; set; }
            public List<CommentBackupDto> Comments { get; set; }
        }

        private class CommentBackupDto
        {
            public int Id { get; set; }
            public string Author { get; set; }
            public string Text { get; set; }
        }
    }
}