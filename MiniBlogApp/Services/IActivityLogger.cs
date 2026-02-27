using System.Collections.Generic;

namespace MiniBlogApp.Services
{
    public interface IActivityLogger
    {
        void AddLog(ActionLogger log);
        IEnumerable<ActionLogger> GetLogs();
        void ClearAll();
    }
}