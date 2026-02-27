using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniBlogApp.Models;
using MiniBlogApp.Services;
using NToastNotify;

namespace MiniBlogApp.Pages
{
    /**
     * @file EditPost.cshtml.cs
     * @brief Page model for editing an existing blog post.
     *
     * @details This file contains the PageModel class used in MiniBlogApp
     * for editing posts. It handles both GET and POST requests.
     * Only authenticated users can edit their own posts. Ownership
     * is verified against the current session username.
     * Includes NToastNotify for user feedback on updates.
     */
    public class EditPostModel : PageModel
    {
        private readonly IToastNotification _toastNotification;
        private readonly IBlogStorage _blogStorage; // 1. Додаємо поле для сервісу

        /**
         * @brief Constructor for EditPostModel.
         * @param toastNotification Injected service for displaying notifications.
         * @param blogStorage Injected service for blog storage operations.
         */
        public EditPostModel(IToastNotification toastNotification, IBlogStorage blogStorage)
        {
            _toastNotification = toastNotification;
            _blogStorage = blogStorage; // 2. Ініціалізуємо сервіс
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public string Title { get; set; } = string.Empty;

        [BindProperty]
        public string PostContent { get; set; } = string.Empty;

        public string? Username { get; set; }

        public IActionResult OnGet()
        {
            Username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(Username))
                return RedirectToPage("/Login");

            // 3. Використовуємо _blogStorage замість статичного класу
            var post = _blogStorage.GetPostById(Id);
            if (post == null || post.Author != Username)
                return RedirectToPage("/MyPosts");

            Title = post.Title;
            PostContent = post.Content;

            return Page();
        }

        public IActionResult OnPost()
        {
            Username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(Username))
                return RedirectToPage("/Login");

            // 4. Знову використовуємо _blogStorage
            var post = _blogStorage.GetPostById(Id);
            if (post == null || post.Author != Username)
                return RedirectToPage("/MyPosts");

            // 5. Оновлюємо пост через наш інжектований сервіс
            _blogStorage.UpdatePost(Id, Title, PostContent);

            _toastNotification.AddSuccessToastMessage($"Пост '{Title}' успішно оновлено! ✏️");

            return RedirectToPage("/MyPosts");
        }
    }
}