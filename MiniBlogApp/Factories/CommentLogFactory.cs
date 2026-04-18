using Microsoft.Extensions.Logging.Abstractions;
using MiniBlogApp.Services;

namespace MiniBlogApp.Factories
{
    public class CommentLogFactory : LogFactory
    {
        public override ILogEntry CreateLog(string user, string detail)
        {
            return new CommentLogger(user, detail);
        }
    }
}