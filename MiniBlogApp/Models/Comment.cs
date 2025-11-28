/**
 * @file Comment.cs
 * @brief Defines the Comment model used for blog posts in the MiniBlogApp.
 *
 * @details This file contains the Comment class which represents a single 
 *          comment left by a user on a blog post. Each comment stores the 
 *          author's username, the text content, and the timestamp of creation. 
 *          It can be extended in the future to include features such as 
 *          likes, replies, or moderation status.
 * 
 * @example Comment.cs
 * @details Example of creating a new Comment instance and setting its properties
 * @code
 * var comment = new Comment();
 * comment.Author = "serhii";
 * comment.Text = "Great post!";
 * comment.CreatedAt = DateTime.Now;
 * @endcode
 */

namespace MiniBlogApp.Models
{
    /**
     * @class Comment
     * @brief Represents a comment on a blog post.
     * 
     * @details This class stores the essential information for a comment:
     *          the author's username, the text content of the comment,
     *          and the timestamp when it was created. Each comment is linked
     *          to a specific post and can be displayed in chronological order.
     */
    public class Comment
    {
        /**
         * @brief Username of the comment author.
         * 
         * @details Identifies the user who wrote the comment. This should correspond
         *          to the username of a registered user in the system. It is used
         *          for displaying the author of the comment and for audit/logging purposes.
         */
        public string Author { get; set; } = string.Empty;

        /**
         * @brief Text content of the comment.
         * 
         * @details Stores the actual message provided by the user. This property 
         *          should contain meaningful text and can be validated to prevent 
         *          empty or malicious content. It is displayed in the comment section 
         *          under the corresponding post.
         */
        public string Text { get; set; } = string.Empty;

        /**
         * @brief Timestamp when the comment was created.
         * 
         * @details Initialized to the current date and time when the comment instance 
         *          is created. This property is essential for ordering comments 
         *          chronologically and for tracking when interactions occurred.
         */
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
