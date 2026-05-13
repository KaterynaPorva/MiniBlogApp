using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniBlogApp.Services;
using NToastNotify;
using System.Threading.Tasks;

namespace MiniBlogApp.Pages
{
    public class SettingsModel : PageModel
    {
        private readonly BackupService _backupService;
        private readonly IToastNotification _toastNotification;

        // Впроваджуємо залежності (Dependency Injection)
        public SettingsModel(BackupService backupService, IToastNotification toastNotification)
        {
            _backupService = backupService;
            _toastNotification = toastNotification;
        }

        public void OnGet()
        {
            // Метод спрацьовує при звичайному відкритті сторінки
        }

        // Спрацює при натисканні кнопки "Зберегти"
        public async Task<IActionResult> OnPostCreateBackupAsync()
        {
            await _backupService.CreateBackupAsync();

            // Показуємо красиве повідомлення
            _toastNotification.AddSuccessToastMessage("Бекап успішно створено та збережено на сервері!");

            return RedirectToPage(); // Перезавантажуємо сторінку
        }

        // Спрацює при натисканні кнопки "Відновити"
        public async Task<IActionResult> OnPostRestoreBackupAsync()
        {
            await _backupService.RestoreFromBackupAsync();

            // Показуємо повідомлення про успішне злиття
            _toastNotification.AddInfoToastMessage("Дані успішно відновлено та об'єднано з поточними!");

            return RedirectToPage();
        }
    }
}