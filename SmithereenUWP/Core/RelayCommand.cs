using System;
using System.Windows.Input;

namespace SmithereenUWP.Core
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;
        private readonly string _name;
        private readonly bool _isAccent = false;

        public event EventHandler CanExecuteChanged;

        public string Name => _name;
        public bool IsAccent => _isAccent;

        public RelayCommand(Action<object> execute, string name = null, bool isAccent = false)
            : this(execute, null, name, isAccent)
        {
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute, string name, bool isAccent)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            _execute = execute;
            _canExecute = canExecute;
            _name = name;
            _isAccent = isAccent;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}
