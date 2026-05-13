using System.IO;
using System.Threading.Tasks;

namespace MiniBlogApp.Services
{
    public class BackupService
    {
        private readonly ConcurrentBlogStorage _storage;
        private readonly string _backupFilePath = "blog_backup.json";

        public BackupService(ConcurrentBlogStorage storage)
        {
            _storage = storage;
        }

        public async Task CreateBackupAsync()
        {

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