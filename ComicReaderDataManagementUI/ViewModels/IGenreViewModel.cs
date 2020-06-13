using Caliburn.Micro;
using ComicReaderClassLibrary.DataAccess.DataModels;
using ComicReaderDataManagementUI.Events;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ComicReaderDataManagementUI.ViewModels
{
    public interface IGenreViewModel
    {
        /// <summary>
        /// Backing field for the Comic dropdown
        /// </summary>
        BindableCollection<ComicDataModel> ComicList { get; }
        /// <summary>
        /// Backing field for the Comics in Genre listbox
        /// </summary>
        BindableCollection<ComicDataModel> ComicsInGenreBox { get; }
        /// <summary>
        /// Backing field for the Genre dropdown
        /// </summary>
        BindableCollection<GenreDataModel> GenreBox { get; }
        /// <summary>
        /// Backing field for the Genre term textbox
        /// </summary>
        string GenreName { get; set; }
        /// <summary>
        /// Backing field for the Selected Comic in the Comic in Genre Listbox
        /// </summary>
        ComicDataModel SelectedComicInGenre { get; set; }
        /// <summary>
        /// Backing field for the Selected Genre in the Genre dropdown
        /// </summary>
        GenreDataModel SelectedItem { get; set; }
        /// <summary>
        /// Backing field for the Statusbar label
        /// </summary>
        string StatusBar { get; set; }
        /// <summary>
        /// Command to add Comics to Genre
        /// </summary>
        ICommand AddComicToGenreCommand { get; }
        /// <summary>
        /// Command for the New Genre button
        /// </summary>
        ICommand NewGenreCommand { get; }
        /// <summary>
        /// Command for the Save Genre button
        /// </summary>
        ICommand SaveGenreCommand { get; }
        /// <summary>
        /// Command for the Delete Genre button
        /// </summary>
        ICommand DeleteGenreCommand { get; }
        /// <summary>
        /// Event handler for when a Comic was changed in the MainViewModel
        /// </summary>
        Task HandleAsync(ComicChangedOnMainEvent message, CancellationToken cancellationToken);
    }
}