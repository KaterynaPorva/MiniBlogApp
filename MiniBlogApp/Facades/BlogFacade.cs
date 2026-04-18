using MiniBlogApp.Models;
using MiniBlogApp.Services;
using MiniBlogApp.Observers; 
using System.Collections.Generic;

namespace MiniBlogApp.Facades
{
    /**
     * @file BlogFacade.cs
     * @brief Реалізація патернів Facade та Observer.
     * @details Крім спрощення інтерфейсу, клас тепер виступає як Subject (Суб'єкт) 
     * у патерні Observer, дозволяючи іншим сервісам підписуватися на події блогу.
     */
    public class BlogFacade : IBlogFacade
    {
        private readonly IBlogStorage _blogStorage;
        private readonly IMarkdownParser _markdownParser;

        // 2. ПАТЕРН OBSERVER: Список підписників
        private readonly List<IBlogObserver> _observers = new();

        public BlogFacade(IBlogStorage blogStorage, IMarkdownParser markdownParser)
        {
            _blogStorage = blogStorage;
            _markdownParser = markdownParser;
        }

        /**
         * @brief Метод для реєстрації спостерігачів.
         */
        public void Subscribe(IBlogObserver observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
        }

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

            // ПАТЕРН COMPOSITE: Рекурсивний підрахунок
            int total = 0;
            foreach (var comment in post.Comments)
            {
                total += comment.GetTotalCount();
            }
            viewPost.TotalCommentsCount = total;

            return viewPost;
        }

        public void AddLike(int postId, string username)
        {
            _blogStorage.AddLike(postId, username);
        }

        /**
         * @brief Додає коментар та сповіщає всіх спостерігачів.
         */
        public void AddComment(int postId, string username, string text, int? parentCommentId = null)
        {
            // Виконуємо дію в сховищі
            if (parentCommentId.HasValue)
            {
                _blogStorage.AddReply(postId, parentCommentId.Value, username, text);
            }
            else
            {
                _blogStorage.AddComment(postId, username, text);
            }

            // 3. ПАТЕРН OBSERVER: Сповіщаємо всіх підписників про нову подію
            foreach (var observer in _observers)
            {
                observer.Update(username, text);
            }
        }
    }
}