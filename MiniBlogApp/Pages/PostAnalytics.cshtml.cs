using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniBlogApp.Models;
using MiniBlogApp.Services;

namespace MiniBlogApp.Pages
{
    public class PostAnalyticsModel : PageModel
    {
        public List<string> AnalyzedPosts { get; set; } = new();
        public List<string> Summary { get; set; } = new();

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
