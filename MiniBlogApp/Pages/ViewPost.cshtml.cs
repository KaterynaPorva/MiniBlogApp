using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniBlogApp.Models;
using MiniBlogApp.Facades; // ПІДКЛЮЧАЄМО ФАСАД
using NToastNotify;

namespace MiniBlogApp.Pages
{
    public class ViewPostModel : PageModel
    {
        private readonly IToastNotification _toastNotification;
        private readonly IBlogFacade _blogFacade; // Використовуємо Фасад

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

            // Фасад сам дістає пост і конвертує текст
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

        public IActionResult OnPostComment(int id, string commentText)
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username)) return RedirectToPage("/Login");

            if (!string.IsNullOrWhiteSpace(commentText))
            {
                // Коментуємо через Фасад
                _blogFacade.AddComment(id, username, commentText);
                _toastNotification.AddSuccessToastMessage("Коментар успішно додано! 💬");
            }
            else
            {
                _toastNotification.AddErrorToastMessage("Коментар не може бути порожнім.");
            }

            return RedirectToPage(new { id });
        }
    }
}