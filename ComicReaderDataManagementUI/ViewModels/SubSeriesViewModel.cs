using Caliburn.Micro;
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
    public class SubSeriesViewModel : Screen, ISubSeriesViewModel, IHandle<ComicChangedOnMainEvent>, IHandle<SubSeriesChangedEvent>
    {
        #region injected objects
        private readonly IConfiguration _configuration;
        private readonly ISqlUiDbConnection _sqlUiDbConnection;
        private readonly ILogger _logger;
        private IEventAggregator _eventAggregator;
        #endregion

        #region (backed fields)
        private bool newSubSerieAdded { get; set; } = false;
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
        private ICommand _addComicToSubSerieCommand;
        public ICommand AddComicToSubSerieCommand
        {
            get
            {
                if (_addComicToSubSerieCommand == null)
                    _addComicToSubSerieCommand = new ParameteredComicDelegatedCommand(AddComicToSubSerie);
                return _addComicToSubSerieCommand;
            }
        }

        private string _subSeriesTitle = "Select a subseries";
        public string SubSeriesTitle
        {
            get { return _subSeriesTitle; }
            set
            {
                _subSeriesTitle = value;
                NotifyOfPropertyChange(nameof(SubSeriesTitle));
            }
        }

        public DateTime DateSeriesStart
        {
            get
            {
                return SelectedSubSeries.DateStart;
            }
            set
            {
                SelectedSubSeries.DateStart = value;
                NotifyOfPropertyChange(() => DateSeriesStart);
            }
        }
        public DateTime DateSeriesEnd
        {
            get
            {
                return SelectedSubSeries.DateEnd;
            }
            set
            {
                SelectedSubSeries.DateEnd = value;
                NotifyOfPropertyChange(() => DateSeriesEnd);
            }
        }

        private SubSeriesDataModel _selectedSubSeries;
        public SubSeriesDataModel SelectedSubSeries
        {
            get { return _selectedSubSeries; }
            set
            {
                _selectedSubSeries = value;
                if (SelectedSubSeries != null)
                {
                    StatusBar = $"Selected subserie Id is {SelectedSubSeries.Id}";
                    SubSeriesTitle = SelectedSubSeries.Title;
                    DateSeriesStart = SelectedSubSeries.DateStart;
                    DateSeriesEnd = SelectedSubSeries.DateEnd;
                }
                else
                {
                    SubSeriesTitle = "Select a subserie";
                    StatusBar = $"";
                }
                NotifyOfPropertyChange(nameof(SelectedSubSeries));
            }
        }
        private SeriesDataModel _selectedSeries;
        public SeriesDataModel SelectedSeries
        {
            get { return _selectedSeries; }
            set
            {
                _selectedSeries = value;
                NotifyOfPropertyChange(nameof(SelectedSeries));
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
        
        private BindableCollection<SubSeriesDataModel> _subSeriesSelection = new BindableCollection<SubSeriesDataModel>();
        public BindableCollection<SubSeriesDataModel> SubSeriesSelection
        {
            get { return _subSeriesSelection; }
            set
            {
                _subSeriesSelection = value;
                NotifyOfPropertyChange(nameof(SubSeriesSelection));
            }
        }

        private BindableCollection<SeriesDataModel> _parentSeries = new BindableCollection<SeriesDataModel>();
        public BindableCollection<SeriesDataModel> ParentSeries
        {
            get { return _parentSeries; }
            set
            {
                _parentSeries = value;
                NotifyOfPropertyChange(nameof(ParentSeries));
            }
        }

        private BindableCollection<ComicDataModel> _comicInSubSeries = new BindableCollection<ComicDataModel>();
        public BindableCollection<ComicDataModel> ComicInSubSeries
        {
            get { return _comicInSubSeries; }
            set
            {
                _comicInSubSeries = value;
                NotifyOfPropertyChange(nameof(ComicInSubSeries));
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
        public SubSeriesViewModel(IConfiguration configuration, ISqlUiDbConnection sqlUiDbConnection, ILogger<SubSeriesViewModel> logger, IEventAggregator eventAggregator)
        {
            _configuration = configuration;
            _sqlUiDbConnection = sqlUiDbConnection;
            _logger = logger;
            _eventAggregator = eventAggregator;
            _eventAggregator.SubscribeOnPublishedThread(this);
            PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(SelectedSubSeries) && SelectedSubSeries != null)
                {
                    _ = RetrieveParentseriesForSeries();
                    _ = RetrieveComicsForSubSeries();
                }
            };
            //PropertyChanged += (sender, args) =>
            //{
            //    if (args.PropertyName == nameof(SelectedSubSeries) && SelectedSubSeries != null)
            //    {
            //        _ = RetrieveComicsForSubSeries();
            //    }
            //};
            _ = RetrieveSubSeries();
        }
        #endregion

        #region private methods
        private async Task RetrieveSubSeries()
        {
            SubSeriesDataModel selectedSubSeries = SelectedSubSeries as SubSeriesDataModel;
            SubSeriesSelection.Clear();
            var list = await _sqlUiDbConnection.RetrieveSubSerieAsync();
            if (selectedSubSeries != null)
            {
                foreach (SubSeriesDataModel subserie in list)
                {
                    if (selectedSubSeries == subserie)
                    {
                        SelectedSubSeries = selectedSubSeries;
                    }
                }
            }
            SubSeriesSelection.AddRange(list);
            SelectedSubSeries = SubSeriesSelection[0];
            await RetrieveParentseriesForSeries();
        }
        private async Task RetrieveParentseriesForSeries()
        {
            ParentSeries.Clear();
            var list = await _sqlUiDbConnection.RetrieveSerieAsync();
            if (list != null)
            {
                SelectedSeries = list.Where(x => x.Id == SelectedSubSeries.SeriesId).FirstOrDefault();
            }
            ParentSeries.AddRange(list);
        }
        private async Task RetrieveComicsForSubSeries()
        {
            ComicInSubSeries.Clear();
            ComicList.Clear();
            var list = await _sqlUiDbConnection.RetrieveComicsAsync("");
            var l = list.Where(x => x.SubSeriesId == SelectedSubSeries.Id).Select(x => x);
            if (l != null)
            {
                SelectedComicInSeries = l.FirstOrDefault();
            }
            ComicInSubSeries.AddRange(l);
            ComicList.AddRange(list);
        }
        private void AddComicToSubSerie(object selectedComicCollection)
        {
            var selectedComicIds = new List<int>();
            foreach (ComicDataModel comic in selectedComicCollection as IList)
            {
                selectedComicIds.Add(comic.Id);
            }

            var succeeded = _sqlUiDbConnection.SaveSubSerieAssoc(selectedComicIds, SelectedSubSeries.Id);
            if (succeeded == true)
            {
                IList items = (IList)selectedComicCollection;
                var comicsAdded = items.Cast<ComicDataModel>().ToList();
                ComicInSubSeries.AddRange(comicsAdded.Except(ComicInSubSeries.ToList()));
                _eventAggregator.PublishOnUIThreadAsync(new ComicChangedOnDialogEvent());
                StatusBar = $"Comic added to series";
            }
            else
            {
                IList items = (IList)selectedComicCollection;
                var comicsNotAdded = items.Cast<ComicDataModel>();
                ComicInSubSeries.RemoveRange(comicsNotAdded);
                StatusBar = $"Add comic failed, check log";
            }
        }
        #endregion

        #region public methods
        public void NewSubSerie()
        {
            var lastId = SubSeriesSelection.OrderByDescending(i => i.Id).First().Id + 1;
            SelectedSubSeries = new SubSeriesDataModel
            {
                Id = lastId,
                Title = "New sub series",
                DateStart = DateTime.Now,
                DateEnd = DateTime.Now,
                SeriesId = SelectedSeries.Id
            };
            SubSeriesSelection.Add(SelectedSubSeries);
            newSubSerieAdded = true;
        }
        public void SaveSubSerie()
        {
            int subSerieToUpdatePos = SubSeriesSelection.IndexOf(SelectedSubSeries);
            if (newSubSerieAdded == false)
            {
                var subSerieUpdated = _sqlUiDbConnection.UpdateSubSerie(SelectedSubSeries.Id, SubSeriesTitle, DateSeriesStart, DateSeriesEnd, SelectedSeries.Id);
                SubSeriesDataModel subSerieToUpdate = new SubSeriesDataModel
                {
                    Id = SelectedSubSeries.Id,
                    Title = SubSeriesTitle,
                    DateStart = DateSeriesStart,
                    DateEnd = DateSeriesEnd,
                    SeriesId = SelectedSeries.Id
                };
                if (subSerieUpdated == true)
                {
                    SubSeriesSelection.RemoveAt(subSerieToUpdatePos);
                    SubSeriesSelection.Insert(subSerieToUpdatePos, subSerieToUpdate);
                    SelectedSubSeries = SubSeriesSelection[subSerieToUpdatePos];
                    _eventAggregator.PublishOnUIThreadAsync(new ComicChangedOnDialogEvent());
                    StatusBar = $"SubSerie updated";
                }
                else
                {
                    StatusBar = $"SubSerie update failed, check log";
                }
            }
            else
            {
                var newSerieId = _sqlUiDbConnection.InsertSubSerie(SubSeriesTitle, DateSeriesStart, DateSeriesEnd, SelectedSeries.Id);
                SelectedSeries.Id = newSerieId;
                if (SelectedSeries.Id == 0)
                {
                    StatusBar = $"Serie not saved to the database, check log";
                }
                else if (SelectedSeries.Id == 1)
                {
                    StatusBar = $"Serie existed in the database";
                }
                newSubSerieAdded = false;
            }
            _ = RetrieveSubSeries();
        }
        public void DeleteSubSerie()
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                int subSerieToDeletePos = SubSeriesSelection.IndexOf(SelectedSubSeries);
                var subSerieDeleted = _sqlUiDbConnection.DeleteSubSerie(SelectedSubSeries.Id);
                if (subSerieDeleted == true)
                {
                    SubSeriesSelection.RemoveAt(subSerieToDeletePos);
                    SelectedSubSeries = SubSeriesSelection[0];
                    _eventAggregator.PublishOnUIThreadAsync(new ComicChangedOnDialogEvent());
                    StatusBar = $"SubSerie deleted";
                }
                else
                {
                    StatusBar = $"SubSerie deletion failed, check log";
                }
            }
        }
        #endregion

        #region event handlers
        protected override Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            PropertyChanged -= (sender, args) =>
            {
                if (args.PropertyName == "SelectedItem" && SelectedSeries != null)
                {
                    _ = RetrieveParentseriesForSeries();
                    _ = RetrieveComicsForSubSeries();
                }
            };
            //PropertyChanged -= (sender, args) =>
            //{
            //    if (args.PropertyName == nameof(SelectedSubSeries) && SelectedSubSeries != null)
            //    {
            //        _ = RetrieveComicsForSubSeries();
            //    }
            //};
            return base.OnDeactivateAsync(close, cancellationToken);
        }
        async public Task HandleAsync(ComicChangedOnMainEvent message, CancellationToken cancellationToken)
        {
            await RetrieveComicsForSubSeries();
        }
        async public Task HandleAsync(SubSeriesChangedEvent message, CancellationToken cancellationToken)
        {
            await RetrieveParentseriesForSeries();
        }
        #endregion
    }
}