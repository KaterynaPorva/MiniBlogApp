using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniBlogApp.Services;
using MiniBlogApp.Builders;
using MiniBlogApp.Models;
using NToastNotify;

namespace MiniBlogApp.Pages
{
    /**
     * @file CreatePost.cshtml.cs
     * @brief Page model for creating a new blog post.
     *
     * @details This file contains the PageModel class used in MiniBlogApp for creating new posts.
     * Handles both GET and POST requests. Only authenticated users can create posts.
     * Validates input fields and saves posts using injected IBlogStorage.
     */
    public class CreatePostModel : PageModel
    {
        private readonly IToastNotification _toastNotification;
        private readonly IBlogStorage _blogStorage;
        private readonly IPostBuilder _postBuilder;

        /**
         * @brief Constructor to inject services.
         * @param toastNotification The toast notification service.
         * @param blogStorage The blog storage service.
         * @param postBuilder The builder pattern interface for creating posts.
         */
        public CreatePostModel(IToastNotification toastNotification, IBlogStorage blogStorage, IPostBuilder postBuilder)
        {
            _toastNotification = toastNotification;
            _blogStorage = blogStorage;
            _postBuilder = postBuilder;
        }

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

            return Page();
        }

        public IActionResult OnPost()
        {
            Username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(Username))
                return RedirectToPage("/Login");

            if (string.IsNullOrWhiteSpace(Title) || string.IsNullOrWhiteSpace(PostContent))
            {
                ModelState.AddModelError("", "Please fill in all fields.");
                _toastNotification.AddErrorToastMessage("Помилка! Заповніть усі поля.");
                return Page();
            }

            // 3. ВИКОРИСТАННЯ ПАТЕРНУ BUILDER
            // ПРАВИЛЬНИЙ ВИКЛИК: Передаємо просто Username (який є string), без створення об'єкта BlogUser
            Post newPost = _postBuilder
                .SetTitle(Title)
                .SetContent(PostContent)
                .SetAuthor(Username) // Виправлено тут
                .Build();

            // 4. Передаємо готовий об'єкт у сховище
            _blogStorage.AddPost(newPost);

            _toastNotification.AddSuccessToastMessage($"Пост '{Title}' успішно створено!");

            return RedirectToPage("/MyPosts");
        }
    }
}