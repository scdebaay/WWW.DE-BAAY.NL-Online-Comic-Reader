using ComicReaderClassLibrary.DataAccess.DataModels;
using ComicReaderClassLibrary.DataAccess.Implementations;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace ComicReaderDataManagementUI.ViewModels.Commands
{
    internal class ComicDelegatedCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public ComicDelegatedCommand(Action execute) : this(execute, () => true) { }
        public ComicDelegatedCommand(Action execute, Func<bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute())
                return true;

            return false;
        }

        public void Execute(object parameter)
        {
            _execute.Invoke();
        }
    }

    internal class ParameteredComicDelegatedCommand : ICommand
    {
        public ParameteredComicDelegatedCommand(Action<object> execute)
        {
            _execute = execute;
        }

        private Action<object> _execute { get; set; }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            if (parameter != null)
                return true;

            return false;
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}