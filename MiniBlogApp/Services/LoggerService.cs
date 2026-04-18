using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniBlogApp.Services
{
    /**
     * @file LoggerService.cs
     * @brief Logging framework for user actions in the blog.
     * @details Оновлено для використання патерну Factory Method. 
     * Тепер всі логери реалізують єдиний інтерфейс ILogEntry.
     */

    /**
     * @class PostLogger
     * @brief Logs creation of a post by a user.
     */
    public class PostLogger : ILogEntry // ЗМІНЕНО НА ILogEntry
    {
        private readonly string _username;
        public string PostTitle { get; set; }

        public PostLogger(string username, string postTitle)
        {
            _username = username;
            PostTitle = postTitle;
        }

        public string GetMessage()
        {
            return $"[{DateTime.Now:HH:mm}] - User {_username} created post: '{PostTitle}'.";
        }
    }

    /**
     * @class CommentLogger
     * @brief Logs when a user leaves a comment.
     */
    public class CommentLogger : ILogEntry // ЗМІНЕНО НА ILogEntry
    {
        private readonly string _username;
        public string CommentText { get; set; }

        public CommentLogger(string username, string commentText)
        {
            _username = username;
            CommentText = commentText;
        }

        public string GetMessage()
        {
            return $"[{DateTime.Now:HH:mm}] - User {_username} left a comment: '{CommentText}'.";
        }
    }

    /**
     * @class LikeLogger
     * @brief Logs when a user likes a post.
     */
    public class LikeLogger : ILogEntry // ЗМІНЕНО НА ILogEntry
    {
        private readonly string _username;
        public string PostTitle { get; set; }

        public LikeLogger(string username, string postTitle)
        {
            _username = username;
            PostTitle = postTitle;
        }

        public string GetMessage()
        {
            return $"[{DateTime.Now:HH:mm}] - User {_username} liked post: '{PostTitle}'.";
        }
    }

    /**
     * @class LoggerService
     * @brief Service for storing and retrieving user action logs.
     * @details Тепер працює з інтерфейсом ILogEntry, що дозволяє легко додавати нові типи логів.
     */
    public class LoggerService : IActivityLogger
    {
        /** @brief Об'єкт для блокування потоків (забезпечує Thread-Safety). */
        private readonly object _lock = new object();

        /** @brief Collection of all logged actions (тепер зберігаємо готові рядки). */
        private readonly List<string> _logs = new();

        /**
         * @brief Adds a new log entry safely.
         * @param entry ILogEntry instance created by our Factories.
         */
        public void AddLog(ILogEntry entry) // ЗМІНЕНО ПАРАМЕТР НА ILogEntry
        {
            lock (_lock)
            {
                // Отримуємо рядок з логера і додаємо на початок списку (щоб нові були зверху)
                _logs.Insert(0, entry.GetMessage());
            }
        }

        /**
         * @brief Retrieves all logs safely.
         */
        public List<string> GetLogs()
        {
            lock (_lock)
            {
                return _logs.ToList();
            }
        }

        /**
         * @brief Clears all stored logs safely.
         */
        public void ClearAll()
        {
            lock (_lock)
            {
                _logs.Clear();
            }
        }
    }
}