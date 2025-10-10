using MiniBlogApp.Models;

namespace MiniBlogApp.Services
{
    public static class BlogStorage
    {
        public static List<Post> Posts { get; } = new();
        private static int _nextId = 1;

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

        public static void UpdatePost(int id, string title, string content)
        {
            var post = GetPostById(id);
            if (post != null)
            {
                post.Title = title;
                post.Content = content;
            }


        }

        public static void DeletePost(int id)
        {
            var post = GetPostById(id);
            if (post != null)
                Posts.Remove(post);
        }

        public static IEnumerable<Post> GetAllPosts() => Posts.OrderByDescending(p => p.CreatedAt);

        public static IEnumerable<Post> GetPostsByUser(string username) =>
            Posts.Where(p => p.Author == username).OrderByDescending(p => p.CreatedAt);

        public static Post? GetPostById(int id) => Posts.FirstOrDefault(p => p.Id == id);

        public static void AddLike(int postId, string username)
        {
            var post = GetPostById(postId);
            if (post == null) return;

            if (!post.Likes.Any(l => l.Username == username))
                post.Likes.Add(new Like { Username = username });

            LoggerService.AddLog(new LikeLogger(username, post.Title));
        }

        public static void AddComment(int postId, string author, string text)
        {
            var post = GetPostById(postId);
            if (post == null) return;

            post.Comments.Add(new Comment { Author = author, Text = text });

            LoggerService.AddLog(new CommentLogger(author, text));
        }
    }
}
