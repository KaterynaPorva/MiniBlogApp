using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniBlogApp.Models;
using MiniBlogApp.Services;

namespace MiniBlogApp.Pages
{
    /**
     * @file ViewPost.cshtml.cs
     * @brief Page model for viewing a single blog post.
     *
     * @details This file contains the ViewPostModel class used in MiniBlogApp.
     *          It handles displaying a post, adding likes, and adding comments.
     *          Retrieves the current user from the session to allow interactions
     *          and loads the post from BlogStorage using its ID.
     *
     * @example ViewPost.cshtml.cs
     * @code
     * var model = new ViewPostModel();
     * model.Id = 1;
     * model.OnGet();
     * IActionResult likeResult = model.OnPostLike(1);
     * IActionResult commentResult = model.OnPostComment(1, "Great post!");
     * @endcode
     */
    public class ViewPostModel : PageModel
    {
        /**
         * @class ViewPostModel
         * @brief Handles logic for viewing a single blog post.
         *
         * @details Manages the retrieval of a post by ID, allows logged-in users to like or comment,
         *          and ensures the post is associated with the current user session.
         */

        /**
         * @brief The ID of the post to view.
         * @details Bound from the query string using [BindProperty(SupportsGet = true)].
         *          Used to retrieve the post from BlogStorage and perform actions like liking or commenting.
         */
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        /**
         * @brief The post object being viewed.
         * @details Populated in OnGet using BlogStorage.GetPostById.
         *          Contains all information about the post, including author, content, comments, and likes.
         */
        public Post? Post { get; set; }

        /**
         * @brief The username of the currently logged-in user.
         * @details Retrieved from the session in OnGet and action methods.
         *          Used to authorize actions such as liking or commenting.
         */
        public string? Username { get; set; }

        /**
         * @brief Handles GET requests to display the post.
         * @details Retrieves the post by ID and the current username from the session.
         *          Populates the Post and Username properties for page rendering.
         * @return void
         */
        public void OnGet()
        {
            Username = HttpContext.Session.GetString("Username");
            Post = BlogStorage.GetPostById(Id);
        }

        /**
         * @brief Handles POST requests to like a post.
         * @param id The ID of the post to like.
         * @return IActionResult Redirects to the same post page after adding the like.
         * @throws Redirects to the login page if the user is not logged in.
         */
        public IActionResult OnPostLike(int id)
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return RedirectToPage("/Login");

            BlogStorage.AddLike(id, username);
            return RedirectToPage(new { id });
        }

        /**
         * @brief Handles POST requests to add a comment to a post.
         * @param id The ID of the post to comment on.
         * @param commentText The text of the comment.
         * @return IActionResult Redirects to the same post page after adding the comment.
         * @throws Redirects to the login page if the user is not logged in.
         */
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
