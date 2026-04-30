using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniBlogApp.Services;
using MiniBlogApp.Strategies;
using System.Linq;

namespace MiniBlogApp.Pages
{
    public class PostAnalyticsModel : PageModel
    {
        private readonly IBlogStorage _storage;
        private readonly IPostAnalyticsStrategy _analyticsStrategy;

        // Властивості для початкового завантаження сторінки
        public int TotalPosts { get; set; }
        public int TotalLikes { get; set; }
        public int TotalComments { get; set; }

        public PostAnalyticsModel(IBlogStorage storage)
        {
            _storage = storage;
            // Використовуємо паралельну стратегію (з Лаб 3a) для розрахунків
            _analyticsStrategy = new ParallelPostAnalyticsStrategy();
        }

        public void OnGet()
        {
            var posts = _storage.GetAllPosts().ToList();
            TotalPosts = posts.Count;
            TotalLikes = _analyticsStrategy.CalculateTotalLikes(posts);
            TotalComments = _analyticsStrategy.CalculateTotalComments(posts);
        }

        // AJAX-обробник для живого оновлення
        public IActionResult OnGetLiveStats()
        {
            var posts = _storage.GetAllPosts().ToList();

            // Повертаємо дані у форматі JSON
            return new JsonResult(new
            {
                totalPosts = posts.Count,
                totalLikes = _analyticsStrategy.CalculateTotalLikes(posts),
                totalComments = _analyticsStrategy.CalculateTotalComments(posts)
            });
        }
    }
}