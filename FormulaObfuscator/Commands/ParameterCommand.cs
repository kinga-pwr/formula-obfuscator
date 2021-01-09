using System;
using System.Windows.Input;

namespace FormulaObfuscator.Commands
{
    public class ParameterCommand<T> : ICommand
    {
        readonly Action<T> _execute;
        readonly Predicate<object> _canExecute;

        public ParameterCommand(Action<T> execute) : this(execute, null) { }
        public ParameterCommand(Action<T> execute, Predicate<object> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter) => _execute((T)parameter);
    }
}
