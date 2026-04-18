using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniBlogApp.Services
{
    /**
     * @class BaseLogEntry
     * @brief ПАТЕРН TEMPLATE METHOD (Шаблонний метод).
     * @details Цей клас визначає загальний алгоритм (шаблон) форматування логу.
     */
    public abstract class BaseLogEntry : ILogEntry
    {
        protected readonly string _username;

        protected BaseLogEntry(string username)
        {
            _username = username;
        }

        /**
         * @brief Шаблонний метод (Template Method).
         * @details Визначає жорсткий каркас повідомлення. Підкласи не можуть змінити 
         * порядок виклику цих кроків, але можуть змінити самі кроки.
         */
        public string GetMessage()
        {
            // Каркас алгоритму: [Час] Користувач @Ім'я [Дія]: [Деталі]
            return $"[{GetTimestamp()}] Користувач @{_username.ToUpper()} {GetActionName()}: {GetActionDetails()}";
        }

        /** * @brief Хук (Hook). Має реалізацію за замовчуванням, але може бути змінений.
         */
        protected virtual string GetTimestamp() => DateTime.Now.ToString("HH:mm:ss");

        /** @brief Абстрактний крок. Підкласи ОБОВ'ЯЗКОВО мають його реалізувати. */
        protected abstract string GetActionName();

        /** @brief Абстрактний крок. Підкласи ОБОВ'ЯЗКОВО мають його реалізувати. */
        protected abstract string GetActionDetails();
    }

    // --- КОНКРЕТНІ РЕАЛІЗАЦІЇ (КРОКИ АЛГОРИТМУ) ---

    public class PostLogger : BaseLogEntry
    {
        private readonly string _postTitle;

        public PostLogger(string username, string postTitle) : base(username)
            => _postTitle = postTitle;

        protected override string GetActionName() => "створив новий пост";
        protected override string GetActionDetails() => $"«{_postTitle}»";
    }

    public class CommentLogger : BaseLogEntry
    {
        private readonly string _commentText;

        public CommentLogger(string username, string commentText) : base(username)
            => _commentText = commentText;

        protected override string GetActionName() => "залишив коментар";
        protected override string GetActionDetails() => $"«{_commentText}»";
    }

    public class LikeLogger : BaseLogEntry
    {
        private readonly string _postTitle;

        public LikeLogger(string username, string postTitle) : base(username)
            => _postTitle = postTitle;

        protected override string GetActionName() => "поставив лайк на пост";
        protected override string GetActionDetails() => $"«{_postTitle}»";
    }

    // --- САМ СЕРВІС ЛОГУВАННЯ (Без змін, він і так працює ідеально) ---

    public class LoggerService : IActivityLogger
    {
        private readonly object _lock = new object();
        private readonly List<string> _logs = new();

        public void AddLog(ILogEntry entry)
        {
            lock (_lock)
            {
                // Тут викликається наш Шаблонний метод GetMessage()
                _logs.Insert(0, entry.GetMessage());
            }
        }

        public List<string> GetLogs()
        {
            lock (_lock)
            {
                return _logs.ToList();
            }
        }

        public void ClearAll()
        {
            lock (_lock)
            {
                _logs.Clear();
            }
        }
    }
}