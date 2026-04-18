namespace MiniBlogApp.Observers
{
    /// <summary>
    /// Інтерфейс для всіх спостерігачів, які хочуть отримувати оновлення від блогу.
    /// </summary>
    public interface IBlogObserver
    {
        void Update(string author, string text);
    }
}