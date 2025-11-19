/**
 * @file Like.cs
 * @brief Defines the Like model used for blog posts in the MiniBlogApp.
 *
 * @details This file contains the Like class which represents a "like" given 
 *          by a user to a specific blog post. Each like stores the username 
 *          of the user who liked the post. This information is useful for 
 *          displaying who liked a post, counting likes, and preventing 
 *          duplicate likes from the same user.
 * 
 * @example Like.cs
 * @details Example of creating a new Like instance and setting its properties.
 * @code
 * var like = new Like();
 * like.Username = "serhii";
 * @endcode
 */

namespace MiniBlogApp.Models
{
    /**
     * @class Like
     * @brief Represents a "like" on a blog post.
     * 
     * @details This class stores the username of the user who liked a specific post.
     *          Each like is associated with a user and a post. It can be used to
     *          count the number of likes, display which users liked a post, and
     *          prevent duplicate likes from the same user.
     */
    public class Like
    {
        /**
         * @brief Username of the user who liked the post.
         * 
         * @details Identifies which registered user added a like. 
         *          Must correspond to a valid username in the system. 
         *          Can be used for displaying likes and ensuring that each user 
         *          can like a post only once.
         */
        public string Username { get; set; } = string.Empty;
    }
}
