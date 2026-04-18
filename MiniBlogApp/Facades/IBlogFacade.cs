using MiniBlogApp.Models;

namespace MiniBlogApp.Facades
{
    public interface IBlogFacade
    {
        Post? GetPostForView(int id);
        void AddLike(int postId, string username);

        // ДОДАЄМО parentCommentId ЯК НЕОБОВ'ЯЗКОВИЙ ПАРАМЕТР
        void AddComment(int postId, string username, string text, int? parentCommentId = null);
    }
}