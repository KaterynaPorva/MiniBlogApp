using MiniBlogApp.Models;

namespace MiniBlogApp.Services
{
    public class PostAnalyzer<T> where T : Post
    {
        public string Analyze(T post)
        {
            var title = post.Title ?? "без назви";
            var author = post.Author ?? "невідомий";
            var likesCount = post.Likes?.Count ?? 0;
            var commentsCount = post.Comments?.Count ?? 0;

            return $"Пост '{title}' створений користувачем {author}, має {likesCount} лайків і {commentsCount} коментарів.";
        }
    }

    public static class PostStatsUtils
    {
        public static string Summarize(IEnumerable<Post> posts)
        {
            if (posts == null) return "Всього постів: 0, всього лайків: 0, всього коментарів: 0";

            return $"Всього постів: {posts.Count()}, всього лайків: {posts.Sum(p => p.Likes?.Count ?? 0)}, всього коментарів: {posts.Sum(p => p.Comments?.Count ?? 0)}";
        }

        public static string Summarize(IEnumerable<Post> posts, string author)
        {
            if (posts == null) return $"Пости користувача {author}: 0, лайки: 0, коментарі: 0";

            var authorPosts = posts.Where(p => p.Author == author);
            return $"Пости користувача {author}: {authorPosts.Count()}, лайки: {authorPosts.Sum(p => p.Likes?.Count ?? 0)}, коментарі: {authorPosts.Sum(p => p.Comments?.Count ?? 0)}";
        }
    }
}
