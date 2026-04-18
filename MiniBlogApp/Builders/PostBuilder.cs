using MiniBlogApp.Models;
using System;

namespace MiniBlogApp.Builders
{
    /// <summary>
    /// Конкретна реалізація патерну Builder для класу Post.
    /// Дозволяє покроково конструювати об'єкт поста.
    /// </summary>
    public class PostBuilder : IPostBuilder
    {
        // Ініціалізуємо одразу, щоб C# не видавав попередження (Error 4)
        private Post _post = new Post();

        public PostBuilder()
        {
            Reset();
        }

        public void Reset()
        {
            _post = new Post();
            _post.CreatedAt = DateTime.UtcNow; // Встановлюємо дату за замовчуванням
        }

        public IPostBuilder SetTitle(string title)
        {
            _post.Title = title;
            return this;
        }

        public IPostBuilder SetContent(string content)
        {
            _post.Content = content;
            return this;
        }

        // Приймаємо рядок (Username), а не об'єкт (Errors 1, 2, 3)
        public IPostBuilder SetAuthor(string authorName)
        {
            _post.Author = authorName;
            return this;
        }

        public Post Build()
        {
            Post result = _post;
            // Після виклику Build(), будівельник готовий до створення нового поста
            Reset();
            return result;
        }
    }
}