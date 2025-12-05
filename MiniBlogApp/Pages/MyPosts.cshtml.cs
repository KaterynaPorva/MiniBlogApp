using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniBlogApp.Models;
using MiniBlogApp.Services;
using System.Collections.Generic;
using System.Linq;
using NToastNotify;

namespace MiniBlogApp.Pages
{
    /**
     * @file MyPosts.cshtml.cs
     * @brief Page model for managing the current user's blog posts.
     *
     * @details This file contains the MyPostsModel class used in MiniBlogApp.
     * It allows authenticated users to view, filter, and delete their own posts.
     * Actions are validated against the current session to ensure security.
     * Now includes NToastNotify to show alert messages when posts are deleted.
     *
     * @example MyPosts.cshtml.cs
     * @code
     * var model = new MyPostsModel(toastNotificationService);
     * IActionResult getResult = model.OnGet();
     * // getResult returns the page with the logged-in user's posts
     * IActionResult deleteResult = model.OnPostDelete(1);
     * // deleteResult redirects to the same page after deleting the post and showing a toast
     * @endcode
     */
    public class MyPostsModel : PageModel
    {
        /**
         * @brief Service for displaying toast notifications.
         * @details Used to show a warning message when a post is deleted.
         */
        private readonly IToastNotification _toastNotification;

        /**
         * @brief Constructor for MyPostsModel.
         * @param toastNotification The injected service for handling UI notifications.
         */
        public MyPostsModel(IToastNotification toastNotification)
        {
            _toastNotification = toastNotification;
        }

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
         * Redirects to the login page if the user is not authenticated.
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
         * If successful, deletes the post and triggers a yellow warning toast notification.
         * If the post does not exist or belongs to another user, no action is taken.
         * @return IActionResult Redirects to the same page after deletion.
         */
        public IActionResult OnPostDelete(int id)
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return RedirectToPage("/Login");

            var post = BlogStorage.GetPostById(id);

            // Перевіряємо, чи пост існує і чи належить він поточному користувачу
            if (post != null && post.Author == username)
            {
                BlogStorage.DeletePost(id);

                // Відображаємо жовте повідомлення (Warning) про видалення
                _toastNotification.AddWarningToastMessage("Пост було видалено. ???");
            }

            return RedirectToPage();
        }
    }
}