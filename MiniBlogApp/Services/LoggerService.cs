namespace MiniBlogApp.Services
{
    public abstract class ActionLogger
    {
        public string Username { get; set; }
        public DateTime Timestamp { get; set; }

        protected ActionLogger(string username)
        {
            Username = username;
            Timestamp = DateTime.Now;
        }

        public virtual string GetMessage()
        {
            return $"{Timestamp:HH:mm} - Користувач {Username} виконав якусь дію.";
        }
    }

    public class PostLogger : ActionLogger
    {
        public string PostTitle { get; set; }

        public PostLogger(string username, string postTitle) : base(username)
        {
            PostTitle = postTitle;
        }

        public override string GetMessage()
        {
            return $"{Timestamp:HH:mm} - Користувач {Username} створив пост: '{PostTitle}'.";
        }
    }

    public class CommentLogger : ActionLogger
    {
        public string CommentText { get; set; }

        public CommentLogger(string username, string commentText) : base(username)
        {
            CommentText = commentText;
        }

        public override string GetMessage()
        {
            return $"{Timestamp:HH:mm} - Користувач {Username} залишив коментар: '{CommentText}'.";
        }
    }

    public class LikeLogger : ActionLogger
    {
        public string PostTitle { get; set; }

        public LikeLogger(string username, string postTitle) : base(username)
        {
            PostTitle = postTitle;
        }

        public override string GetMessage()
        {
            return $"{Timestamp:HH:mm} - Користувач {Username} поставив лайк посту: '{PostTitle}'.";
        }
    }

    public static class LoggerService
    {
        public static List<ActionLogger> Logs { get; } = new();
        public static void AddLog(ActionLogger log)
        {
            Logs.Add(log);
        }

        public static IEnumerable<ActionLogger> GetLogs() => Logs.OrderByDescending(l => l.Timestamp);
    }
}
