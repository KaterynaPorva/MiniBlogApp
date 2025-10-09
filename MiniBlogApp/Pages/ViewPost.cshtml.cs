using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniBlogApp.Models;
using MiniBlogApp.Services;

namespace MiniBlogApp.Pages
{
    public class ViewPostModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public Post? Post { get; set; }

        public string? Username { get; set; }

        public void OnGet()
        {
            Username = HttpContext.Session.GetString("Username");
            Post = BlogStorage.GetPostById(Id);
        }

        public IActionResult OnPostLike(int id)
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return RedirectToPage("/Login");

            BlogStorage.AddLike(id, username);
            return RedirectToPage(new { id });
        }

        public IActionResult OnPostComment(int id, string commentText)
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return RedirectToPage("/Login");

            if (!string.IsNullOrWhiteSpace(commentText))
                BlogStorage.AddComment(id, username, commentText);

            return RedirectToPage(new { id });
        }
    }
}
