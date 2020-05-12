using Caliburn.Micro;
using ComicReaderClassLibrary.DataAccess.DataModels;
using ComicReaderDataManagementUI.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ComicReaderDataManagementUI.ViewModels
{
    public interface ISubSeriesViewModel
    {
        BindableCollection<ComicDataModel> ComicBox { get; set; }
        BindableCollection<ComicDataModel> ComicInSubSeries { get; set; }
        DateTime DateSeriesEnd { get; set; }
        DateTime DateSeriesStart { get; set; }
        BindableCollection<SeriesDataModel> ParentSeries { get; set; }
        ComicDataModel SelectedComic { get; set; }
        ComicDataModel SelectedComicInSeries { get; set; }
        SeriesDataModel SelectedSeries { get; set; }
        SubSeriesDataModel SelectedSubSeries { get; set; }
        string StatusBar { get; set; }
        BindableCollection<SubSeriesDataModel> SubSeriesSelection { get; set; }
        string SubSeriesTitle { get; set; }

        void AddComicToSubSerie();
        void DeleteSubSerie();
        Task HandleAsync(ComicChangedOnMainEvent message, CancellationToken cancellationToken);
        Task HandleAsync(SubSeriesChangedEvent message, CancellationToken cancellationToken);
        void NewSubSerie();
        void SaveSubSerie();
    }
}