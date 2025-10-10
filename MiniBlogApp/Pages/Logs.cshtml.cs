using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniBlogApp.Services;

namespace MiniBlogApp.Pages
{
    public class LogsModel : PageModel
    {
        public List<ActionLogger> Logs { get; set; } = new();

        public void OnGet()
        {
            Logs = LoggerService.GetLogs().ToList();
        }
    }
}
