using MiniBlogApp.Services;

namespace MiniBlogApp.Factories
{
    /**
     * @brief Фабрика для створення логів нових постів.
     */
    public class PostLogFactory : LogFactory
    {
        public override ILogEntry CreateLog(string user, string detail)
        {
            return new PostLogger(user, detail);
        }
    }
}