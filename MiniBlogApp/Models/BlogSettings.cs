namespace MiniBlogApp.Models
{
    /**
     * @class BlogSettings
     * @brief Реалізація патерну Singleton для керування глобальними налаштуваннями блогу.
     * @details Гарантує наявність лише одного екземпляра налаштувань у системі.
     */
    public sealed class BlogSettings
    {
        private static BlogSettings? _instance;
        private static readonly object _lock = new object();

        // Глобальні параметри блогу
        public string BlogName { get; set; } = "MiniBlogApp";
        public string FooterText { get; set; } = "© 2026 - MiniBlogApp";

        // 1. Приватний конструктор: неможливо створити об'єкт через new BlogSettings()
        private BlogSettings() { }

        /** * @brief Статична властивість для доступу до єдиного екземпляра (Thread-safe) 
         */
        public static BlogSettings Instance
        {
            get
            {
                // 2. Блокування для безпеки в багатопотоковому середовищі (Thread safety)
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new BlogSettings();
                    }
                    return _instance;
                }
            }
        }
    }
}