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
    public class LanguageViewModel : Screen, ILanguageViewModel, IHandle<ComicChangedOnMainEvent>
    {
        #region injected objects
        private readonly IConfiguration _configuration;
        private readonly ISqlUiDbConnection _sqlUiDbConnection;
        private readonly ILogger _logger;
        private IEventAggregator _eventAggregator;
        #endregion

        #region (backed) fields
        private bool newLanguageAdded { get; set; } = false;
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


        private LanguageDataModel _selectedItem = new LanguageDataModel();
        public LanguageDataModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                if (SelectedItem != null)
                {
                    StatusBar = $"Selected language Id is {SelectedItem.Id}";
                    LanguageName = SelectedItem.Term;
                }
                else
                {
                    LanguageName = "Select a language";
                    StatusBar = $"";
                }
                NotifyOfPropertyChange(nameof(SelectedItem));
            }
        }
        private string _languageName = "Select a language";
        public string LanguageName
        {
            get { return _languageName; }
            set
            {
                _languageName = value;
                NotifyOfPropertyChange(nameof(LanguageName));
            }
        }
        public ComicDataModel SelectedComic { get; set; } = new ComicDataModel();
        public ComicDataModel SelectedComicInLanguage { get; set; } = new ComicDataModel();

        private BindableCollection<LanguageDataModel> _languageBox = new BindableCollection<LanguageDataModel>();
        public BindableCollection<LanguageDataModel> LanguageBox
        {
            get { return _languageBox; }
            private set
            {
                _languageBox = value;
                NotifyOfPropertyChange(nameof(LanguageBox));
            }
        }

        private BindableCollection<ComicDataModel> _comicsInLanguageBox = new BindableCollection<ComicDataModel>();
        public BindableCollection<ComicDataModel> ComicsInLanguageBox
        {
            get { return _comicsInLanguageBox; }
            private set
            {
                _comicsInLanguageBox = value;
                NotifyOfPropertyChange(nameof(ComicsInLanguageBox));
            }
        }

        public BindableCollection<ComicDataModel> _comicBox = new BindableCollection<ComicDataModel>();
        public BindableCollection<ComicDataModel> ComicBox
        {
            get { return _comicBox; }
            private set
            {
                _comicBox = value;
                NotifyOfPropertyChange(nameof(ComicBox));
            }
        }
        #endregion

        #region constructor
        public LanguageViewModel(IConfiguration configuration, ISqlUiDbConnection sqlUiDbConnection, ILogger<LanguageViewModel> logger, IEventAggregator eventAggregator)
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
                    _ = RetrieveComicsForLanguage();
                }
            };
            _ = RetrieveLanguages();
        }
        #endregion

        #region private methods
        private async Task RetrieveLanguages()
        {
            LanguageDataModel selectedLanguage = SelectedItem as LanguageDataModel;
            LanguageBox.Clear();
            var list = await _sqlUiDbConnection.RetrieveLanguageAsync();
            if (selectedLanguage != null)
            {
                foreach (LanguageDataModel lang in list)
                {
                    if (selectedLanguage == lang)
                    {
                        SelectedItem = selectedLanguage;
                    }
                }
            }
            LanguageBox.AddRange(list);
            SelectedItem = LanguageBox[0];
            await RetrieveComicsForLanguage();
        }

        private async Task RetrieveComicsForLanguage()
        {
            ComicsInLanguageBox.Clear();
            ComicBox.Clear();
            var list = await _sqlUiDbConnection.RetrieveComicsAsync();
            var l = list.Where(x => x.LanguageId == SelectedItem.Id).Select(x => x);
            if (l != null)
                {
                    SelectedComicInLanguage = l.FirstOrDefault();
                }
            ComicsInLanguageBox.AddRange(l);
            ComicBox.AddRange(list);
            SelectedComic = ComicBox[0];
        }
        #endregion

        #region public methods
        public void AddComicToLanguage()
        {
            var succeeded = _sqlUiDbConnection.SaveLanguageAssoc(SelectedComic.Id, SelectedItem.Id);
            if (succeeded == true)
            {
                ComicsInLanguageBox.Add(SelectedComic);
                _eventAggregator.PublishOnUIThreadAsync(new ComicChangedOnDialogEvent());
                StatusBar = $"Comic added to language";
            }
            else
            {
                ComicsInLanguageBox.Remove(SelectedComic);
                StatusBar = $"Add comic failed, check log";
            }
        }

        public void NewLanguage()
        {
            var lastId = LanguageBox.OrderByDescending(i => i.Id).First().Id + 1;
            SelectedItem = new LanguageDataModel
            {
                Id = lastId,
                Term = "New language"
            };
            LanguageBox.Add(SelectedItem);
            newLanguageAdded = true;
        }

        public void SaveLanguage()
        {
            int langToUpdatePos = LanguageBox.IndexOf(SelectedItem);
            if (newLanguageAdded == false)
            {
                var languageUpdated = _sqlUiDbConnection.UpdateLanguage(SelectedItem.Id, LanguageName);
                LanguageDataModel langToUpdate = new LanguageDataModel
                {
                    Id = SelectedItem.Id,
                    Term = LanguageName
                };
                if (languageUpdated == true)
                {
                    LanguageBox.RemoveAt(langToUpdatePos);
                    LanguageBox.Insert(langToUpdatePos, langToUpdate);
                    SelectedItem = LanguageBox[langToUpdatePos];
                    _eventAggregator.PublishOnUIThreadAsync(new ComicChangedOnDialogEvent());
                    StatusBar = $"Language updated";
                }
                else
                {
                    StatusBar = $"Language update failed, check log";
                }
            }
            else
            {
                var newLanguageId = _sqlUiDbConnection.InsertLanguage(LanguageName);
                SelectedItem.Id = newLanguageId;
                if (SelectedItem.Id == 0)
                {
                    StatusBar = $"Language not saved to the database, check log";
                }
                else if (SelectedItem.Id == 1)
                {
                    StatusBar = $"Language existed in the database";
                }
                newLanguageAdded = false;
            }
            _ = RetrieveLanguages();
        }

        public void DeleteLanguage()
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                int langToDeletePos = LanguageBox.IndexOf(SelectedItem);
                var languageDeleted = _sqlUiDbConnection.DeleteLanguage(SelectedItem.Id);
                if (languageDeleted == true)
                {
                    LanguageBox.RemoveAt(langToDeletePos);
                    SelectedItem = LanguageBox[0];
                    _eventAggregator.PublishOnUIThreadAsync(new ComicChangedOnDialogEvent());
                    StatusBar = $"Language deleted";
                }
                else
                {
                    StatusBar = $"Language deletion failed, check log";
                }
            }
        }
        #endregion

        #region event handlers
        protected override Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            PropertyChanged -= (sender, args) =>
            {
                if (args.PropertyName == "SelectedItem" && SelectedItem != null)
                {
                    _ = RetrieveComicsForLanguage();
                }
            };
            return base.OnDeactivateAsync(close, cancellationToken);
        }
        async public Task HandleAsync(ComicChangedOnMainEvent message, CancellationToken cancellationToken)
        {
            await RetrieveComicsForLanguage();
        }
        #endregion
    }
}
