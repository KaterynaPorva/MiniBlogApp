using System.Diagnostics;

namespace MiniBlogApp.Observers
{
    public class CommentNotificationObserver : IBlogObserver
    {
        public void Update(string author, string text)
        {
            Debug.WriteLine($"[СПОstatus]: Користувач {author} залишив новий коментар: {text}");
        }
    }
}