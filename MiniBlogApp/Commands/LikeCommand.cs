using MiniBlogApp.Services;

namespace MiniBlogApp.Commands
{
    public class LikeCommand : ICommand
    {
        private readonly IBlogStorage _storage;
        private readonly int _postId;
        private readonly string _username;

        public LikeCommand(IBlogStorage storage, int postId, string username)
        {
            _storage = storage;
            _postId = postId;
            _username = username;
        }

        public void Execute()
        {
            _storage.AddLike(_postId, _username);
        }

        public void Undo()
        {
            _storage.RemoveLike(_postId, _username);
        }
    }
}