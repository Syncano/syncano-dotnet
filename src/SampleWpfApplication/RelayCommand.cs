using System;
using System.Windows.Input;

namespace SampleWpfApplication
{
    public class RelayCommand : ICommand
    {
        private readonly Action _commandAction;
        private readonly Func<bool> _canExecuteFunc;

        public RelayCommand(Action commandAction)
            : this(commandAction, () => true)
        {
        }

        public RelayCommand(Action commandAction, Func<bool> canExecuteFunc  )
        {
            _commandAction = commandAction;
            _canExecuteFunc = canExecuteFunc;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecuteFunc();
        }

        public void Execute(object parameter)
        {
            _commandAction();
        }

        public event EventHandler CanExecuteChanged;
    }
}