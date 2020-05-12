using Caliburn.Micro;
using ComicReaderClassLibrary.DataAccess.DataModels;
using ComicReaderClassLibrary.DataAccess.Implementations;
using ComicReaderDataManagementUI.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ComicReaderDataManagementUI.ViewModels
{
    public class SeriesViewModel : Screen, ISeriesViewModel, IHandle<ComicChangedOnMainEvent>, IHandle<SubSeriesChangedEvent>
    {
        private readonly IConfiguration _configuration;
        private readonly ISqlUiDbConnection _sqlUiDbConnection;
        private readonly ILogger _logger;
        private IEventAggregator _eventAggregator;
        private bool newSerieAdded { get; set; } = false;
        private string _statusBar;
        public string StatusBar
        {
            get { return _statusBar; }
            set
            {
                _statusBar = value;
                NotifyOfPropertyChange(nameof(StatusBar));
            }
        }

        private string _seriesTitle = "Select a series";
        public string SeriesTitle
        {
            get { return _seriesTitle; }
            set 
            { 
                _seriesTitle = value;
                NotifyOfPropertyChange(nameof(SeriesTitle));
            }
        }

        public DateTime DateSeriesStart
        {
            get
            {
                return SelectedSeries.DateStart;
            }
            set
            {
                SelectedSeries.DateStart = value;
                NotifyOfPropertyChange(() => DateSeriesStart);
            }
        }
        public DateTime DateSeriesEnd
        {
            get
            {
                return SelectedSeries.DateEnd;
            }
            set
            {
                SelectedSeries.DateEnd = value;
                NotifyOfPropertyChange(() => DateSeriesEnd);
            }
        }

        private SeriesDataModel _selectedSeries;
        public SeriesDataModel SelectedSeries
        {
            get { return _selectedSeries; }
            set 
            { 
                _selectedSeries = value;
                if (SelectedSeries != null)
                {
                    StatusBar = $"Selected serie Id is {SelectedSeries.Id}";
                    SeriesTitle = SelectedSeries.Title;
                    DateSeriesStart = SelectedSeries.DateStart;
                    DateSeriesEnd = SelectedSeries.DateEnd;
                }
                else
                {
                    SeriesTitle = "Select a serie";
                    StatusBar = $"";
                }
                NotifyOfPropertyChange(nameof(SelectedSeries));
            }
        }
        private SubSeriesDataModel _selectedSubSeries;
        public SubSeriesDataModel SelectedSubSeries
        {
            get { return _selectedSubSeries; }
            set 
            { 
                _selectedSubSeries = value;
                NotifyOfPropertyChange(nameof(SelectedSubSeries));
            }
        }
        private ComicDataModel _selectedComicInSeries;
        public ComicDataModel SelectedComicInSeries
        {
            get { return _selectedComicInSeries; }
            set 
            { 
                _selectedComicInSeries = value;
                NotifyOfPropertyChange(nameof(SelectedComicInSeries));
            }
        }
        private ComicDataModel _selectedComic;
        public ComicDataModel SelectedComic
        {
            get { return _selectedComic; }
            set 
            { 
                _selectedComic = value;
                NotifyOfPropertyChange(nameof(SelectedComic));
            }
        }


        private BindableCollection<SeriesDataModel> _seriesSelection = new BindableCollection<SeriesDataModel>();
        public BindableCollection<SeriesDataModel> SeriesSelection
        {
            get { return _seriesSelection; }
            set
            {
                _seriesSelection = value;
                NotifyOfPropertyChange(nameof(SeriesSelection));
            }
        }

        private BindableCollection<SubSeriesDataModel> _subSeriesInSeries = new BindableCollection<SubSeriesDataModel>();
        public BindableCollection<SubSeriesDataModel> SubSeriesInSeries
        {
            get { return _subSeriesInSeries; }
            set
            {
                _subSeriesInSeries = value;
                NotifyOfPropertyChange(nameof(SubSeriesInSeries));
            }
        }

        private BindableCollection<ComicDataModel> _comicInSeries = new BindableCollection<ComicDataModel>();
        public BindableCollection<ComicDataModel> ComicInSeries
        {
            get { return _comicInSeries; }
            set
            {
                _comicInSeries = value;
                NotifyOfPropertyChange(nameof(ComicInSeries));
            }
        }
        private BindableCollection<ComicDataModel> _comicBox = new BindableCollection<ComicDataModel>();
        public BindableCollection<ComicDataModel> ComicBox
        {
            get { return _comicBox; }
            set
            {
                _comicBox = value;
                NotifyOfPropertyChange(nameof(ComicBox));
            }
        }
        public SeriesViewModel(IConfiguration configuration, ISqlUiDbConnection sqlUiDbConnection, ILogger<SeriesViewModel> logger, IEventAggregator eventAggregator)
        {
            _configuration = configuration;
            _sqlUiDbConnection = sqlUiDbConnection;
            _logger = logger;
            _eventAggregator = eventAggregator;
            _eventAggregator.SubscribeOnPublishedThread(this);
            PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(SelectedSeries) && SelectedSeries != null)
                {
                    _ = RetrieveSubseriesForSeries();
                    _ = RetrieveComicsForSeries();
                }
            };
            PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(SelectedSubSeries) && SelectedSubSeries != null)
                {
                    _ = RetrieveComicsForSubSeries();
                }
            };
            _ = RetrieveSeries();
        }

        private async Task RetrieveSeries()
        {
            SeriesDataModel selectedSeries = SelectedSeries as SeriesDataModel;
            SeriesSelection.Clear();
            var list = await _sqlUiDbConnection.RetrieveSerieAsync();
            if (selectedSeries != null)
            {
                foreach (SeriesDataModel serie in list)
                {
                    if (selectedSeries == serie)
                    {
                        SelectedSeries = selectedSeries;
                    }
                }
            }
            SeriesSelection.AddRange(list);
            SelectedSeries = SeriesSelection[0];
            await RetrieveSubseriesForSeries();
        }
        private async Task RetrieveSubseriesForSeries()
        {
            SubSeriesInSeries.Clear();
            var list = await _sqlUiDbConnection.RetrieveSubSerieAsync();
            var l = list.Where(x => x.SeriesId == SelectedSeries.Id).Select(x => x);
            if (l != null)
            {
                SelectedSubSeries = l.FirstOrDefault();
            }
            SubSeriesInSeries.AddRange(l);
        }
        private async Task RetrieveComicsForSeries()
        {
            ComicInSeries.Clear();
            ComicBox.Clear();
            var list = await _sqlUiDbConnection.RetrieveComicsAsync();
            var l = list.Where(x => x.SeriesId == SelectedSeries.Id).Select(x => x);
            if (l != null)
            {
                SelectedComicInSeries = l.FirstOrDefault();
            }
            ComicInSeries.AddRange(l);
            ComicBox.AddRange(list);
            SelectedComic = ComicBox[0];
        }
        private async Task RetrieveComicsForSubSeries()
        {
            ComicInSeries.Clear();
            ComicBox.Clear();
            var list = await _sqlUiDbConnection.RetrieveComicsAsync();
            var l = list.Where(x => x.SubSeriesId == SelectedSubSeries.Id).Select(x => x);
            if (l != null)
            {
                SelectedComicInSeries = l.FirstOrDefault();
            }
            ComicInSeries.AddRange(l);
            SelectedComicInSeries = ComicInSeries[0];
            ComicBox.AddRange(list);
            SelectedComic = ComicBox[0];
        }

        public void AddComicToSerie()
        {
            var succeeded = _sqlUiDbConnection.SaveSerieAssoc(SelectedComic.Id, SelectedSeries.Id);
            if (succeeded == true)
            {
                ComicInSeries.Add(SelectedComic);
                _eventAggregator.PublishOnUIThreadAsync(new ComicChangedOnDialogEvent());
                StatusBar = $"Comic added to series";
            }
            else
            {
                ComicInSeries.Remove(SelectedComic);
                StatusBar = $"Add comic failed, check log";
            }
        }
        public void NewSerie()
        {
            var lastId = SeriesSelection.OrderByDescending(i => i.Id).First().Id + 1;
            SelectedSeries = new SeriesDataModel
            {
                Id = lastId,
                Title = "New series",
                DateStart = DateTime.Now,
                DateEnd = DateTime.Now
            };
            SeriesSelection.Add(SelectedSeries);
            newSerieAdded = true;
        }
        public void SaveSerie()
        {
            int serieToUpdatePos = SeriesSelection.IndexOf(SelectedSeries);
            if (newSerieAdded == false)
            {
                var serieUpdated = _sqlUiDbConnection.UpdateSerie(SelectedSeries.Id, SeriesTitle, DateSeriesStart, DateSeriesEnd);
                SeriesDataModel serieToUpdate = new SeriesDataModel
                {
                    Id = SelectedSeries.Id,
                    Title = SeriesTitle,
                    DateStart = DateSeriesStart,
                    DateEnd = DateSeriesEnd
                };
                if (serieUpdated == true)
                {
                    SeriesSelection.RemoveAt(serieToUpdatePos);
                    SeriesSelection.Insert(serieToUpdatePos, serieToUpdate);
                    SelectedSeries = SeriesSelection[serieToUpdatePos];
                    _eventAggregator.PublishOnUIThreadAsync(new ComicChangedOnDialogEvent());
                    StatusBar = $"Serie updated";
                }
                else
                {
                    StatusBar = $"Serie update failed, check log";
                }
            }
            else
            {
                var newSerieId = _sqlUiDbConnection.InsertSerie(SeriesTitle, DateSeriesStart, DateSeriesEnd);
                SelectedSeries.Id = newSerieId;
                if (SelectedSeries.Id == 0)
                {
                    StatusBar = $"Serie not saved to the database, check log";
                }
                else if (SelectedSeries.Id == 1)
                {
                    StatusBar = $"Serie existed in the database";
                }
                newSerieAdded = false;
            }
            _ = RetrieveSeries();
        }
        public void DeleteSerie()
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                int serieToDeletePos = SeriesSelection.IndexOf(SelectedSeries);
                var serieDeleted = _sqlUiDbConnection.DeleteSerie(SelectedSeries.Id);
                if (serieDeleted == true)
                {
                    SeriesSelection.RemoveAt(serieToDeletePos);
                    SelectedSeries = SeriesSelection[0];
                    _eventAggregator.PublishOnUIThreadAsync(new ComicChangedOnDialogEvent());
                    StatusBar = $"Serie deleted";
                }
                else
                {
                    StatusBar = $"Serie deletion failed, check log";
                }
            }
        }

        protected override Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            PropertyChanged -= (sender, args) =>
            {
                if (args.PropertyName == "SelectedItem" && SelectedSeries != null)
                {
                    _ = RetrieveSubseriesForSeries();
                    _ = RetrieveComicsForSeries();
                }
            };
            PropertyChanged -= (sender, args) =>
            {
                if (args.PropertyName == nameof(SelectedSubSeries) && SelectedSubSeries != null)
                {
                    _ = RetrieveComicsForSubSeries();
                }
            };
            return base.OnDeactivateAsync(close, cancellationToken);
        }
        async public Task HandleAsync(ComicChangedOnMainEvent message, CancellationToken cancellationToken)
        {
            await RetrieveComicsForSeries();
        }
        async public Task HandleAsync(SubSeriesChangedEvent message, CancellationToken cancellationToken)
        {
            await RetrieveSubseriesForSeries();
        }
    }
}
