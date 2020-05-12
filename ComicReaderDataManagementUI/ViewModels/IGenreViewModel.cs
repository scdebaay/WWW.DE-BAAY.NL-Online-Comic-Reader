using Caliburn.Micro;
using ComicReaderClassLibrary.DataAccess.DataModels;
using ComicReaderDataManagementUI.Events;
using System.Threading;
using System.Threading.Tasks;

namespace ComicReaderDataManagementUI.ViewModels
{
    public interface IGenreViewModel
    {
        BindableCollection<ComicDataModel> ComicBox { get; }
        BindableCollection<ComicDataModel> ComicsInGenreBox { get; }
        BindableCollection<GenreDataModel> GenreBox { get; }
        string GenreName { get; set; }
        ComicDataModel SelectedComic { get; set; }
        ComicDataModel SelectedComicInGenre { get; set; }
        GenreDataModel SelectedItem { get; set; }
        string StatusBar { get; set; }

        void AddComicToGenre();
        void DeleteGenre();
        Task HandleAsync(ComicChangedOnMainEvent message, CancellationToken cancellationToken);
        void NewGenre();
        void SaveGenre();
    }
}