using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniBlogApp.Models;
using MiniBlogApp.Services;
using MiniBlogApp.Strategies;
using MiniBlogApp.Facades;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System;

namespace MiniBlogApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IBlogFacade _blogFacade;
        private readonly IMarkdownParser _markdownParser;

        public IndexModel(IBlogFacade blogFacade, IMarkdownParser markdownParser)
        {
            _blogFacade = blogFacade;
            _markdownParser = markdownParser;
        }

        public List<Post> AllPosts { get; set; } = new();
        public string? Username { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchQuery { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SortBy { get; set; } = "date";

        // --- НОВІ ПОЛЯ ДЛЯ ПАГІНАЦІЇ ---
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1; // За замовчуванням 1 сторінка

        public int TotalPages { get; set; }
        public const int PageSize = 5; // Скільки постів показувати на одній сторінці

        public void OnGet()
        {
            Username = HttpContext.Session.GetString("Username");

            IPostSortStrategy strategy = SortBy == "popular"
                ? new PopularitySortStrategy()
                : new DateSortStrategy();

            // 1. Отримуємо всі відфільтровані та відсортовані пости
            var postsQuery = _blogFacade.GetAllPosts(strategy, SearchQuery);

            // 2. Рахуємо загальну кількість сторінок
            int totalCount = postsQuery.Count();
            TotalPages = (int)Math.Ceiling(totalCount / (double)PageSize);

            // Якщо сторінок немає (0 постів), ставимо хоча б 1 сторінку
            if (TotalPages == 0) TotalPages = 1;

            // 3. Відрізаємо тільки ті пости, які потрібні для поточної сторінки
            AllPosts = postsQuery
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();
        }

        public string ConvertMarkdown(string content)
        {
            return _markdownParser.Parse(content);
        }
    }
}