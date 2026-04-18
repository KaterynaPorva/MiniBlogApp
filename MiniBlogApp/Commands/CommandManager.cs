using System.Collections.Generic;

namespace MiniBlogApp.Commands
{
    public class CommandManager
    {
        private readonly Stack<ICommand> _history = new Stack<ICommand>();

        public void ExecuteCommand(ICommand command)
        {
            command.Execute();
            _history.Push(command); // Зберігаємо в історію
        }

        public void UndoLastCommand()
        {
            if (_history.Count > 0)
            {
                var command = _history.Pop();
                command.Undo();
            }
        }

        public bool CanUndo => _history.Count > 0;
    }
}