using MiniBlogApp.Models;

namespace MiniBlogApp.Services
{
    public class PostAnalyzer<T> where T : Post
    {
        public string Analyze(T post)
        {
            return $"Пост '{post.Title}' створений користувачем {post.Author}, має {post.Likes.Count} лайків і {post.Comments.Count} коментарів.";
        }
    }

    public static class PostStatsUtils
    {
        public static string Summarize(IEnumerable<Post> posts)
        {
            return $"Всього постів: {posts.Count()}, всього лайків: {posts.Sum(p => p.Likes.Count)}, всього коментарів: {posts.Sum(p => p.Comments.Count)}";
        }

        public static string Summarize(IEnumerable<Post> posts, string author)
        {
            var authorPosts = posts.Where(p => p.Author == author);
            return $"Пости користувача {author}: {authorPosts.Count()}, лайки: {authorPosts.Sum(p => p.Likes.Count)}, коментарі: {authorPosts.Sum(p => p.Comments.Count)}";
        }
    }
}
