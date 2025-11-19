using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniBlogApp.Models;
using MiniBlogApp.Services;
using System.Collections.Generic;
using System.Linq;

namespace MiniBlogApp.Pages
{
    /**
     * @file PostAnalytics.cshtml.cs
     * @brief Page model for blog post analytics.
     *
     * @details This file contains the PostAnalyticsModel class used in MiniBlogApp.
     *          It provides functionality to analyze all posts, generate textual summaries,
     *          and compute statistics for overall blog performance and specific users.
     *
     * @example PostAnalytics.cshtml.cs
     * @code
     * var model = new PostAnalyticsModel();
     * model.OnGet();
     * // model.AnalyzedPosts now contains textual analysis for each post
     * // model.Summary contains overall and user-specific statistics
     * @endcode
     */
    public class PostAnalyticsModel : PageModel
    {
        /**
         * @class PostAnalyticsModel
         * @brief Handles the analytics and statistics of blog posts.
         *
         * @details Retrieves all posts from BlogStorage, analyzes them individually,
         *          and stores results for display on the analytics page.
         */

        /**
         * @brief List of analyzed posts in textual format.
         * @return List<string> Each element contains information about the post such as author, likes, and comments.
         * @details Populated by analyzing each post individually using PostAnalyzer.
         */
        public List<string> AnalyzedPosts { get; set; } = new();

        /**
         * @brief Summary statistics for all posts.
         * @return List<string> The first element contains overall statistics; the second contains statistics for a specific user.
         * @details Useful for quickly evaluating the blog's performance and user engagement using PostStatsUtils.
         */
        public List<string> Summary { get; set; } = new();

        /**
         * @brief Handles GET request for the post analytics page.
         * @details Loads all posts from BlogStorage, analyzes each post using PostAnalyzer,
         *          and generates summary statistics using PostStatsUtils. Populates
         *          AnalyzedPosts and Summary properties for display on the page.
         */
        public void OnGet()
        {
            var posts = BlogStorage.GetAllPosts().ToList();

            foreach (var post in posts)
            {
                var analyzer = new PostAnalyzer<Post>();
                AnalyzedPosts.Add(analyzer.Analyze(post));
            }

            Summary.Add(PostStatsUtils.Summarize(posts));
            Summary.Add(PostStatsUtils.Summarize(posts, "serhii"));
        }
    }
}
