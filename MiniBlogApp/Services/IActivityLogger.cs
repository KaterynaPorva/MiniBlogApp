using System.Collections.Generic;

namespace MiniBlogApp.Services
{
    public interface IActivityLogger
    {
        void AddLog(ILogEntry entry);
        List<string> GetLogs();
        void ClearAll();
    }
}