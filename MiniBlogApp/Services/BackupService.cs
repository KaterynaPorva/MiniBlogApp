using System.IO;
using System.Threading.Tasks;

namespace MiniBlogApp.Services
{
    public class BackupService
    {
        // ПРОБЛЕМА БУЛА ТУТ: міняємо IBlogStorage на конкретний ConcurrentBlogStorage
        private readonly ConcurrentBlogStorage _storage;
        private readonly string _backupFilePath = "blog_backup.json";

        // Просимо DI дати нам одразу потокобезпечне сховище
        public BackupService(ConcurrentBlogStorage storage)
        {
            _storage = storage;
        }

        public async Task CreateBackupAsync()
        {
            // Тепер нам не треба робити приведення типів (cast), 
            // бо ми вже маємо правильний об'єкт!
            string json = _storage.ExportDataToJson();
            await File.WriteAllTextAsync(_backupFilePath, json);
        }

        public async Task RestoreFromBackupAsync()
        {
            if (File.Exists(_backupFilePath))
            {
                string json = await File.ReadAllTextAsync(_backupFilePath);
                _storage.ImportDataFromJson(json);
            }
        }
    }
}