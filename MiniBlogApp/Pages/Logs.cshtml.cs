using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniBlogApp.Models;
using MiniBlogApp.Services;
using NToastNotify;

namespace MiniBlogApp.Pages
{
    /**
     * @file Logs.cshtml.cs
     * @brief Page model for displaying and managing user activity logs.
     *
     * @details This file contains the LogsModel class used in MiniBlogApp.
     * It handles retrieving and displaying the activity logs and 
     * provides functionality to clear all logs. Useful for auditing 
     * and monitoring user activity within the application.
     * Now uses NToastNotify for user feedback and Dependency Injection for logging.
     */
    public class LogsModel : PageModel
    {
        private readonly IToastNotification _toastNotification; 
        private readonly IActivityLogger _activityLogger; // 1. Додаємо сервіс логування

        /**
         * @brief Constructor for LogsModel.
         * @param toastNotification Injected service for displaying notifications.
         * @param activityLogger Injected service for managing logs.
         */
        public LogsModel(IToastNotification toastNotification, IActivityLogger activityLogger)
        {
            _toastNotification = toastNotification;
            _activityLogger = activityLogger; // 2. Ініціалізуємо
        }

        /**
         * @brief Collection of action log entries.
         * @return List<ActionLogger> All recorded user actions.
         */
        public List<ActionLogger> Logs { get; set; } = new();

        /**
         * @brief Handles GET requests to display the activity log.
         * @details Loads all log entries from IActivityLogger and assigns them to the Logs property.
         */
        public void OnGet()
        {
            // 3. Використовуємо інжектований об'єкт
            Logs = _activityLogger.GetLogs().ToList();
        }

        /**
         * @brief Handles POST requests to clear the activity log.
         * @details Clears all entries in the activity log using IActivityLogger.
         * Displays a success toast message and redirects to the same page.
         * @return IActionResult Redirects to the current page after clearing the log.
         */
        public IActionResult OnPostClearLogs()
        {
            // 4. Очищуємо через екземпляр сервісу
            _activityLogger.ClearAll();
            
            _toastNotification.AddSuccessToastMessage("Журнал активності успішно очищено. 🧹");
            
            return RedirectToPage();
        }
    }
}