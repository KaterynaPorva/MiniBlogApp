using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniBlogApp.Models;
using MiniBlogApp.Services;

namespace MiniBlogApp.Pages
{
    public class EditPostModel : PageModel
    {
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

            var post = BlogStorage.GetPostById(Id);
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

            var post = BlogStorage.GetPostById(Id);
            if (post == null || post.Author != Username)
                return RedirectToPage("/MyPosts");

            BlogStorage.UpdatePost(Id, Title, PostContent);

            return RedirectToPage("/MyPosts");
        }
    }
}
