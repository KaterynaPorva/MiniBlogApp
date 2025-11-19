using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniBlogApp.Models;
using MiniBlogApp.Services;

namespace MiniBlogApp.Pages
{
    /**
     * @file EditPost.cshtml.cs
     * @brief Page model for editing an existing blog post.
     *
     * @details This file contains the PageModel class used in MiniBlogApp
     *          for editing posts. It handles both GET and POST requests.
     *          Only authenticated users can edit their own posts. Ownership
     *          is verified against the current session username.
     *
     * @example EditPost.cshtml.cs
     * @code
     * var model = new EditPostModel();
     * model.Id = 1;
     * model.Title = "Updated Title";
     * model.PostContent = "Updated content of the post.";
     * IActionResult result = model.OnPost();
     * @endcode
     */
    public class EditPostModel : PageModel
    {
        /**
         * @class EditPostModel
         * @brief Handles editing of an existing blog post.
         *
         * @details Retrieves the post by ID, populates the form fields for editing,
         *          verifies user ownership, validates inputs, and updates the post
         *          via BlogStorage. Redirects appropriately if unauthorized.
         */

        /**
         * @brief Identifier of the post to edit.
         * @details Bound to the GET request query parameter to specify which post
         *          should be loaded for editing. This value is required for both
         *          GET and POST operations.
         * @param Id Unique identifier of the post.
         */
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        /**
         * @brief Title of the post to edit.
         * @details Bound to the Razor page form input for editing the post title.
         *          Should not be empty. Used to update the post in storage.
         * @param Title Updated title for the post.
         */
        [BindProperty]
        public string Title { get; set; } = string.Empty;

        /**
         * @brief Content of the post to edit.
         * @details Bound to the Razor page textarea for editing the post content.
         *          Should not be empty. Used to update the post in storage.
         * @param PostContent Updated content for the post.
         */
        [BindProperty]
        public string PostContent { get; set; } = string.Empty;

        /**
         * @brief Username of the currently logged-in user.
         * @details Retrieved from the session to verify ownership of the post.
         */
        public string? Username { get; set; }

        /**
         * @brief Handles GET request for editing a post.
         * @details Retrieves the post by ID and populates form fields for editing.
         *          Redirects to Login page if user is not authenticated.
         *          Redirects to MyPosts page if the post does not exist or is
         *          owned by another user.
         * @return IActionResult Returns the page for editing the post or redirects
         *                        as necessary.
         * @throws InvalidOperationException If the user is not logged in (handled via redirect).
         */
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

        /**
         * @brief Handles POST request to update the post.
         * @details Updates the post title and content in storage after verifying
         *          that the user is authenticated and owns the post. Redirects
         *          to MyPosts page after successful update.
         * @return IActionResult Redirects to MyPosts page or to Login if unauthorized.
         * @throws InvalidOperationException If the user is not logged in or does not
         *                                   own the post (handled via redirect).
         */
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
