using Caliburn.Micro;
using ComicReaderClassLibrary.DataAccess.DataModels;
using ComicReaderDataManagementUI.Events;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ComicReaderDataManagementUI.ViewModels
{
    public interface ILanguageViewModel
    {
        /// <summary>
        /// Backing field for the Statusbar label
        /// </summary>
        public string StatusBar { get; }
        /// <summary>
        /// Command to add Comis to Language
        /// </summary>
        public ICommand AddComicToLanguageCommand { get; }
        /// <summary>
        /// Command for the New Language button
        /// </summary>
        public ICommand NewLanguageCommand { get; }
        /// <summary>
        /// Command for the Save Language button
        /// </summary>
        public ICommand SaveLanguageCommand { get; }
        /// <summary>
        /// Command for the Delete Language button
        /// </summary>
        public ICommand DeleteLanguageCommand { get; }
        /// <summary>
        /// Backing field for the Comic dropdown
        /// </summary>
        BindableCollection<ComicDataModel> ComicList { get; }
        /// <summary>
        /// Backing field for the Comics in Language listbox
        /// </summary>
        BindableCollection<ComicDataModel> ComicsInLanguageBox { get; }
        /// <summary>
        /// Backing field for the Language dropdown
        /// </summary>
        BindableCollection<LanguageDataModel> LanguageBox { get; }
        /// <summary>
        /// Backing field for the Language term textbox
        /// </summary>
        string LanguageName { get; set; }
        /// <summary>
        /// Backing field for the Selected Comic in the Comic in Language Listbox
        /// </summary>
        ComicDataModel SelectedComicInLanguage { get; set; }
        /// <summary>
        /// Backing field for the Selected Language in the Language dropdown
        /// </summary>
        LanguageDataModel SelectedItem { get; set; }
        /// <summary>
        /// Event handler for when a Comic was changed in the MainViewModel
        /// </summary>
        Task HandleAsync(ComicChangedOnMainEvent message, CancellationToken cancellationToken);
    }
}