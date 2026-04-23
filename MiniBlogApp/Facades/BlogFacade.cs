using MiniBlogApp.Models;
using MiniBlogApp.Services;
using MiniBlogApp.Observers;
using MiniBlogApp.Commands;
using MiniBlogApp.Strategies; 
using System.Collections.Generic;
using System.Linq; 

namespace MiniBlogApp.Facades
{
    public class BlogFacade : IBlogFacade
    {
        private readonly IBlogStorage _blogStorage;
        private readonly IMarkdownParser _markdownParser;
        private readonly List<IBlogObserver> _observers = new();
        private readonly CommandManager _commandManager = new();

        public BlogFacade(IBlogStorage blogStorage, IMarkdownParser markdownParser)
        {
            _blogStorage = blogStorage;
            _markdownParser = markdownParser;
        }

        /**
         * @brief Отримання всіх постів з урахуванням сортування та пошуку.
         * @param strategy Стратегія сортування (Date або Popularity).
         * @param searchQuery Рядок для пошуку.
         */
        public IEnumerable<Post> GetAllPosts(IPostSortStrategy strategy, string? searchQuery = null)
        {
            // 1. Отримуємо відсортовані пости зі сховища (через Proxy та Decorator)
            // Тут використовується патерн Strategy
            var posts = _blogStorage.GetAllPosts(strategy);

            // 2. Якщо користувач ввів запит — фільтруємо результат
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                var query = searchQuery.ToLower();
                posts = posts.Where(p =>
                    p.Title.ToLower().Contains(query) ||
                    p.Content.ToLower().Contains(query) ||
                    p.Author.ToLower().Contains(query)
                );
            }

            return posts.ToList();
        }

        public void Subscribe(IBlogObserver observer)
        {
            if (!_observers.Contains(observer)) _observers.Add(observer);
        }

        public void UndoLastAction() => _commandManager.UndoLastCommand();

        public Post? GetPostForView(int id)
        {
            var post = _blogStorage.GetPostById(id);
            if (post == null) return null;

            var viewPost = new Post
            {
                Id = post.Id,
                Title = post.Title,
                Author = post.Author,
                CreatedAt = post.CreatedAt,
                Likes = post.Likes,
                Comments = post.Comments,
                Content = _markdownParser.Parse(post.Content)
            };

            int total = 0;
            foreach (var comment in post.Comments) total += comment.GetTotalCount();
            viewPost.TotalCommentsCount = total;

            return viewPost;
        }

        public void AddLike(int postId, string username)
        {
            var likeCommand = new LikeCommand(_blogStorage, postId, username);
            _commandManager.ExecuteCommand(likeCommand);
        }

        public void AddComment(int postId, string username, string text, int? parentCommentId = null)
        {
            if (parentCommentId.HasValue)
                _blogStorage.AddReply(postId, parentCommentId.Value, username, text);
            else
                _blogStorage.AddComment(postId, username, text);

            foreach (var observer in _observers) observer.Update(username, text);
        }
    }
}