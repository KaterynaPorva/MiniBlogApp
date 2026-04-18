using MiniBlogApp.Models;
using MiniBlogApp.Services;

namespace MiniBlogApp.Facades
{
    /**
     * @file BlogFacade.cs
     * @brief Реалізація патерну Facade.
     * @details Об'єднує роботу сховища постів та парсера Markdown. 
     * Також координує рекурсивні операції патерну Composite для коментарів.
     */
    public class BlogFacade : IBlogFacade
    {
        private readonly IBlogStorage _blogStorage;
        private readonly IMarkdownParser _markdownParser;

        public BlogFacade(IBlogStorage blogStorage, IMarkdownParser markdownParser)
        {
            _blogStorage = blogStorage;
            _markdownParser = markdownParser;
        }

        /**
         * @brief Отримує пост і готує його до відображення.
         * @details Використовує рекурсію Composite для підрахунку всіх вкладених коментарів.
         */
        public Post? GetPostForView(int id)
        {
            var post = _blogStorage.GetPostById(id);
            if (post == null) return null;

            // Створюємо об'єкт для View, щоб не змінювати оригінал у пам'яті
            var viewPost = new Post
            {
                Id = post.Id,
                Title = post.Title,
                Author = post.Author,
                CreatedAt = post.CreatedAt,
                Likes = post.Likes,
                Comments = post.Comments,
                // Адаптер перетворює Markdown в HTML
                Content = _markdownParser.Parse(post.Content)
            };

            // ПАТЕРН COMPOSITE: Рекурсивно рахуємо загальну кількість (коментарі + відповіді)
            int total = 0;
            foreach (var comment in post.Comments)
            {
                total += comment.GetTotalCount();
            }
            viewPost.TotalCommentsCount = total;

            return viewPost;
        }

        /**
         * @brief Делегує додавання лайку сховищу.
         */
        public void AddLike(int postId, string username)
        {
            _blogStorage.AddLike(postId, username);
        }

        /**
         * @brief Універсальний метод додавання коментарів.
         * @param parentCommentId Якщо передано, додає коментар як відповідь (Composite Node).
         */
        public void AddComment(int postId, string username, string text, int? parentCommentId = null)
        {
            if (parentCommentId.HasValue)
            {
                // Викликаємо рекурсивне додавання відповіді в сховищі
                _blogStorage.AddReply(postId, parentCommentId.Value, username, text);
            }
            else
            {
                // Додаємо звичайний коментар у корінь поста
                _blogStorage.AddComment(postId, username, text);
            }
        }
    }
}