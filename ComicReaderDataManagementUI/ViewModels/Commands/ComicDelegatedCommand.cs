using ComicReaderClassLibrary.DataAccess.DataModels;
using ComicReaderClassLibrary.DataAccess.Implementations;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace ComicReaderDataManagementUI.ViewModels.Commands
{
    internal class ComicDelegatedCommand : ICommand
    {
        public ComicDelegatedCommand(Action execute)
        {
            _execute = execute;
        }

        private Action _execute { get; set; }

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