using Caliburn.Micro;
using ComicReaderClassLibrary.DataAccess.DataModels;
using ComicReaderClassLibrary.DataAccess.Implementations;
using ComicReaderDataManagementUI.Events;
using ComicReaderDataManagementUI.ViewModels.Commands;
using Dapper;
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
    public class GenreViewModel : Screen, IGenreViewModel, IHandle<ComicChangedOnMainEvent>
    {
        #region Injected objects
        private readonly IConfiguration _configuration;
        private readonly ISqlUiDbConnection _sqlUiDbConnection;
        private readonly ILogger _logger;
        private IEventAggregator _eventAggregator;
        #endregion

        #region (backed) fields
        private bool newGenreAdded { get; set; } = false;
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
        private ICommand _addComicToGenreCommand;
        public ICommand AddComicToGenreCommand
        {
            get
            {
                if (_addComicToGenreCommand == null)
                    _addComicToGenreCommand = new ParameteredComicDelegatedCommand(AddComicToGenre);
                return _addComicToGenreCommand;
            }
        }

        private ICommand _newGenreCommand;
        public ICommand NewGenreCommand
        {
            get 
            {
                if (_newGenreCommand == null)
                    _newGenreCommand = new ComicDelegatedCommand(NewGenre);
                return _newGenreCommand; 
            }
        }

        private ICommand _saveGenreCommand;
        public ICommand SaveGenreCommand
        {
            get
            {
                if (_saveGenreCommand == null)
                    _saveGenreCommand = new ComicDelegatedCommand(SaveGenre);
                return _saveGenreCommand;
            }
        }

        private ICommand _deleteGenreCommand;
        public ICommand DeleteGenreCommand
        {
            get
            {
                if (_deleteGenreCommand == null)
                    _deleteGenreCommand = new ComicDelegatedCommand(DeleteGenre);
                return _deleteGenreCommand;
            }
        }


        private GenreDataModel _selectedItem = new GenreDataModel();
        public GenreDataModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                if (SelectedItem != null)
                {
                    StatusBar = $"Selected publisher Id is {SelectedItem.Id}";
                    GenreName = SelectedItem.Term;
                }
                else
                {
                    GenreName = "Select a type";
                    StatusBar = $"";
                }
                NotifyOfPropertyChange(nameof(SelectedItem));
            }
        }
        private string _genreName = "Select a genre";
        public string GenreName
        {
            get { return _genreName; }
            set
            {
                _genreName = value;
                NotifyOfPropertyChange(nameof(GenreName));
            }
        }
        public ComicDataModel SelectedComicInGenre { get; set; } = new ComicDataModel();

        private BindableCollection<GenreDataModel> _genreBox = new BindableCollection<GenreDataModel>();
        public BindableCollection<GenreDataModel> GenreBox
        {
            get { return _genreBox; }
            private set
            {
                _genreBox = value;
                NotifyOfPropertyChange(nameof(GenreBox));
            }
        }

        private BindableCollection<ComicDataModel> _comicsInGenreBox = new BindableCollection<ComicDataModel>();
        public BindableCollection<ComicDataModel> ComicsInGenreBox
        {
            get { return _comicsInGenreBox; }
            private set
            {
                _comicsInGenreBox = value;
                NotifyOfPropertyChange(nameof(ComicsInGenreBox));
            }
        }

        public BindableCollection<ComicDataModel> _comicList = new BindableCollection<ComicDataModel>();
        public BindableCollection<ComicDataModel> ComicList
        {
            get { return _comicList; }
            private set
            {
                _comicList = value;
                NotifyOfPropertyChange(nameof(ComicList));
            }
        }
        #endregion

        #region constructor
        public GenreViewModel(IConfiguration configuration, ISqlUiDbConnection sqlUiDbConnection, ILogger<GenreViewModel> logger, IEventAggregator eventAggregator)
        {
            _configuration = configuration;
            _sqlUiDbConnection = sqlUiDbConnection;
            _logger = logger;
            _eventAggregator = eventAggregator;
            _eventAggregator.SubscribeOnPublishedThread(this);
            PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "SelectedItem" && SelectedItem != null)
                {
                    _ = RetrieveComicsForGenre();
                }
            };
            _ = RetrieveGenres();
        }
        #endregion

        #region private methods
        private async Task RetrieveGenres()
        {
            GenreDataModel selectedGenre = SelectedItem as GenreDataModel;
            GenreBox.Clear();
            var list = await _sqlUiDbConnection.RetrieveGenreAsync();
            if (selectedGenre != null)
            {
                foreach (GenreDataModel genre in list)
                {
                    if (selectedGenre == genre)
                    {
                        SelectedItem = selectedGenre;
                    }
                }
            }
            GenreBox.AddRange(list);
            SelectedItem = GenreBox[0];
            await RetrieveComicsForGenre();
        }

        private async Task RetrieveComicsForGenre()
        {
            ComicsInGenreBox.Clear();
            ComicList.Clear();
            var comiclist = await _sqlUiDbConnection.RetrieveComicsAsync("");
            var genreidlist = await _sqlUiDbConnection.RetrieveComicsByGenreIdAsync(SelectedItem.Id);
            var genrelist = new List<ComicDataModel>();
            if (genreidlist != null)
            {
                genrelist = comiclist.Where(comic => genreidlist.Contains(comic.Id)).AsList();
                SelectedComicInGenre = genrelist.FirstOrDefault();
            }
            ComicsInGenreBox.AddRange(genrelist);
            ComicList.AddRange(comiclist);
        }

        private void AddComicToGenre(object selectedComicCollection)
        {
            List<ComicToGenreDataModel> comicToGenreAssoc = new List<ComicToGenreDataModel>();

            foreach (ComicDataModel comic in selectedComicCollection as IList)
            {
                comicToGenreAssoc.Add(new ComicToGenreDataModel { ComicId = comic.Id, GenreId = SelectedItem.Id });
            }

            var succeeded = _sqlUiDbConnection.SaveGenreAssoc(comicToGenreAssoc);
            if (succeeded == true)
            {
                IList items = (IList)selectedComicCollection;
                var comicsAdded = items.Cast<ComicDataModel>().ToList();
                ComicsInGenreBox.AddRange(comicsAdded.Except(ComicsInGenreBox.ToList()));
                _eventAggregator.PublishOnUIThreadAsync(new ComicChangedOnDialogEvent());
                StatusBar = $"Comic(s) added to genre";
            }
            else
            {
                IList items = (IList)selectedComicCollection;
                var comicsNotAdded = items.Cast<ComicDataModel>();
                ComicsInGenreBox.RemoveRange(comicsNotAdded);
                StatusBar = $"Add comic(s) failed, check log";
            }
        }

        private void NewGenre()
        {
            var lastId = GenreBox.OrderByDescending(i => i.Id).First().Id + 1;
            SelectedItem = new GenreDataModel
            {
                Id = lastId,
                Term = "New genre"
            };
            GenreBox.Add(SelectedItem);
            newGenreAdded = true;
        }

        private void SaveGenre()
        {
            int genreToUpdatePos = GenreBox.IndexOf(SelectedItem);
            if (newGenreAdded == false)
            {
                var genreUpdated = _sqlUiDbConnection.UpdateGenre(SelectedItem.Id, GenreName);
                GenreDataModel genreToUpdate = new GenreDataModel
                {
                    Id = SelectedItem.Id,
                    Term = GenreName
                };
                if (genreUpdated == true)
                {
                    GenreBox.RemoveAt(genreToUpdatePos);
                    GenreBox.Insert(genreToUpdatePos, genreToUpdate);
                    SelectedItem = GenreBox[genreToUpdatePos];
                    _eventAggregator.PublishOnUIThreadAsync(new ComicChangedOnDialogEvent());
                    StatusBar = $"Genre updated";
                }
                else
                {
                    StatusBar = $"Genre update failed, check log";
                }
            }
            else
            {
                var newGenreId = _sqlUiDbConnection.InsertGenre(GenreName);
                SelectedItem.Id = newGenreId;
                if (SelectedItem.Id == 0)
                {
                    StatusBar = $"Genre not saved to the database, check log";
                }
                else if (SelectedItem.Id == 1)
                {
                    StatusBar = $"Genre existed in the database";
                }
                newGenreAdded = false;
            }
            _ = RetrieveGenres();
        }

        private void DeleteGenre()
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                int genreToDeletePos = GenreBox.IndexOf(SelectedItem);
                var genreDeleted = _sqlUiDbConnection.DeleteGenre(SelectedItem.Id);
                if (genreDeleted == true)
                {
                    GenreBox.RemoveAt(genreToDeletePos);
                    SelectedItem = GenreBox[0];
                    _eventAggregator.PublishOnUIThreadAsync(new ComicChangedOnDialogEvent());
                    StatusBar = $"Genre deleted";
                }
                else
                {
                    StatusBar = $"Genre deletion failed, check log";
                }
            }
        }
        #endregion

        #region public methods        
        #endregion

        #region aux methods
        protected override Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            PropertyChanged -= (sender, args) =>
            {
                if (args.PropertyName == "SelectedItem" && SelectedItem != null)
                {
                    _ = RetrieveComicsForGenre();
                }
            };
            return base.OnDeactivateAsync(close, cancellationToken);
        }
        async public Task HandleAsync(ComicChangedOnMainEvent message, CancellationToken cancellationToken)
        {
            await RetrieveComicsForGenre();
        }
        #endregion
    }
}