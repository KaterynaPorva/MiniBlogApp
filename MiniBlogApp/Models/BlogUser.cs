/**
 * @file BlogUser.cs
 * @brief Defines the BlogUser model used for authentication in the MiniBlogApp.
 *
 * @details This file contains the basic user representation used across the 
 *          application. The BlogUser class holds the credentials required 
 *          for login functionality. It currently supports only the username 
 *          and password fields but can be expanded in the future to include 
 *          roles, profile details, account status, and other security-related 
 *          attributes if needed.
 *
 * @example BlogUser.cs
 * @details Example of creating a new BlogUser instance and setting its properties.
 * @code
 * var user = new BlogUser();
 * user.Username = "serhii";
 * user.Password = "securepassword123";
 * @endcode
 */

namespace MiniBlogApp.Models
{
    /**
     * @class BlogUser
     * @brief Represents a user of the blog application.
     * 
     * @details This class stores the simplest possible form of user information.
     *          It is used primarily for login validation and session tracking.
     *          Although minimal by design for the laboratory context, it serves 
     *          as a foundation for extending authentication and user management 
     *          features, such as adding email verification, password recovery, 
     *          or user permissions.
     */
    public class BlogUser
    {
        /**
         * @brief Username of the blog user.
         * 
         * @details The username acts as the primary identifier for the user.
         *          It must be unique across the system to distinguish users 
         *          during authentication, authorization, or when associating 
         *          content such as posts, likes, or comments. If the username 
         *          is invalid or duplicated, user authentication may fail.
         */
        public string Username { get; set; } = string.Empty;

        /**
         * @brief Password of the blog user.
         * 
         * @details The stored password is currently in plain text due to simplicity 
         *          requirements of the project environment. In real-world systems, 
         *          passwords must always be stored securely using hashing (e.g., 
         *          bcrypt, PBKDF2) and salted values. This property is used during 
         *          login to verify the user's identity.
         */
        public string Password { get; set; } = string.Empty;
    }
}
