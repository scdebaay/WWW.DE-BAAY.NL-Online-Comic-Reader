using Caliburn.Micro;
using ComicReaderClassLibrary.DataAccess.DataModels;
using ComicReaderDataManagementUI.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ComicReaderDataManagementUI.ViewModels
{
    public interface ISeriesViewModel
    {
        BindableCollection<ComicDataModel> ComicBox { get; set; }
        BindableCollection<ComicDataModel> ComicInSeries { get; set; }
        DateTime DateSeriesEnd { get; set; }
        DateTime DateSeriesStart { get; set; }
        ComicDataModel SelectedComic { get; set; }
        ComicDataModel SelectedComicInSeries { get; set; }
        SeriesDataModel SelectedSeries { get; set; }
        SubSeriesDataModel SelectedSubSeries { get; set; }
        BindableCollection<SeriesDataModel> SeriesSelection { get; set; }
        string SeriesTitle { get; set; }
        string StatusBar { get; set; }
        BindableCollection<SubSeriesDataModel> SubSeriesInSeries { get; set; }

        void AddComicToSerie();
        void DeleteSerie();
        Task HandleAsync(ComicChangedOnMainEvent message, CancellationToken cancellationToken);
        Task HandleAsync(SubSeriesChangedEvent message, CancellationToken cancellationToken);
        void NewSerie();
        void SaveSerie();
    }
}