namespace MiniBlogApp.Composites
{
    /// <summary>
    /// Загальний інтерфейс для патерну Composite.
    /// Описує операції, які можна виконувати як над одним коментарем, так і над гілкою коментарів.
    /// </summary>
    public interface ICommentComponent
    {
        /// <summary>
        /// Рекурсивно рахує кількість коментарів у цій гілці.
        /// </summary>
        int GetTotalCount();

        /// <summary>
        /// Повертає відформатований текст коментаря з урахуванням вкладеності (відступів).
        /// </summary>
        string Display(int indentLevel = 0);
    }
}