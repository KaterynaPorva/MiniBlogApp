using MiniBlogApp.Services; // Щоб бачити ILogEntry та CommentLogger
using MiniBlogApp.Factories;

namespace MiniBlogApp.Factories
{
    public class CommentLogFactory : LogFactory
    {
        public override ILogEntry CreateLog(string user, string detail)
        {
            // CommentLogger має лежати в Services і реалізувати ILogEntry
            return new CommentLogger(user, detail);
        }
    }
}