using MiniBlogApp.Models;
using MiniBlogApp.Services;
using MiniBlogApp.Observers;
using MiniBlogApp.Commands; // 1. ПІДКЛЮЧАЄМО КОМАНДИ
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

        // ПАТЕРН OBSERVER: Список підписників
        private readonly List<IBlogObserver> _observers = new();

        // 2. ПАТЕРН COMMAND: Менеджер для керування історією команд
        private readonly CommandManager _commandManager = new();

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

        /**
         * @brief ПАТЕРН COMMAND: Метод для скасування останньої дії користувача.
         */
        public void UndoLastAction()
        {
            _commandManager.UndoLastCommand();
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

            // ПАТЕРН COMPOSITE: Рекурсивний підрахунок коментарів
            int total = 0;
            foreach (var comment in post.Comments)
            {
                total += comment.GetTotalCount();
            }
            viewPost.TotalCommentsCount = total;

            return viewPost;
        }

        /**
         * @brief ПАТЕРН COMMAND: Реалізація додавання лайку через команду.
         */
        public void AddLike(int postId, string username)
        {
            // Замість прямого виклику сховища, створюємо об'єкт команди
            var likeCommand = new LikeCommand(_blogStorage, postId, username);

            // Виконуємо її через менеджер, щоб вона потрапила в історію (для Undo)
            _commandManager.ExecuteCommand(likeCommand);
        }

        public void AddComment(int postId, string username, string text, int? parentCommentId = null)
        {
            if (parentCommentId.HasValue)
            {
                _blogStorage.AddReply(postId, parentCommentId.Value, username, text);
            }
            else
            {
                _blogStorage.AddComment(postId, username, text);
            }

            // ПАТЕРН OBSERVER: Сповіщення
            foreach (var observer in _observers)
            {
                observer.Update(username, text);
            }
        }
    }
}