using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniBlogApp.Models;
using MiniBlogApp.Services;
using System.Collections.Generic;
using System.Linq;

namespace MiniBlogApp.Pages
{
    /**
     * @file Index.cshtml.cs
     * @brief Model for the main blog page.
     *
     * @details This file contains the IndexModel class used in MiniBlogApp.
     * It displays all blog posts and information about the currently logged-in user.
     * Provides functionality to log out and clear the user session.
     *
     * @example Index.cshtml.cs
     * @code
     * var model = new IndexModel(blogStorage);
     * model.OnGet();
     * var posts = model.AllPosts;
     * string? username = model.Username;
     * IActionResult logoutResult = model.OnPostLogout();
     * @endcode
     */
    public class IndexModel : PageModel
    {
        /**
         * @class IndexModel
         * @brief Handles display of all blog posts and user session information.
         *
         * @details Uses injected IBlogStorage to retrieve posts and session data to identify
         * the currently logged-in user. Provides methods to load posts
         * on page load and to handle logout.
         */

        private readonly IBlogStorage _blogStorage; // 1. Додаємо поле для сервісу

        /**
         * @brief Constructor for IndexModel.
         * @param blogStorage Injected service for blog storage operations.
         */
        public IndexModel(IBlogStorage blogStorage)
        {
            _blogStorage = blogStorage; // 2. Ініціалізуємо сервіс
        }

        /**
         * @brief Collection of all blog posts.
         * @return List<Post> Returns a list of all posts available in the blog.
         * @details Populated from IBlogStorage when the page is loaded via OnGet method.
         */
        public List<Post> AllPosts { get; set; } = new();

        /**
         * @brief Username of the currently logged-in user.
         * @details This property is used to display user information and control access to certain actions.
         */
        public string? Username { get; set; }

        /**
         * @brief Handles GET request for the main blog page.
         * @details Retrieves the username from the session and loads all posts from IBlogStorage.
         * If no user is logged in, Username will be null.
         */
        public void OnGet()
        {
            Username = HttpContext.Session.GetString("Username");

            // 3. Використовуємо інжектований сервіс замість статичного класу
            AllPosts = _blogStorage.GetAllPosts().ToList();
        }

        /**
         * @brief Handles POST request to log out the user.
         * @details Clears the current session and redirects the user to the login page.
         * Ensures that no session data is retained after logout.
         * @return IActionResult Redirects to the Login page after session clearance.
         */
        public IActionResult OnPostLogout()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/Login");
        }
    }
}