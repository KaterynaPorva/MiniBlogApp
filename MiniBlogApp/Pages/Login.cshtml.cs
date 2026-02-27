using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniBlogApp.Models;
using MiniBlogApp.Services;

namespace MiniBlogApp.Pages
{
    /**
     * @file Login.cshtml.cs
     * @brief Page model for the user login page.
     *
     * @details This file contains the LoginModel class used in MiniBlogApp.
     * It handles user authentication via UserService, manages session
     * data for logged-in users, and redirects authenticated users to
     * the main page. Provides feedback when login fails.
     *
     * @example Login.cshtml.cs
     * @code
     * var model = new LoginModel(new UserService());
     * model.Input.Username = "serhii";
     * model.Input.Password = "securepassword123";
     * IActionResult result = model.OnPost();
     * // result redirects to /Index on successful authentication
     * @endcode
     */
    public class LoginModel : PageModel
    {
        /**
         * @class LoginModel
         * @brief Handles authentication and session management for user login.
         *
         * @details Uses UserService to verify credentials, sets session for authenticated users,
         * and displays errors when authentication fails. Redirects already logged-in
         * users to the main blog page.
         */

        /**
         * @brief Service for managing users and authentication.
         * @details Provides methods for verifying credentials and retrieving user information.
         */
        private readonly UserService _userService;

        /**
         * @brief Constructor for LoginModel.
         * @param userService Service used to authenticate users and retrieve user data.
         */
        public LoginModel(UserService userService)
        {
            _userService = userService;
        }

        /**
         * @brief User input data for login.
         * @bindproperty Bound to the login form on the Razor page.
         * @details Stores the username and password entered by the user for authentication.
         * @return BlogUser Contains the login credentials submitted by the user.
         */
        [BindProperty]
        public BlogUser Input { get; set; } = new();

        /**
         * @brief Error message displayed when login fails.
         * @details Populated when the authentication attempt is unsuccessful.
         */
        public string ErrorMessage { get; set; } = string.Empty;

        /**
         * @brief Handles GET requests for the login page.
         * @details Redirects authenticated users (users with an existing session) to the main blog page.
         * @return IActionResult Redirects to index or returns the login page.
         */
        public IActionResult OnGet() // Змінено з void на IActionResult
        {
            var existing = HttpContext.Session.GetString("Username");
            if (!string.IsNullOrEmpty(existing))
            {
                return RedirectToPage("/Index"); // Правильний редирект для Razor Pages
            }

            return Page(); // Повертаємо сторінку, якщо користувач не авторизований
        }

        /**
         * @brief Handles POST requests for user login.
         * @details Authenticates the user using UserService. If successful, sets the session
         * and redirects to the main page. If unsuccessful, displays an error message.
         * @return IActionResult Redirects to the main page on successful login, or returns the login page with an error.
         */
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