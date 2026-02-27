using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniBlogApp.Services
{
    /**
     * @file LoggerService.cs
     * @brief Logging framework for user actions in the blog.
     * @details
     * Provides a base abstract class ActionLogger and specialized loggers for posts, comments, and likes.
     * LoggerService stores all logs in memory and provides methods for adding, retrieving, and clearing logs.
     * Now implements IActivityLogger for Dependency Injection and uses locks for thread safety.
     */

    /**
     * @class ActionLogger
     * @brief Abstract base class for logging user actions.
     * @details Stores the username of the user performing the action and the timestamp.
     * Can be extended for specific actions like creating posts, commenting, or liking.
     */
    public abstract class ActionLogger
    {
        /** @brief Username of the user who performed the action. */
        public string Username { get; set; } = string.Empty;

        /** @brief Timestamp of when the action occurred. */
        public DateTime Timestamp { get; set; }

        /**
         * @brief Constructor for ActionLogger.
         * @param username Username of the user performing the action.
         * @details Sets the timestamp to current time.
         */
        protected ActionLogger(string username)
        {
            Username = username;
            Timestamp = DateTime.Now;
        }

        /**
         * @brief Returns a default message for the action.
         * @return string Message describing the action.
         */
        public virtual string GetMessage()
        {
            return $"{Timestamp:HH:mm} - User {Username} performed an action.";
        }
    }

    /**
     * @class PostLogger
     * @brief Logs creation of a post by a user.
     */
    public class PostLogger : ActionLogger
    {
        /** @brief Title of the created post. */
        public string PostTitle { get; set; }

        /**
         * @brief Constructor for PostLogger.
         * @param username Username of the post author.
         * @param postTitle Title of the post.
         */
        public PostLogger(string username, string postTitle) : base(username)
        {
            PostTitle = postTitle;
        }

        /** @inheritdoc */
        public override string GetMessage()
        {
            return $"{Timestamp:HH:mm} - User {Username} created post: '{PostTitle}'.";
        }
    }

    /**
     * @class CommentLogger
     * @brief Logs when a user leaves a comment.
     */
    public class CommentLogger : ActionLogger
    {
        /** @brief Text content of the comment. */
        public string CommentText { get; set; }

        /**
         * @brief Constructor for CommentLogger.
         * @param username Username of the commenter.
         * @param commentText Text of the comment.
         */
        public CommentLogger(string username, string commentText) : base(username)
        {
            CommentText = commentText;
        }

        /** @inheritdoc */
        public override string GetMessage()
        {
            return $"{Timestamp:HH:mm} - User {Username} left a comment: '{CommentText}'.";
        }
    }

    /**
     * @class LikeLogger
     * @brief Logs when a user likes a post.
     */
    public class LikeLogger : ActionLogger
    {
        /** @brief Title of the post that was liked. */
        public string PostTitle { get; set; }

        /**
         * @brief Constructor for LikeLogger.
         * @param username Username of the user who liked the post.
         * @param postTitle Title of the liked post.
         */
        public LikeLogger(string username, string postTitle) : base(username)
        {
            PostTitle = postTitle;
        }

        /** @inheritdoc */
        public override string GetMessage()
        {
            return $"{Timestamp:HH:mm} - User {Username} liked post: '{PostTitle}'.";
        }
    }

    /**
     * @class LoggerService
     * @brief Service for storing and retrieving user action logs.
     * @details
     * Stores all ActionLogger instances in memory safely. Allows adding, retrieving 
     * (sorted by timestamp descending), and clearing all logs. 
     * Implements IActivityLogger for DI.
     */
    public class LoggerService : IActivityLogger
    {
        /** @brief Об'єкт для блокування потоків (забезпечує Thread-Safety). */
        private readonly object _lock = new object();

        /** @brief Collection of all logged actions. */
        public List<ActionLogger> Logs { get; } = new();

        /**
         * @brief Adds a new log entry safely.
         * @param log ActionLogger instance representing the action.
         */
        public void AddLog(ActionLogger log)
        {
            lock (_lock)
            {
                Logs.Add(log);
            }
        }

        /**
         * @brief Retrieves all logs ordered by timestamp descending safely.
         * @return IEnumerable<ActionLogger> Collection of all logs sorted newest first.
         */
        public IEnumerable<ActionLogger> GetLogs()
        {
            lock (_lock)
            {
                return Logs.OrderByDescending(l => l.Timestamp).ToList();
            }
        }

        /**
         * @brief Clears all stored logs safely.
         */
        public void ClearAll()
        {
            lock (_lock)
            {
                Logs.Clear();
            }
        }
    }
}