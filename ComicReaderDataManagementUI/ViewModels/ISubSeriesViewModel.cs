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
        /// <summary>
        /// Backing field for the Comic dropdown
        /// </summary>
        BindableCollection<ComicDataModel> ComicBox { get; set; }
        /// <summary>
        /// Backing field for the Comics in SubSeries listbox
        /// </summary>
        BindableCollection<ComicDataModel> ComicInSubSeries { get; set; }
        /// <summary>
        /// Backing field for the DateSeriesEnd field
        /// </summary>
        DateTime DateSeriesEnd { get; set; }
        /// <summary>
        /// Backing field for the DateSeriesStart field
        /// </summary>
        DateTime DateSeriesStart { get; set; }
        /// <summary>
        /// Backing field for the ParentSeries dropdown
        /// </summary>
        BindableCollection<SeriesDataModel> ParentSeries { get; set; }
        /// <summary>
        /// Backing field for the Selected Comic in the Comic dropdown
        /// </summary>
        ComicDataModel SelectedComic { get; set; }
        /// <summary>
        /// Backing field for the Selected Comic in the Comic in Subseries Listbox
        /// </summary>
        ComicDataModel SelectedComicInSeries { get; set; }
        /// <summary>
        /// Backing field for the Selected Serie in the ParentSeries dropdown
        /// </summary>
        SeriesDataModel SelectedSeries { get; set; }
        /// <summary>
        /// Backing field for the Selected SubSerie in the Series Listbox
        /// </summary>
        SubSeriesDataModel SelectedSubSeries { get; set; }
        /// <summary>
        /// Backing field for the Statusbar label
        /// </summary>
        string StatusBar { get; set; }
        /// <summary>
        /// Backing field for the Selected SubSerie in the SubSerie Listbox
        /// </summary>
        BindableCollection<SubSeriesDataModel> SubSeriesSelection { get; set; }
        /// <summary>
        /// Backing field for the Selected Series Title textbox
        /// </summary>
        string SubSeriesTitle { get; set; }

        /// <summary>
        /// Method for the Add Comic to Series button
        /// </summary>
        void AddComicToSubSerie();
        /// <summary>
        /// Method for the Delete SubSeries button
        /// </summary>
        void DeleteSubSerie();
        /// <summary>
        /// Event handler for when a Comic was changed in the MainViewModel
        /// </summary>
        Task HandleAsync(ComicChangedOnMainEvent message, CancellationToken cancellationToken);
        /// <summary>
        /// Event handler for when a Comic was changed in the SeriesViewModel
        /// </summary>
        Task HandleAsync(SubSeriesChangedEvent message, CancellationToken cancellationToken);
        /// <summary>
        /// Method for the New SubSerie button
        /// </summary>
        void NewSubSerie();
        /// <summary>
        /// Method for the Save SubSerie button
        /// </summary>
        void SaveSubSerie();
    }
}