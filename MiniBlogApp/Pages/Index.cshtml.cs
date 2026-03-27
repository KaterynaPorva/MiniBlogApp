using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniBlogApp.Models;
using MiniBlogApp.Services;
using MiniBlogApp.Strategies;
using System.Collections.Generic;
using System.Linq;

namespace MiniBlogApp.Pages
{
    /**
     * @file Index.cshtml.cs
     * @brief Model for the main blog page.
     *
     * @details This file contains the IndexModel class used in MiniBlogApp.
     * It displays all blog posts and information about the currently logged-in user.
     * Provides functionality to log out and clear the user session.
     */
    public class IndexModel : PageModel
    {
        private readonly IBlogStorage _blogStorage;
        private readonly IMarkdownParser _markdownParser; //Поле для Адаптера

        //Конструктор тепер приймає ще й парсер
        public IndexModel(IBlogStorage blogStorage, IMarkdownParser markdownParser)
        {
            _blogStorage = blogStorage;
            _markdownParser = markdownParser;
        }

        public List<Post> AllPosts { get; set; } = new();

        public string? Username { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SortBy { get; set; } = "date";

        public void OnGet()
        {
            Username = HttpContext.Session.GetString("Username");

            // Вибираємо стратегію на основі запиту користувача (Патерн Strategy)
            IPostSortStrategy strategy;

            if (SortBy == "popular")
            {
                strategy = new PopularitySortStrategy();
            }
            else
            {
                strategy = new DateSortStrategy(); // За замовчуванням
            }

            // Передаємо обрану стратегію у сховище
            AllPosts = _blogStorage.GetAllPosts(strategy).ToList();
        }

        public IActionResult OnPostLogout()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/Login");
        }

        /** * @brief 3. ДОДАНО: Метод для конвертації Markdown в HTML через Адаптер 
         */
        public string ConvertMarkdown(string content)
        {
            return _markdownParser.Parse(content);
        }
    }
}