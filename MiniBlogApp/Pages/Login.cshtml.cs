using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniBlogApp.Models;
using MiniBlogApp.Services;

namespace MiniBlogApp.Pages
{
    public class LoginModel : PageModel
    {
        private readonly UserService _userService;
        public LoginModel(UserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public BlogUser Input { get; set; } = new();

        public string ErrorMessage { get; set; } = string.Empty;

        public void OnGet()
        {
            var existing = HttpContext.Session.GetString("Username");
            if (!string.IsNullOrEmpty(existing))
            {
                Response.Redirect("/Index");
            }
        }

        public IActionResult OnPost()
        {
            var user = _userService.Authenticate(Input.Username, Input.Password);
            if (user != null)
            {
                HttpContext.Session.SetString("Username", user.Username);
                return RedirectToPage("/Index");
            }

            ErrorMessage = "Невірний логін або пароль";
            return Page();
        }
    }
}
