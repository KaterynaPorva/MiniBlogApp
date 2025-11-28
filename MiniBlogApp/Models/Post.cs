namespace MiniBlogApp.Models
{
    /**
     * @file Post.cs
     * @brief Defines the Post model used in the MiniBlogApp.
     * 
     * @details This class represents a single blog post including its author, title,
     *          content, creation date, and associated comments and likes. It is the core
     *          entity used for displaying and managing posts in the blog application.
     * 
     * @example Post.cs
     * @details Example of creating a new Post instance and setting its properties.
     * @code
     * var post = new Post();
     * post.Author = "serhii";
     * post.Title = "My First Blog Post";
     * post.Content = "This is the content of my first post.";
     * @endcode
     */
    public class Post
    {
        /**
         * @brief Unique identifier of the post.
         * 
         * @details Used to uniquely identify each post in storage
         *          and for operations such as editing or deleting a post.
         */
        public int Id { get; set; }

        /**
         * @brief Author of the post.
         * 
         * @details Stores the username of the user who created the post.
         *          Used for ownership checks, display, and filtering posts by user.
         */
        public string Author { get; set; } = string.Empty;

        /**
         * @brief Title of the post.
         * 
         * @details The main heading of the post shown in listings and detail pages.
         */
        public string Title { get; set; } = string.Empty;

        /**
         * @brief Content of the post.
         * 
         * @details Stores the main text body of the post.
         */
        public string Content { get; set; } = string.Empty;

        /**
         * @brief Date and time when the post was created.
         * 
         * @details Automatically set to the current date and time when a post is created.
         */
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        /**
         * @brief List of comments associated with this post.
         * 
         * @details Stores all comments added by users. Each comment contains
         *          the author, text, and creation date.
         */
        public List<Comment> Comments { get; set; } = new();

        /**
         * @brief List of likes associated with this post.
         * 
         * @details Stores information about users who liked this post.
         */
        public List<Like> Likes { get; set; } = new();
    }
}
