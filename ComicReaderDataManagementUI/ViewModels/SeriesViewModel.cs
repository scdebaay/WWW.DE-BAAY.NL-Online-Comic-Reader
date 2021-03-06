﻿using Caliburn.Micro;
using ComicReaderClassLibrary.DataAccess.DataModels;
using ComicReaderClassLibrary.DataAccess.Implementations;
using ComicReaderDataManagementUI.Events;
using ComicReaderDataManagementUI.ViewModels.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ComicReaderDataManagementUI.ViewModels
{
    public class SeriesViewModel : Screen, ISeriesViewModel, IHandle<ComicChangedOnMainEvent>, IHandle<SubSeriesChangedEvent>
    {
        #region injected objects
        private readonly IConfiguration _configuration;
        private readonly ISqlUiDbConnection _sqlUiDbConnection;
        private readonly ILogger _logger;
        private IEventAggregator _eventAggregator;
        #endregion

        #region (backed) fields
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
        private ICommand _addComicToSeriesCommand;
        public ICommand AddComicToSerieCommand
        {
            get
            {
                if (_addComicToSeriesCommand == null)
                    _addComicToSeriesCommand = new ParameteredComicDelegatedCommand(AddComicToSerie);
                return _addComicToSeriesCommand;
            }
        }

        private ICommand _newSerieCommand;
        public ICommand NewSerieCommand
        {
            get
            {
                if (_newSerieCommand == null)
                    _newSerieCommand = new ComicDelegatedCommand(NewSerie);
                return _newSerieCommand;
            }
        }

        private ICommand _saveSerieCommand;
        public ICommand SaveSerieCommand
        {
            get
            {
                if (_saveSerieCommand == null)
                    _saveSerieCommand = new ComicDelegatedCommand(SaveSerie);
                return _saveSerieCommand;
            }
        }

        private ICommand _deleteSerieCommand;
        public ICommand DeleteSerieCommand
        {
            get
            {
                if (_deleteSerieCommand == null)
                    _deleteSerieCommand = new ComicDelegatedCommand(DeleteSerie);
                return _deleteSerieCommand;
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
        public BindableCollection<ComicDataModel> ComicList
        {
            get { return _comicBox; }
            set
            {
                _comicBox = value;
                NotifyOfPropertyChange(nameof(ComicList));
            }
        }
        #endregion

        #region constructor
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
        #endregion

        #region private methods
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
            ComicList.Clear();
            var list = await _sqlUiDbConnection.RetrieveComicsAsync("");
            var l = list.Where(x => x.SeriesId == SelectedSeries.Id).Select(x => x);
            if (l != null)
            {
                SelectedComicInSeries = l.FirstOrDefault();
            }
            ComicInSeries.AddRange(l);
            ComicList.AddRange(list);
        }
        private async Task RetrieveComicsForSubSeries()
        {
            ComicInSeries.Clear();
            ComicList.Clear();
            var list = await _sqlUiDbConnection.RetrieveComicsAsync("");
            var l = list.Where(x => x.SubSeriesId == SelectedSubSeries.Id).Select(x => x);
            if (l != null)
            {
                SelectedComicInSeries = l.FirstOrDefault();
            }
            ComicInSeries.AddRange(l);
            SelectedComicInSeries = ComicInSeries[0];
            ComicList.AddRange(list);
        }
        private void AddComicToSerie(object selectedComicCollection)
        {
            var selectedComicIds = new List<int>();
            foreach (ComicDataModel comic in selectedComicCollection as IList)
            {
                selectedComicIds.Add(comic.Id);
            }

            var succeeded = _sqlUiDbConnection.SaveSerieAssoc(selectedComicIds, SelectedSeries.Id);
            if (succeeded == true)
            {
                IList items = (IList)selectedComicCollection;
                var comicsAdded = items.Cast<ComicDataModel>().ToList();
                ComicInSeries.AddRange(comicsAdded.Except(ComicInSeries.ToList()));
                _eventAggregator.PublishOnUIThreadAsync(new ComicChangedOnDialogEvent());
                StatusBar = $"Comic(s) added to series";
            }
            else
            {
                IList items = (IList)selectedComicCollection;
                var comicsNotAdded = items.Cast<ComicDataModel>();
                ComicInSeries.RemoveRange(comicsNotAdded);
                StatusBar = $"Add comic(s) failed, check log";
            }
        }
        private void NewSerie()
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
        private void SaveSerie()
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
        private void DeleteSerie()
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
        #endregion

        #region public methods
        #endregion

        #region event handlers
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
        #endregion
    }
}
