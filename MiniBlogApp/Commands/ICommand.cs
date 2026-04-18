namespace MiniBlogApp.Commands
{
    public interface ICommand
    {
        void Execute(); // Виконати дію
        void Undo();    // Скасувати дію
    }
}