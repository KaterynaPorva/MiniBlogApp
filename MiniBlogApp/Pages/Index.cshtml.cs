using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniBlogApp.Models;
using MiniBlogApp.Services;

namespace MiniBlogApp.Pages
{
    public class IndexModel : PageModel
    {
        public List<Post> AllPosts { get; set; } = new();
        public string? Username { get; set; }

        public void OnGet()
        {
            Username = HttpContext.Session.GetString("Username");
            AllPosts = BlogStorage.GetAllPosts().ToList();
        }
        public IActionResult OnPostLogout()
        {
            HttpContext.Session.Clear(); 
            return RedirectToPage("/Login");
        }
    }
}