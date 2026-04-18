using MiniBlogApp.Models;
using System;

namespace MiniBlogApp.States
{
    public class ModerationState : IPostState
    {
        public void SubmitForReview(Post post) => throw new InvalidOperationException("Пост вже на модерації.");

        public void Approve(Post post)
        {
            // Модератор схвалив -> публікуємо
            post.State = new PublishedState();
        }

        public void Reject(Post post)
        {
            // Модератор відхилив -> повертаємо в чернетки
            post.State = new DraftState();
        }

        public string GetStatusName() => "⏳ На модерації";
    }
}