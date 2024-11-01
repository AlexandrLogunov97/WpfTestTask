using System.Windows.Input;

namespace TestTask.Base
{
    public class Command : ICommand
    {
        public Command(Action<object?>? execute, Func<object?, bool>? canExecute = null)
        {
            this._execute = execute;
            this._canExecute = canExecute;
        }

        public Command(Action<object?>? execute) : this(execute, null) { }

        public bool CanExecute(object? parameter)
        {
            return this._canExecute == null || this._canExecute(parameter);
        }

        public void Execute(object? parameter)
        {
            this._execute?.Invoke(parameter);
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }

            remove { CommandManager.RequerySuggested -= value; }
        }

        private Action<object?>? _execute;

        private Func<object?, bool>? _canExecute;
    }
}
