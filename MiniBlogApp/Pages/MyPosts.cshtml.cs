using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniBlogApp.Models;
using MiniBlogApp.Services;

namespace MiniBlogApp.Pages
{
    public class MyPostsModel : PageModel
    {
        public List<Post> MyPosts { get; set; } = new();
        public string? Username { get; set; }

        public IActionResult OnGet()
        {
            Username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(Username))
                return RedirectToPage("/Login");

            MyPosts = BlogStorage.GetPostsByUser(Username).ToList();
            return Page();
        }
    }
}
