using MiniBlogApp.Models;

namespace MiniBlogApp.Services
{
    /**
     * @file PostAnalysisService.cs
     * @brief Provides tools for analyzing individual posts and summarizing post collections.
     * @details Contains PostAnalyzer<T> for analyzing a single post and PostStatsUtils for generating
     *          summaries of multiple posts. Useful for generating textual statistics for blogs.
     * @example PostAnalysisService.cs
     * @code
     * var analyzer = new PostAnalyzer<Post>();
     * string summary = analyzer.Analyze(post);
     * string overallSummary = PostStatsUtils.Summarize(allPosts);
     * string userSummary = PostStatsUtils.Summarize(allPosts, "serhii");
     * @endcode
     */

    /**
     * @class PostAnalyzer<T>
     * @brief Analyzes individual blog posts.
     * @details Provides methods to summarize a single post, including its title, author,
     *          number of likes, and number of comments. Generic allows using derived types of Post.
     */
    public class PostAnalyzer<T> where T : Post
    {
        /**
         * @brief Analyzes a single post.
         * @param post The post to analyze.
         * @return string Summary of the post including title, author, likes, and comments.
         * @details Returns a readable textual summary that can be used in logs, UI, or reports.
         */
        public string Analyze(T post)
        {
            var title = post.Title ?? "без назви";
            var author = post.Author ?? "невідомий";
            var likesCount = post.Likes?.Count ?? 0;
            var commentsCount = post.Comments?.Count ?? 0;

            return $"Пост '{title}' створений користувачем {author}, має {likesCount} лайків і {commentsCount} коментарів.";
        }
    }

    /**
     * @class PostStatsUtils
     * @brief Utility class for summarizing collections of posts.
     * @details Provides overall statistics about all posts or filtered by specific author.
     */
    public static class PostStatsUtils
    {
        /**
         * @brief Summarizes all posts.
         * @param posts Collection of posts.
         * @return string Summary including total posts, likes, and comments.
         * @details Calculates the total number of posts, total likes, and total comments
         *          for the given collection of posts.
         */
        public static string Summarize(IEnumerable<Post> posts)
        {
            if (posts == null)
                return "Всього постів: 0, всього лайків: 0, всього коментарів: 0";

            return $"Всього постів: {posts.Count()}, всього лайків: {posts.Sum(p => p.Likes?.Count ?? 0)}, всього коментарів: {posts.Sum(p => p.Comments?.Count ?? 0)}";
        }

        /**
         * @brief Summarizes posts filtered by author.
         * @param posts Collection of posts.
         * @param author Author name to filter posts.
         * @return string Summary of the author's posts including count, likes, and comments.
         * @details Filters the collection by author and then calculates totals for that author.
         */
        public static string Summarize(IEnumerable<Post> posts, string author)
        {
            if (posts == null)
                return $"Пости користувача {author}: 0, лайки: 0, коментарі: 0";

            var authorPosts = posts.Where(p => p.Author == author);
            return $"Пости користувача {author}: {authorPosts.Count()}, лайки: {authorPosts.Sum(p => p.Likes?.Count ?? 0)}, коментарі: {authorPosts.Sum(p => p.Comments?.Count ?? 0)}";
        }
    }
}
