using MiniBlogApp.Composites;
using System.Collections.Generic;
using System.Text;

namespace MiniBlogApp.Models
{
    /// <summary>
    /// Клас коментаря, який реалізує патерн Composite.
    /// Він одночасно є і окремим об'єктом (коментарем), і контейнером для інших таких об'єктів (відповідей).
    /// </summary>
    public class Comment : ICommentComponent
    {
        public int Id { get; set; } // Додаємо ID
        public string Author { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public List<ICommentComponent> Replies { get; set; } = new List<ICommentComponent>();

        // Метод для додавання відповіді
        public void AddReply(ICommentComponent reply)
        {
            Replies.Add(reply);
        }

        // Рекурсивний підрахунок кількості всіх коментарів у цій гілці
        public int GetTotalCount()
        {
            int count = 1; // Рахуємо поточний коментар

            foreach (var reply in Replies)
            {
                count += reply.GetTotalCount(); // Магія патерну: рекурсивно рахуємо всі вкладені відповіді
            }

            return count;
        }

        // Рекурсивне форматування дерева коментарів
        public string Display(int indentLevel = 0)
        {
            string indent = new string('-', indentLevel * 2); // Створюємо відступ для візуалізації вкладеності
            var sb = new StringBuilder();

            sb.AppendLine($"{indent}> {Author}: {Text}");

            foreach (var reply in Replies)
            {
                // Рекурсивно викликаємо Display для всіх відповідей, збільшуючи рівень відступу
                sb.Append(reply.Display(indentLevel + 1));
            }

            return sb.ToString();
        }
    }
}