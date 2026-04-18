using MiniBlogApp.Services; // Обов'язково для ILogEntry

namespace MiniBlogApp.Factories
{
    /**
     * @brief Абстрактна фабрика для створення логів.
     */
    public abstract class LogFactory
    {
        public abstract ILogEntry CreateLog(string user, string detail);
    }
}