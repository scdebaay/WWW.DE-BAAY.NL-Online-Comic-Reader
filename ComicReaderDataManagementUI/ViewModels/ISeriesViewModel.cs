using Caliburn.Micro;
using ComicReaderClassLibrary.DataAccess.DataModels;
using ComicReaderDataManagementUI.Events;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ComicReaderDataManagementUI.ViewModels
{
    public interface ISeriesViewModel
    {
        /// <summary>
        /// Backing field for the Comic dropdown
        /// </summary>
        BindableCollection<ComicDataModel> ComicList { get; set; }
        /// <summary>
        /// Backing field for the Comics in Series listbox
        /// </summary>
        BindableCollection<ComicDataModel> ComicInSeries { get; set; }
        /// <summary>
        /// Backing field for the DateSeriesEnd field
        /// </summary>
        DateTime DateSeriesEnd { get; set; }
        /// <summary>
        /// Backing field for the DateSeriesStart field
        /// </summary>
        DateTime DateSeriesStart { get; set; }        
        /// <summary>
        /// Backing field for the Selected Comic in the Comic in Series Listbox
        /// </summary>
        ComicDataModel SelectedComicInSeries { get; set; }
        /// <summary>
        /// Backing field for the Selected Serie in the Series dropdown
        /// </summary>
        SeriesDataModel SelectedSeries { get; set; }
        /// <summary>
        /// Backing field for the Selected SubSerie in the Series Listbox
        /// </summary>
        SubSeriesDataModel SelectedSubSeries { get; set; }
        /// <summary>
        /// Backing field for the Selected Serie in the Serie Listbox
        /// </summary>
        BindableCollection<SeriesDataModel> SeriesSelection { get; set; }
        /// <summary>
        /// Backing field for the Selected Series Title textbox
        /// </summary>
        string SeriesTitle { get; set; }
        /// <summary>
        /// Backing field for the Statusbar label
        /// </summary>
        string StatusBar { get; set; }
        /// <summary>
        /// Command to add Comis to Serie
        /// </summary>
        ICommand AddComicToSerieCommand { get; }
        /// <summary>
        /// Backing field for the SubSeries Listbox
        /// </summary>
        BindableCollection<SubSeriesDataModel> SubSeriesInSeries { get; set; }
        
        /// <summary>
        /// Method for the Delete Series button
        /// </summary>
        void DeleteSerie();
        /// <summary>
        /// Event handler for when a Comic was changed in the MainViewModel
        /// </summary>
        Task HandleAsync(ComicChangedOnMainEvent message, CancellationToken cancellationToken);
        /// <summary>
        /// Event handler for when a Comic was changed in the SubSeriesViewModel
        /// </summary>
        Task HandleAsync(SubSeriesChangedEvent message, CancellationToken cancellationToken);
        /// <summary>
        /// Method for the New Serie button
        /// </summary>
        void NewSerie();
        /// <summary>
        /// Method for the Save Serie button
        /// </summary>
        void SaveSerie();
    }
}