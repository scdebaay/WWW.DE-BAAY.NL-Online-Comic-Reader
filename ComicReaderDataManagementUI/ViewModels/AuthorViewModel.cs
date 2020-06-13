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
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ComicReaderDataManagementUI.ViewModels
{
    public class AuthorViewModel : Screen, IAuthorViewModel, IHandle<ComicChangedOnMainEvent>
    {
        #region injected objects
        private readonly IConfiguration _configuration;
        private readonly ISqlUiDbConnection _sqlUiDbConnection;
        private readonly ILogger _logger;
        private IEventAggregator _eventAggregator;
        #endregion        

        #region (backed) fields
        private bool newAuthorAdded { get; set; } = false;

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
        private ICommand _addComicToAuthorCommand;
        public ICommand AddComicToAuthorCommand
        {
            get
            {
                if (_addComicToAuthorCommand == null)
                    _addComicToAuthorCommand = new ParameteredComicDelegatedCommand(AddComicToAuthor);
                return _addComicToAuthorCommand;
            }
        }

        private ICommand _newAuthorCommand;
        public ICommand NewAuthorCommand
        {
            get
            {
                if (_newAuthorCommand == null)
                    _newAuthorCommand = new ComicDelegatedCommand(NewAuthor);
                return _newAuthorCommand;
            }
        }

        private ICommand _saveAuthorCommand;
        public ICommand SaveAuthorCommand
        {
            get
            {
                if (_saveAuthorCommand == null)
                    _saveAuthorCommand = new ComicDelegatedCommand(SaveAuthor);
                return _saveAuthorCommand;
            }
        }

        private ICommand _deleteAuthorCommand;
        public ICommand DeleteAuthorCommand
        {
            get
            {
                if (_deleteAuthorCommand == null)
                    _deleteAuthorCommand = new ComicDelegatedCommand(DeleteAuthor);
                return _deleteAuthorCommand;
            }
        }

        private AuthorDataModel _selectedItem = new AuthorDataModel();
        public AuthorDataModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                if (SelectedItem != null)
                {
                    StatusBar = $"Selected author Id is {SelectedItem.Id}";
                    FirstName = SelectedItem.FirstName;
                    MiddleName = SelectedItem.MiddleName;
                    LastName = SelectedItem.LastName;
                    DateBirth = SelectedItem.DateBirth;
                    DateDeceased = SelectedItem.DateDeceased;
                    Active = SelectedItem.Active;
                }
                else
                {
                    FirstName = "Select an author";
                    StatusBar = $"";
                }
                NotifyOfPropertyChange(nameof(SelectedItem));
            }
        }
        private string _firstName = "Select an author";
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                NotifyOfPropertyChange(nameof(FirstName));
            }
        }
        private string _middleName = "";
        public string MiddleName
        {
            get { return _middleName; }
            set
            {
                _middleName = value;
                NotifyOfPropertyChange(nameof(MiddleName));
            }
        }
        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                NotifyOfPropertyChange(nameof(LastName));
            }
        }
        private DateTime _dateBirth;
        public DateTime DateBirth
        {
            get { return _dateBirth; }
            set
            {
                _dateBirth = value;
                NotifyOfPropertyChange(nameof(DateBirth));
            }
        }
        private DateTime _dateDeceased;
        public DateTime DateDeceased
        {
            get { return _dateDeceased; }
            set
            {
                _dateDeceased = value;
                NotifyOfPropertyChange(nameof(DateDeceased));
            }
        }
        private bool _active;
        public bool Active
        {
            get { return _active; }
            set
            {
                _active = value;
                NotifyOfPropertyChange(nameof(Active));
            }
        }

        public ComicDataModel SelectedComicByAuthor { get; set; } = new ComicDataModel();

        private BindableCollection<AuthorDataModel> _authorBox = new BindableCollection<AuthorDataModel>();
        public BindableCollection<AuthorDataModel> AuthorBox
        {
            get { return _authorBox; }
            private set
            {
                _authorBox = value;
                NotifyOfPropertyChange(nameof(AuthorBox));
            }
        }

        private BindableCollection<ComicDataModel> _comicsByAuthorBox = new BindableCollection<ComicDataModel>();
        public BindableCollection<ComicDataModel> ComicsByAuthorBox
        {
            get { return _comicsByAuthorBox; }
            private set
            {
                _comicsByAuthorBox = value;
                NotifyOfPropertyChange(nameof(ComicsByAuthorBox));
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
        public AuthorViewModel(IConfiguration configuration, ISqlUiDbConnection sqlUiDbConnection, ILogger<AuthorViewModel> logger, IEventAggregator eventAggregator)
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
                    _ = RetrieveComicsByAuthor();
                }
            };
            _ = RetrieveAuthors();
        }
        #endregion

        #region private methods
        private async Task RetrieveAuthors()
        {
            AuthorDataModel selectedAuthor = SelectedItem as AuthorDataModel;
            AuthorBox.Clear();
            var list = await _sqlUiDbConnection.RetrieveAuthorAsync();
            if (selectedAuthor != null)
            {
                foreach (AuthorDataModel author in list)
                {
                    if (selectedAuthor == author)
                    {
                        SelectedItem = selectedAuthor;
                    }
                }
            }
            AuthorBox.AddRange(list);
            SelectedItem = AuthorBox[0];
            await RetrieveComicsByAuthor();
        }

        private async Task RetrieveComicsByAuthor()
        {
            ComicsByAuthorBox.Clear();
            ComicList.Clear();
            var comiclist = await _sqlUiDbConnection.RetrieveComicsAsync("");
            var authoridlist = await _sqlUiDbConnection.RetrieveComicsByAuthorIdAsync(SelectedItem.Id);
            var authorlist = new List<ComicDataModel>();
            if (authoridlist != null)
            {
                authorlist = comiclist.Where(comic => authoridlist.Contains(comic.Id)).AsList();
                SelectedComicByAuthor = authorlist.FirstOrDefault();
            }
            ComicsByAuthorBox.AddRange(authorlist);
            ComicList.AddRange(comiclist);
        }

        private void AddComicToAuthor(object selectedComicCollection)
        {
            List<ComicToAuthorsDataModel> comicToAuthorAssoc = new List<ComicToAuthorsDataModel>();

            foreach (ComicDataModel comic in selectedComicCollection as IList)
            {
                comicToAuthorAssoc.Add(new ComicToAuthorsDataModel { ComicId = comic.Id, AuthorId = SelectedItem.Id });
            }

            var succeeded = _sqlUiDbConnection.SaveAuthorAssoc(comicToAuthorAssoc);
            if (succeeded == true)
            {
                IList items = (IList)selectedComicCollection;
                var comicsAdded = items.Cast<ComicDataModel>().ToList();
                ComicsByAuthorBox.AddRange(comicsAdded.Except(ComicsByAuthorBox.ToList()));
                _eventAggregator.PublishOnUIThreadAsync(new ComicChangedOnDialogEvent());
                StatusBar = $"Comic(s) added to author";
            }
            else
            {
                IList items = (IList)selectedComicCollection;
                var comicsNotAdded = items.Cast<ComicDataModel>();
                ComicsByAuthorBox.RemoveRange(comicsNotAdded);
                StatusBar = $"Add comic(s) failed, check log";
            }
        }

        private void NewAuthor()
        {
            var lastId = AuthorBox.OrderByDescending(i => i.Id).First().Id + 1;
            SelectedItem = new AuthorDataModel
            {
                Id = lastId,
                FirstName = "New author",
                LastName = "",
                MiddleName = "",
                DateBirth = DateTime.Now,
                DateDeceased = DateTime.Now,
            };
            AuthorBox.Add(SelectedItem);
            newAuthorAdded = true;
        }

        private void SaveAuthor()
        {
            int authorToUpdatePos = AuthorBox.IndexOf(SelectedItem);
            if (newAuthorAdded == false)
            {
                var authorUpdated = _sqlUiDbConnection.UpdateAuthor(SelectedItem.Id, SelectedItem.FirstName, SelectedItem.MiddleName, SelectedItem.LastName, SelectedItem.DateBirth, SelectedItem.DateDeceased, SelectedItem.Active);
                AuthorDataModel authorToUpdate = new AuthorDataModel
                {
                    Id = SelectedItem.Id,
                    FirstName = SelectedItem.FirstName,
                    MiddleName = SelectedItem.MiddleName,
                    LastName = SelectedItem.LastName,
                    DateBirth = SelectedItem.DateBirth,
                    DateDeceased = SelectedItem.DateDeceased,
                    Active = SelectedItem.Active
                };
                if (authorUpdated == true)
                {
                    AuthorBox.RemoveAt(authorToUpdatePos);
                    AuthorBox.Insert(authorToUpdatePos, authorToUpdate);
                    SelectedItem = AuthorBox[authorToUpdatePos];
                    _eventAggregator.PublishOnUIThreadAsync(new ComicChangedOnDialogEvent());
                    StatusBar = $"Author updated";
                }
                else
                {
                    StatusBar = $"Author update failed, check log";
                }
            }
            else
            {
                var newAuthorId = _sqlUiDbConnection.InsertAuthor(FirstName, MiddleName, LastName, DateBirth, DateDeceased, Active);
                SelectedItem.Id = newAuthorId;
                if (SelectedItem.Id == 0)
                {
                    StatusBar = $"Author not saved to the database, check log";
                }
                else if (SelectedItem.Id == 1)
                {
                    StatusBar = $"Author existed in the database";
                }
                newAuthorAdded = false;
            }
            _ = RetrieveAuthors();
        }

        private void DeleteAuthor()
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                int genreToDeletePos = AuthorBox.IndexOf(SelectedItem);
                var authorDeleted = _sqlUiDbConnection.DeleteAuthor(SelectedItem.Id);
                if (authorDeleted == true)
                {
                    AuthorBox.RemoveAt(genreToDeletePos);
                    SelectedItem = AuthorBox[0];
                    _eventAggregator.PublishOnUIThreadAsync(new ComicChangedOnDialogEvent());
                    StatusBar = $"Author deleted";
                }
                else
                {
                    StatusBar = $"Author deletion failed, check log";
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
                    _ = RetrieveComicsByAuthor();
                }
            };
            return base.OnDeactivateAsync(close, cancellationToken);
        }
        async public Task HandleAsync(ComicChangedOnMainEvent message, CancellationToken cancellationToken)
        {
            await RetrieveComicsByAuthor();
        }
        #endregion
    }
}