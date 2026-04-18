using MiniBlogApp.Models;
using System;

namespace MiniBlogApp.States
{
    public class PublishedState : IPostState
    {
        public void SubmitForReview(Post post) => throw new InvalidOperationException("Пост вже опубліковано.");

        public void Approve(Post post) => throw new InvalidOperationException("Пост вже опубліковано.");

        // Якщо треба зняти з публікації, повертаємо в чернетки
        public void Reject(Post post)
        {
            post.State = new DraftState();
        }

        public string GetStatusName() => "✅ Опубліковано";
    }
}