using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ComicReaderDataManagementUI.ViewModels.Commands
{
    internal class ComicDelegatedCommandAsync : INotifyPropertyChanged, ICommand
    {
        private readonly Func<Task> _execute;
        private readonly Func<bool> _canExecute;

        public ComicDelegatedCommandAsync(Func<Task> execute) : this(execute, () => true) { }

        public ComicDelegatedCommandAsync(Func<Task> execute, Func<bool> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        private bool _isExecuting;

        public bool IsExecuting
        {
            get => _isExecuting;
            set
            {
                _isExecuting = value;
                RaisePropertyChanged();
            }
        }

        public event EventHandler CanExecuteChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        public bool CanExecute(object parameter) => !IsExecuting
                                                        && (_canExecute == null || _canExecute());

        public async void Execute(object parameter)
        {
            if (!CanExecute(parameter))
                return;

            IsExecuting = true;
            try
            {
                await _execute().ConfigureAwait(true);
            }
            finally
            {
                IsExecuting = false;
            }
        }

        public void RaisePropertyChanged() => PropertyChanged?.Invoke(this, null);
        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, null);
    }
}
