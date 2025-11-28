using MiniBlogApp.Models;

namespace MiniBlogApp.Services
{
    /**
     * @file BlogStorage.cs
     * @class BlogStorage
     * @brief Static storage for blog posts and related operations.
     * @details
     * This static class provides in-memory storage for all blog posts, 
     * manages unique IDs for new posts, and handles creation, updating, 
     * deletion, retrieval, adding likes, and comments. All actions are logged
     * via LoggerService. Being static, it serves as a global repository for posts.
     * 
     * @example BlogStorage.cs
     * @code
     * // Adding a new post
     * var post = BlogStorage.AddPost("serhii", "My Post", "This is the content");
     * 
     * // Updating a post
     * BlogStorage.UpdatePost(post.Id, "Updated Title", "Updated Content");
     *
     * // Deleting a post
     * BlogStorage.DeletePost(post.Id);
     *
     * // Retrieving posts
     * var allPosts = BlogStorage.GetAllPosts();
     * var userPosts = BlogStorage.GetPostsByUser("serhii");
     * var singlePost = BlogStorage.GetPostById(post.Id);
     *
     * // Adding a like
     * BlogStorage.AddLike(post.Id, "user1");
     *
     * // Adding a comment
     * BlogStorage.AddComment(post.Id, "user1", "Nice post!");
     * @endcode
     */

    public static class BlogStorage
    {
        /**
         * @brief List of all posts in the blog.
         * @details
         * Stores instances of Post added to the blog. Can be accessed globally
         * for retrieving, enumerating, or processing posts.
         */
        public static List<Post> Posts { get; } = new();

        /**
         * @brief Next ID to assign to a new post.
         * @details
         * Ensures each new post receives a unique ID. Incremented automatically
         * when a post is added. Acts as a global counter for Post IDs.
         */
        private static int _nextId = 1;

        /**
         * @brief Adds a new post to the storage.
         * @param author Username of the post author.
         * @param title Title of the post.
         * @param content Content of the post.
         * @return Post The newly created post object.
         * @details
         * Increments the internal ID counter, adds the post to the Posts list,
         * and logs the creation via PostLogger.
         */
        public static Post AddPost(string author, string title, string content)
        {
            var post = new Post
            {
                Id = _nextId++,
                Author = author,
                Title = title,
                Content = content,
                CreatedAt = DateTime.Now
            };
            Posts.Add(post);

            LoggerService.AddLog(new PostLogger(author, title));

            return post;
        }

        /**
         * @brief Updates an existing post by ID.
         * @param id ID of the post to update.
         * @param title New title for the post.
         * @param content New content for the post.
         * @return Post? The updated post or null if no post with the specified ID exists.
         * @details
         * Searches for a post by its ID. If found, updates its title and content.
         * Otherwise, returns null.
         */
        public static Post? UpdatePost(int id, string title, string content)
        {
            var post = GetPostById(id);
            if (post != null)
            {
                post.Title = title;
                post.Content = content;
                return post;
            }
            return null;
        }

        /**
         * @brief Deletes a post by ID.
         * @param id ID of the post to delete.
         * @details
         * Searches for a post by its ID and removes it from the Posts list if it exists.
         */
        public static void DeletePost(int id)
        {
            var post = GetPostById(id);
            if (post != null)
                Posts.Remove(post);
        }

        /**
         * @brief Returns all posts sorted by creation date (newest first).
         * @return IEnumerable<Post> Collection of all posts in descending order of creation.
         */
        public static IEnumerable<Post> GetAllPosts() => Posts.OrderByDescending(p => p.CreatedAt);

        /**
         * @brief Returns all posts created by a specific user.
         * @param username Username of the user whose posts are being retrieved.
         * @return IEnumerable<Post> Collection of posts by the specified user, ordered by newest first.
         */
        public static IEnumerable<Post> GetPostsByUser(string username) =>
            Posts.Where(p => p.Author == username).OrderByDescending(p => p.CreatedAt);

        /**
         * @brief Retrieves a post by its ID.
         * @param id ID of the post to retrieve.
         * @return Post? The post if found, or null if no post with the given ID exists.
         * @details
         * Searches the Posts list for a post with the specified ID.
         */
        public static Post? GetPostById(int id) => Posts.FirstOrDefault(p => p.Id == id);

        /**
         * @brief Adds a like to a post.
         * @param postId ID of the post to like.
         * @param username Username of the user liking the post.
         * @details
         * Only adds a like if the user has not already liked the post.
         * Logs the action via LikeLogger for tracking.
         */
        public static void AddLike(int postId, string username)
        {
            var post = GetPostById(postId);
            if (post == null) return;

            if (!post.Likes.Any(l => l.Username == username))
            {
                post.Likes.Add(new Like { Username = username });
                LoggerService.AddLog(new LikeLogger(username, post.Title));
            }
        }

        /**
         * @brief Adds a comment to a post.
         * @param postId ID of the post to comment on.
         * @param author Username of the comment author.
         * @param text Text content of the comment.
         * @details
         * Adds a new Comment object to the post's Comments collection.
         * Logs the action using CommentLogger for auditing.
         */
        public static void AddComment(int postId, string author, string text)
        {
            var post = GetPostById(postId);
            if (post == null) return;

            post.Comments.Add(new Comment { Author = author, Text = text });
            LoggerService.AddLog(new CommentLogger(author, text));
        }
    }
}
