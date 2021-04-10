using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace UI
{
    public class AsyncCommand : ICommand
    {
        public Func<object, Task> ExecuteFunction { get; }
        public Predicate<object> CanExecutePredicate { get; }

        public event EventHandler CanExecuteChanged;

        public void UpdateCanExecute() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        public bool IsWorking { get; private set; }

        public AsyncCommand(Func<object, Task> executeFunction) : this(executeFunction, (obj) => true) { }

        public AsyncCommand(Func<object, Task> executeFunction, Predicate<object> canExecutePredicate)
        {
            ExecuteFunction = executeFunction;
            CanExecutePredicate = canExecutePredicate;
        }

        public bool CanExecute(object parameter) => !IsWorking && (CanExecutePredicate?.Invoke(parameter) ?? true);

        public void Execute(object parameter)
        {
            IsWorking = true;
            UpdateCanExecute();

            ExecuteFunction(parameter);

            IsWorking = false;
            UpdateCanExecute();
        }
    }
}
