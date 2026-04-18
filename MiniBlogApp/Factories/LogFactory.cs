using Microsoft.Extensions.Logging.Abstractions;
using MiniBlogApp.Services;

namespace MiniBlogApp.Factories
{
    public abstract class LogFactory
    {
        // Це Фабричний метод. 
        public abstract ILogEntry CreateLog(string user, string detail);
    }
}