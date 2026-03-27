namespace MiniBlogApp.Services
{
    /**
     * @interface IMarkdownParser
     * @brief Інтерфейс для патерну Adapter. Визначає контракт для парсингу Markdown.
     */
    public interface IMarkdownParser
    {
        string Parse(string markdown);
    }

    /**
     * @class MarkdigAdapter
     * @brief Адаптує функціонал сторонньої бібліотеки Markdig до нашого інтерфейсу.
     */
    public class MarkdigAdapter : IMarkdownParser
    {
        public string Parse(string markdown)
        {
            // Викликаємо метод зовнішньої бібліотеки
            return Markdig.Markdown.ToHtml(markdown ?? string.Empty);
        }
    }
}