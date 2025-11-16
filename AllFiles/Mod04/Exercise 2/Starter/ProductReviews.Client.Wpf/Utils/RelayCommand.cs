using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProductReviews.Client.Utils
{
    public class RelayCommand : ICommand
    {
        private readonly Func<object, Task>? _execute;
        private readonly Predicate<object>? _canExecute;

        public RelayCommand(Func<object, Task> execute)
            : this(execute, null)
        {
        }
        public RelayCommand(Func<object, Task>? execute, Predicate<object>? canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }
        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute.Invoke(parameter!);
        }
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public void Execute(object? parameter)
        {
            _execute?.Invoke(parameter!);
        }
    }
}
