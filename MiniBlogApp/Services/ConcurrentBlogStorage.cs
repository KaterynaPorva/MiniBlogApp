using MiniBlogApp.Models;
using MiniBlogApp.Strategies;
using MiniBlogApp.Composites;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MiniBlogApp.Services
{
    /// <summary>
    /// @brief Потокобезпечне сховище дописів на основі ReaderWriterLockSlim.
    /// @details Дозволяє паралельне читання багатьма потоками, але надає ексклюзивний доступ під час запису.
    /// </summary>
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
                try
                {
                    return _posts.ToList();
                }
                finally
                {
                    _rwLock.ExitReadLock();
                }
            }
        }

        public Post AddPost(Post post)
        {
            _rwLock.EnterWriteLock();
            try
            {
                post.Id = _nextPostId++;
                post.Likes ??= new List<Like>();
                post.Comments ??= new List<Comment>(); // Тут List<Comment>
                _posts.Add(post);
                return post;
            }
            finally
            {
                _rwLock.ExitWriteLock();
            }
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
            finally
            {
                _rwLock.ExitWriteLock();
            }
        }

        public void DeletePost(int id)
        {
            _rwLock.EnterWriteLock();
            try
            {
                var post = _posts.FirstOrDefault(p => p.Id == id);
                if (post != null)
                {
                    _posts.Remove(post);
                }
            }
            finally
            {
                _rwLock.ExitWriteLock();
            }
        }

        public IEnumerable<Post> GetAllPosts()
        {
            _rwLock.EnterReadLock();
            try
            {
                return _posts.ToList();
            }
            finally
            {
                _rwLock.ExitReadLock();
            }
        }

        public IEnumerable<Post> GetPostsByUser(string username)
        {
            _rwLock.EnterReadLock();
            try
            {
                return _posts.Where(p => p.Author == username).ToList();
            }
            finally
            {
                _rwLock.ExitReadLock();
            }
        }

        public Post? GetPostById(int id)
        {
            _rwLock.EnterReadLock();
            try
            {
                return _posts.FirstOrDefault(p => p.Id == id);
            }
            finally
            {
                _rwLock.ExitReadLock();
            }
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
            finally
            {
                _rwLock.ExitWriteLock();
            }
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
                    if (like != null)
                    {
                        post.Likes.Remove(like);
                    }
                }
            }
            finally
            {
                _rwLock.ExitWriteLock();
            }
        }

        public void AddComment(int postId, string author, string text)
        {
            _rwLock.EnterWriteLock();
            try
            {
                var post = _posts.FirstOrDefault(p => p.Id == postId);
                if (post != null)
                {
                    post.Comments ??= new List<Comment>(); // Тут List<Comment>
                    post.Comments.Add(new Comment
                    {
                        Id = _nextCommentId++,
                        Author = author,
                        Text = text,
                        Replies = new List<ICommentComponent>() // А ось тут ICommentComponent
                    });
                }
            }
            finally
            {
                _rwLock.ExitWriteLock();
            }
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
                        parentComment.Replies ??= new List<ICommentComponent>(); // Тут ICommentComponent
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
            finally
            {
                _rwLock.ExitWriteLock();
            }
        }

        public IEnumerable<Post> GetAllPosts(IPostSortStrategy sortStrategy)
        {
            _rwLock.EnterReadLock();
            List<Post> snapshot;
            try
            {
                snapshot = _posts.ToList();
            }
            finally
            {
                _rwLock.ExitReadLock();
            }

            return sortStrategy.Sort(snapshot);
        }
    }
}