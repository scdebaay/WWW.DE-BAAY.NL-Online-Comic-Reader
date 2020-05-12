using Caliburn.Micro;
using ComicReaderClassLibrary.DataAccess.DataModels;
using ComicReaderDataManagementUI.Events;
using System.Threading;
using System.Threading.Tasks;

namespace ComicReaderDataManagementUI.ViewModels
{
    public interface IPublisherViewModel
    {
        BindableCollection<ComicDataModel> ComicBox { get; }
        BindableCollection<ComicDataModel> ComicsByPublisherBox { get; }
        BindableCollection<PublisherDataModel> PublisherBox { get; }
        string PublisherName { get; set; }
        ComicDataModel SelectedComic { get; set; }
        ComicDataModel SelectedComicByPublisher { get; set; }
        PublisherDataModel SelectedItem { get; set; }
        string StatusBar { get; set; }

        void AddComicToPublisher();
        void DeletePublisher();
        Task HandleAsync(ComicChangedOnMainEvent message, CancellationToken cancellationToken);
        void NewPublisher();
        void SavePublisher();
    }
}