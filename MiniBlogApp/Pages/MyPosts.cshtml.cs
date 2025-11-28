using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniBlogApp.Models;
using MiniBlogApp.Services;
using System.Collections.Generic;
using System.Linq;

namespace MiniBlogApp.Pages
{
    /**
     * @file MyPosts.cshtml.cs
     * @brief Page model for managing the current user's blog posts.
     *
     * @details This file contains the MyPostsModel class used in MiniBlogApp.
     *          It allows authenticated users to view, filter, and delete their own posts.
     *          Actions are validated against the current session to ensure security.
     *
     * @example MyPosts.cshtml.cs
     * @code
     * var model = new MyPostsModel();
     * IActionResult getResult = model.OnGet();
     * // getResult returns the page with the logged-in user's posts
     * IActionResult deleteResult = model.OnPostDelete(1);
     * // deleteResult redirects to the same page after deleting the post with ID 1
     * @endcode
     */
    public class MyPostsModel : PageModel
    {
        /**
         * @class MyPostsModel
         * @brief Handles displaying and managing posts of the logged-in user.
         *
         * @details Retrieves posts from BlogStorage filtered by the current session username.
         *          Provides functionality to delete posts owned by the user.
         */

        /**
         * @brief Collection of the current user's blog posts.
         * @return List<Post> All posts created by the logged-in user.
         * @details Populated from BlogStorage when OnGet is called.
         */
        public List<Post> MyPosts { get; set; } = new();

        /**
         * @brief Username of the logged-in user.
         * @details Retrieved from the session to filter posts and verify authorization.
         */
        public string? Username { get; set; }

        /**
         * @brief Handles GET request to display the user's posts.
         * @details Loads all posts belonging to the current user.
         *          Redirects to the login page if the user is not authenticated.
         * @return IActionResult Returns the page with the user's posts or redirects to Login if not authenticated.
         */
        public IActionResult OnGet()
        {
            Username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(Username))
                return RedirectToPage("/Login");

            MyPosts = BlogStorage.GetPostsByUser(Username).ToList();
            return Page();
        }

        /**
         * @brief Handles POST request to delete a specific post.
         * @param id The unique identifier of the post to delete.
         * @details Verifies that the post belongs to the logged-in user before deleting it.
         *          If the post does not exist or belongs to another user, no action is taken.
         * @return IActionResult Redirects to the same page after deletion.
         */
        public IActionResult OnPostDelete(int id)
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return RedirectToPage("/Login");

            var post = BlogStorage.GetPostById(id);
            if (post != null && post.Author == username)
            {
                BlogStorage.DeletePost(id);
            }

            return RedirectToPage();
        }
    }
}
