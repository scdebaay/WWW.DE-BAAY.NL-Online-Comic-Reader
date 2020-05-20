using Caliburn.Micro;
using ComicReaderClassLibrary.DataAccess.DataModels;
using ComicReaderDataManagementUI.Events;
using System.Threading;
using System.Threading.Tasks;

namespace ComicReaderDataManagementUI.ViewModels
{
    public interface IGenreViewModel
    {
        /// <summary>
        /// Backing field for the Comic dropdown
        /// </summary>
        BindableCollection<ComicDataModel> ComicBox { get; }
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
        /// Backing field for the Selected Comic in the Comic dropdown
        /// </summary>
        ComicDataModel SelectedComic { get; set; }
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
        /// Method for the Add Comic to Genre button
        /// </summary>
        void AddComicToGenre();
        /// <summary>
        /// Method for the Delete Genre button
        /// </summary>
        void DeleteGenre();
        /// <summary>
        /// Event handler for when a Comic was changed in the MainViewModel
        /// </summary>
        Task HandleAsync(ComicChangedOnMainEvent message, CancellationToken cancellationToken);
        /// <summary>
        /// Method for the New Genre button
        /// </summary>
        void NewGenre();
        /// <summary>
        /// Method for the Save Genre button
        /// </summary>
        void SaveGenre();
    }
}