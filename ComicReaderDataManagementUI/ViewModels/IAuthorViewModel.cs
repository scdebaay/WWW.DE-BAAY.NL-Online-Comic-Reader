using Caliburn.Micro;
using ComicReaderClassLibrary.DataAccess.DataModels;
using ComicReaderDataManagementUI.Events;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ComicReaderDataManagementUI.ViewModels
{
    /// <summary>
    /// Interface for the Author ViewModel.
    /// </summary>
    public interface IAuthorViewModel
    {
        /// <summary>
        /// Backing field for the Author drop downbox
        /// </summary>
        BindableCollection<AuthorDataModel> AuthorBox { get; }
        /// <summary>
        /// Backing field for the Comic Listbox
        /// </summary>
        BindableCollection<ComicDataModel> ComicList { get; }
        /// <summary>
        /// Backing field for the Comic by Author Listbox
        /// </summary>
        BindableCollection<ComicDataModel> ComicsByAuthorBox { get; }
        /// <summary>
        /// Backing field for the Firstname textbox
        /// </summary>
        string FirstName { get; set; }
        /// <summary>
        /// Backing field for the Lastname textbox
        /// </summary>
        string LastName { get; set; }
        /// <summary>
        /// Backing field for the Middlename textbox
        /// </summary>
        string MiddleName { get; set; }
        /// <summary>
        /// Backing field for the Datebirth DateTime picker
        /// </summary>
        public DateTime DateBirth { get; set; }
        /// <summary>
        /// Backing field for the Datedeceased DateTime picker
        /// </summary>
        public DateTime DateDeceased { get; set; }
        /// <summary>
        /// Backing field for the selected comic in the Comic by author Listbox
        /// </summary>
        ComicDataModel SelectedComicByAuthor { get; set; }
        /// <summary>
        /// Backing field for the selected author in the author Listbox
        /// </summary>
        AuthorDataModel SelectedItem { get; set; }
        /// <summary>
        /// Backing field for the statusbar label
        /// </summary>
        string StatusBar { get; set; }
        /// <summary>
        /// Command to add Comics to Author
        /// </summary>
        ICommand AddComicToAuthorCommand { get; }
        /// <summary>
        /// Method for delete author button.
        /// </summary>
        void DeleteAuthor();
        /// <summary>
        /// Eventhandler for Changed comic in MainView
        /// </summary>
        Task HandleAsync(ComicChangedOnMainEvent message, CancellationToken cancellationToken);
        /// <summary>
        /// Method for new author button.
        /// </summary>
        void NewAuthor();
        /// <summary>
        /// Method for save author button.
        /// </summary>
        void SaveAuthor();
    }
}