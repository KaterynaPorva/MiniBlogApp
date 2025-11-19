using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniBlogApp.Services;

namespace MiniBlogApp.Pages
{
    /**
     * @file CreatePost.cshtml.cs
     * @brief Page model for creating a new blog post.
     *
     * @details This file contains the PageModel class used in MiniBlogApp for creating new posts.
     *          Handles both GET and POST requests. Only authenticated users can create posts.
     *          Validates input fields and saves posts using BlogStorage.
     *
     * @example CreatePost.cshtml.cs
     * @details Simulating creation of a new post for user "serhii"
     * result redirects to /MyPosts if successful
     * @code
     * var model = new CreatePostModel();
     * model.Title = "My New Post";
     * model.PostContent = "This is the content of my new post.";
     * IActionResult result = model.OnPost();
     * @endcode
     */
    public class CreatePostModel : PageModel
    {
        /**
         * @brief Title of the new post.
         * @details Bound to the input field on the Razor page form.
         *          Represents the main heading of the post. Cannot be empty.
         * @bindproperty
         */
        [BindProperty]
        public string Title { get; set; } = string.Empty;

        /**
         * @brief Content of the new post.
         * @details Bound to the textarea field on the Razor page form.
         *          Stores the main text of the blog post. Cannot be empty.
         * @bindproperty
         */
        [BindProperty]
        public string PostContent { get; set; } = string.Empty;

        /**
         * @brief Username of the currently logged-in user.
         * @details Retrieved from the session to associate the post with the user.
         */
        public string? Username { get; set; }

        /**
         * @brief Handles GET requests for the Create Post page.
         * @details Checks authentication. If the user is not logged in, redirects to the Login page.
         *          Otherwise, returns the page to display the form for creating a new post.
         * @return IActionResult Returns the page or redirects to login if unauthenticated.
         */
        public IActionResult OnGet()
        {
            Username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(Username))
                return RedirectToPage("/Login");

            return Page();
        }

        /**
         * @brief Handles POST requests to create a new post.
         * @details Validates that Title and PostContent are not empty. 
         *          If validation fails, adds an error message and returns the same page.
         *          If valid, saves the post via BlogStorage and redirects to the MyPosts page.
         * @return IActionResult Redirects to MyPosts on success, or returns the same page on validation failure.
         * @throws InvalidOperationException If the user is not logged in (handled via redirect).
         */
        public IActionResult OnPost()
        {
            Username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(Username))
                return RedirectToPage("/Login");

            if (string.IsNullOrWhiteSpace(Title) || string.IsNullOrWhiteSpace(PostContent))
            {
                ModelState.AddModelError("", "Please fill in all fields.");
                return Page();
            }

            BlogStorage.AddPost(Username, Title, PostContent);
            return RedirectToPage("/MyPosts");
        }
    }
}
