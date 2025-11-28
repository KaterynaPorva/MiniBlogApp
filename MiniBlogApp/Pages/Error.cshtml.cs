using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MiniBlogApp.Pages
{
    /**
     * @file Error.cshtml.cs
     * @brief Model for displaying application errors.
     *
     * @details This file contains the ErrorModel class used in MiniBlogApp.
     *          It provides detailed information about errors and a request identifier
     *          for diagnostics and logging purposes.
     *
     * @example Error.cshtml.cs
     * @code
     * var model = new ErrorModel(logger);
     * model.OnGet();
     * bool showId = model.ShowRequestId;
     * string? requestId = model.RequestId;
     * @endcode
     */
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class ErrorModel : PageModel
    {
        /**
         * @class ErrorModel
         * @brief Handles display of application errors.
         *
         * @details Provides a RequestId for tracking errors in logs and diagnostics.
         *          Indicates if the RequestId should be displayed to the user.
         */

        /**
         * @brief Identifier of the current request.
         * @details Used for tracking errors in logs and diagnostics. 
         *          Automatically set from Activity.Current.Id or HttpContext.TraceIdentifier.
         */
        public string? RequestId { get; set; }

        /**
         * @brief Indicates whether the RequestId should be displayed.
         * @return True if RequestId is not null or empty; otherwise, false.
         */
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        /**
         * @brief Logger for recording error information.
         * @details Allows writing error messages or diagnostics to configured logging providers.
         */
        private readonly ILogger<ErrorModel> _logger;

        /**
         * @brief Constructor for the ErrorModel.
         * @param logger Logger instance for recording error information.
         */
        public ErrorModel(ILogger<ErrorModel> logger)
        {
            _logger = logger;
        }

        /**
         * @brief Handles GET requests for the error page.
         * @details Sets the RequestId property to the current activity ID or HttpContext.TraceIdentifier.
         *          This value is used for error tracking and diagnostic purposes.
         */
        public void OnGet()
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        }
    }
}
