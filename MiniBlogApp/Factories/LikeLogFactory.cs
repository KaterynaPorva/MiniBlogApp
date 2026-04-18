using Microsoft.Extensions.Logging.Abstractions;
using MiniBlogApp.Services;

namespace MiniBlogApp.Factories
{
    public class LikeLogFactory : LogFactory
    {
        public override ILogEntry CreateLog(string user, string detail)
        {
            return new LikeLogger(user, detail); 
        }
    }
}