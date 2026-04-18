using MiniBlogApp.Models;
using MiniBlogApp.Services;

namespace MiniBlogApp.Facades
{
    /// <summary>
    /// Реалізація Фасаду. Приховує складність взаємодії між сховищем та парсером.
    /// </summary>
    public class BlogFacade : IBlogFacade
    {
        private readonly IBlogStorage _blogStorage;
        private readonly IMarkdownParser _markdownParser;

        public BlogFacade(IBlogStorage blogStorage, IMarkdownParser markdownParser)
        {
            _blogStorage = blogStorage;
            _markdownParser = markdownParser;
        }

        public Post? GetPostForView(int id)
        {
            // 1. Звертаємося до бази
            var post = _blogStorage.GetPostById(id);
            if (post == null) return null;

            // 2. Створюємо копію поста для сторінки, 
            // одразу перетворюючи Markdown на HTML за допомогою парсера
            return new Post
            {
                Id = post.Id,
                Title = post.Title,
                Author = post.Author,
                CreatedAt = post.CreatedAt,
                Likes = post.Likes,
                Comments = post.Comments,
                Content = _markdownParser.Parse(post.Content)
            };
        }

        public void AddLike(int postId, string username)
        {
            _blogStorage.AddLike(postId, username);
        }

        public void AddComment(int postId, string username, string text)
        {
            _blogStorage.AddComment(postId, username, text);
        }
    }
}