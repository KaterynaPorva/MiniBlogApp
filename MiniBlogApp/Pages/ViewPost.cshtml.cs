using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniBlogApp.Models;
using MiniBlogApp.Facades; // ПІДКЛЮЧАЄМО ФАСАД
using NToastNotify;

namespace MiniBlogApp.Pages
{
    /**
     * @file ViewPost.cshtml.cs
     * @brief Модель сторінки для перегляду одного поста.
     * @details Використовує IBlogFacade для доступу до даних та виконання дій (лайки, коментарі).
     */
    public class ViewPostModel : PageModel
    {
        private readonly IToastNotification _toastNotification;
        private readonly IBlogFacade _blogFacade;

        public ViewPostModel(IToastNotification toastNotification, IBlogFacade blogFacade)
        {
            _toastNotification = toastNotification;
            _blogFacade = blogFacade;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public Post? Post { get; set; }
        public string? Username { get; set; }

        public IActionResult OnGet()
        {
            Username = HttpContext.Session.GetString("Username");

            // Фасад сам дістає пост і конвертує Markdown в HTML
            Post = _blogFacade.GetPostForView(Id);

            if (Post == null)
            {
                return RedirectToPage("/Index");
            }

            return Page();
        }

        public IActionResult OnPostLike(int id)
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username)) return RedirectToPage("/Login");

            // Лайкаємо через Фасад
            _blogFacade.AddLike(id, username);
            _toastNotification.AddInfoToastMessage("Ви вподобали цей пост! ❤️");

            return RedirectToPage(new { id });
        }

        /**
         * @brief Обробка додавання коментаря або відповіді.
         * @param id ID поста.
         * @param commentText Текст коментаря.
         * @param parentCommentId ID батьківського коментаря (для вкладеності).
         */
        public IActionResult OnPostComment(int id, string commentText, int? parentCommentId)
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username)) return RedirectToPage("/Login");

            if (!string.IsNullOrWhiteSpace(commentText))
            {
                // Передаємо ID батька у фасад. Якщо він null — це звичайний коментар.
                _blogFacade.AddComment(id, username, commentText, parentCommentId);

                string message = parentCommentId.HasValue ? "Відповідь додано! 💬" : "Коментар успішно додано! 💬";
                _toastNotification.AddSuccessToastMessage(message);
            }
            else
            {
                _toastNotification.AddErrorToastMessage("Текст не може бути порожнім.");
            }

            return RedirectToPage(new { id });
        }
    }
}