using System;
using System.Collections.Generic;
using MiniBlogApp.States; // Підключаємо інтерфейс станів

namespace MiniBlogApp.Models
{
    /**
     * @class Post
     * @brief Основна модель поста, яка керує своєю поведінкою через патерн State.
     */
    public class Post
    {
        // --- 1. ВЛАСТИВОСТІ ДАНИХ (Data Properties) ---
        // Ці поля потрібні для роботи Builder, Facade та відображення на сторінках

        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Колекції для лайків та коментарів (потрібні для Composite/Decorator)
        public List<Like> Likes { get; set; } = new List<Like>();
        public List<Comment> Comments { get; set; } = new List<Comment>();

        // ⬇️ ДОДАНО: Властивість, якої не вистачало для усунення останніх 2 помилок! ⬇️
        public int TotalCommentsCount { get; set; }

        // --- 2. ПАТЕРН STATE (Логіка станів) ---

        /** * @brief Поточний стан об'єкта.
         * @details Контекст (Post) делегує виконання дій цьому об'єкту стану.
         */
        public IPostState State { get; set; }

        /**
         * @brief Конструктор за замовчуванням.
         * @details Встановлює початковий стан — "Чернетка".
         */
        public Post()
        {
            State = new DraftState();

            // Ініціалізація списків, щоб уникнути NullReferenceException
            Likes = new List<Like>();
            Comments = new List<Comment>();
        }

        // --- 3. МЕТОДИ ПЕРЕХОДУ (Delegation methods) ---
        // Ці методи просто викликають відповідні дії у поточного стану

        /** @brief Крок: Чернетка -> Модерація */
        public void SubmitForReview() => State.SubmitForReview(this);

        /** @brief Крок: Модерація -> Опубліковано */
        public void Approve() => State.Approve(this);

        /** @brief Крок: Модерація/Опубліковано -> Чернетка (Відхилення) */
        public void Reject() => State.Reject(this);

        /** @brief Отримання текстового статусу для виводу в UI */
        public string GetCurrentStatus() => State.GetStatusName();
    }
}