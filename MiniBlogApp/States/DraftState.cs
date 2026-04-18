using MiniBlogApp.Models;
using System;

namespace MiniBlogApp.States
{
    public class DraftState : IPostState
    {
        public void SubmitForReview(Post post)
        {
            post.State = new ModerationState();
        }

        public void Approve(Post post) => throw new InvalidOperationException("Не можна опублікувати чернетку без модерації.");

        public void Reject(Post post) => throw new InvalidOperationException("Чернетка ще не на модерації, її не можна відхилити.");

        public string GetStatusName() => "📝 Чернетка";
    }
}