using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniBlogApp.Services;

namespace MiniBlogApp.Pages
{
    /**
     * @file Logs.cshtml.cs
     * @brief Page model for displaying and managing user activity logs.
     *
     * @details This file contains the LogsModel class used in MiniBlogApp.
     *          It handles retrieving and displaying the activity logs and 
     *          provides functionality to clear all logs. Useful for auditing 
     *          and monitoring user activity within the application.
     *
     * @example Logs.cshtml.cs
     * @code
     * var model = new LogsModel();
     * model.OnGet();
     * // model.Logs now contains all recorded user actions
     * IActionResult result = model.OnPostClearLogs();
     * // result redirects to the same page with log cleared
     * @endcode
     */
    public class LogsModel : PageModel
    {
        /**
         * @class LogsModel
         * @brief Handles displaying and managing user activity logs.
         *
         * @details Retrieves logs from LoggerService and allows clearing them.
         *          Intended for administrators or monitoring purposes.
         */

        /**
         * @brief Collection of action log entries.
         * @return List<ActionLogger> All recorded user actions.
         * @details Populated from LoggerService when OnGet is called.
         */
        public List<ActionLogger> Logs { get; set; } = new();

        /**
         * @brief Handles GET requests to display the activity log.
         * @details Loads all log entries from LoggerService and assigns them to the Logs property.
         */
        public void OnGet()
        {
            Logs = LoggerService.GetLogs().ToList();
        }

        /**
         * @brief Handles POST requests to clear the activity log.
         * @details Clears all entries in the activity log using LoggerService.
         *          Displays a success message using TempData and redirects to the same page.
         * @return IActionResult Redirects to the current page after clearing the log.
         */
        public IActionResult OnPostClearLogs()
        {
            LoggerService.ClearAll();
            TempData["Message"] = "Журнал активності очищено.";
            return RedirectToPage();
        }
    }
}
