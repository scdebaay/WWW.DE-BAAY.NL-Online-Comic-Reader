using Caliburn.Micro;
using ComicReaderClassLibrary.DataAccess.DataModels;
using ComicReaderClassLibrary.DataAccess.Implementations;
using ComicReaderDataManagementUI.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ComicReaderDataManagementUI.ViewModels
{
    public class MainViewModel : Conductor<object>.Collection.OneActive, IHandle<ComicChangedOnDialogEvent>
    {
        #region Injected objects
        private readonly IConfiguration _configuration;
        private readonly ISqlUiDbConnection _sqlUiDbConnection;
        private readonly ILogger _logger;
        private ILanguageViewModel _languageViewModel;
        private ITypeViewModel _typeViewModel;
        private ISeriesViewModel _seriesViewModel;
        private ISubSeriesViewModel _subSeriesViewModel;
        private IGenreViewModel _genreViewModel;
        private IPublisherViewModel _publisherViewModel;
        private IAuthorViewModel _authorViewModel;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        public IEventAggregator _eventAggregator;
        public IWindowManager _windowManager;
        #endregion

        #region Backed fields
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

        private ComicDataModel _selectedItem = new ComicDataModel();
        public ComicDataModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                if (SelectedItem != null)
                {
                    StatusBar = $"Selected comic Id: {SelectedItem.Id}";
                    //_logger.LogInformation($"Selected comic is {_selectedItem.Name}");                
                    _ = RetrieveAuthors(SelectedItem.Id);
                    _ = RetrieveGenres(SelectedItem.Id);
                    _ = RetrievePublishers(SelectedItem.Id);
                    _ = RetrieveLanguages();
                    _ = RetrieveTypes();
                    _ = RetrieveSeries();
                    _ = RetrieveSubSeries();
                    NotifyOfPropertyChange(() => SelectedItem);
                    NotifyOfPropertyChange(() => TitleInput);
                    NotifyOfPropertyChange(() => ThumbUrl);
                    NotifyOfPropertyChange(() => TotalPagesInput);
                    NotifyOfPropertyChange(() => PathInput);
                    NotifyOfPropertyChange(() => PublicationDatePicker);
                    NotifyOfPropertyChange(() => ReleaseDatePicker);
                }
            }
        }

        private AuthorDataModel _selectedAuthor = new AuthorDataModel();
        public AuthorDataModel SelectedAuthor
        {
            get { return _selectedAuthor; }
            set
            {
                _selectedAuthor = value;
                NotifyOfPropertyChange(() => SelectedItem);
                NotifyOfPropertyChange(nameof(AuthorList));
            }
        }

        private GenreDataModel _selectedGenre = new GenreDataModel();
        public GenreDataModel SelectedGenre
        {
            get { return _selectedGenre; }
            set
            {
                _selectedGenre = value;
                NotifyOfPropertyChange(() => SelectedItem);
                NotifyOfPropertyChange(nameof(GenreList));
            }
        }

        private PublisherDataModel _selectedPublisher = new PublisherDataModel();
        public PublisherDataModel SelectedPublisher
        {
            get { return _selectedPublisher; }
            set
            {
                _selectedPublisher = value;
                NotifyOfPropertyChange(() => SelectedItem);
                NotifyOfPropertyChange(nameof(PublisherBox));
            }
        }
        public LanguageDataModel SelectedLanguage
        {
            get;
            set;
        }
        public TypeDataModel SelectedType
        {
            get;
            set;
        }
        public SeriesDataModel SelectedSerie
        {
            get;
            set;
        }
        public SubSeriesDataModel SelectedSubSerie
        {
            get;
            set;
        }

        private BindableCollection<ComicDataModel> _comicList = new BindableCollection<ComicDataModel>();
        public BindableCollection<ComicDataModel> ComicList
        {
            get
            {
                return _comicList;
            }
            set
            {
                _comicList = value;
                NotifyOfPropertyChange(nameof(ComicList));
            }
        }
        private BindableCollection<AuthorDataModel> _authorList = new BindableCollection<AuthorDataModel>();
        public BindableCollection<AuthorDataModel> AuthorList
        {
            get
            {
                return _authorList;
            }
            set
            {
                _authorList = value;
                NotifyOfPropertyChange(nameof(AuthorList));
            }
        }
        private BindableCollection<GenreDataModel> _genreList = new BindableCollection<GenreDataModel>();
        public BindableCollection<GenreDataModel> GenreList
        {
            get
            {
                return _genreList;
            }
            set
            {
                _genreList = value;
                NotifyOfPropertyChange(nameof(GenreList));
            }
        }
        private BindableCollection<LanguageDataModel> _languageBox = new BindableCollection<LanguageDataModel>();
        public BindableCollection<LanguageDataModel> LanguageBox
        {
            get
            {
                return _languageBox;
            }
            set
            {
                _languageBox = value;
                NotifyOfPropertyChange(nameof(LanguageBox));
            }
        }
        private BindableCollection<TypeDataModel> _typeBox = new BindableCollection<TypeDataModel>();
        public BindableCollection<TypeDataModel> TypeBox
        {
            get
            {
                return _typeBox;
            }
            set
            {
                _typeBox = value;
                NotifyOfPropertyChange(nameof(TypeBox));
            }
        }
        private BindableCollection<SeriesDataModel> _seriesBox = new BindableCollection<SeriesDataModel>();
        public BindableCollection<SeriesDataModel> SeriesBox
        {
            get
            {
                return _seriesBox;
            }
            set
            {
                _seriesBox = value;
                NotifyOfPropertyChange(nameof(SeriesBox));
            }
        }
        private BindableCollection<SubSeriesDataModel> _subSeriesBox = new BindableCollection<SubSeriesDataModel>();
        public BindableCollection<SubSeriesDataModel> SubSeriesBox
        {
            get
            {
                return _subSeriesBox;
            }
            set
            {
                _subSeriesBox = value;
                NotifyOfPropertyChange(nameof(SubSeriesBox));
            }
        }
        private BindableCollection<PublisherDataModel> _publisherBox = new BindableCollection<PublisherDataModel>();
        public BindableCollection<PublisherDataModel> PublisherBox
        {
            get
            {
                return _publisherBox;
            }
            set
            {
                _publisherBox = value;
                NotifyOfPropertyChange(nameof(PublisherBox));
            }
        }
        #endregion

        #region constructor
        public MainViewModel(IConfiguration configuration, 
                            ISqlUiDbConnection sqlUiDbConnection, 
                            ILogger<MainViewModel> logger, 
                            ILanguageViewModel languageViewModel, 
                            ITypeViewModel typeViewModel,
                            ISeriesViewModel seriesViewModel,
                            ISubSeriesViewModel subSeriesViewModel,
                            IGenreViewModel genreViewModel,
                            IPublisherViewModel publisherViewModel,
                            IAuthorViewModel authorViewModel,
                            IEventAggregator eventAggregator, 
                            IWindowManager windowManager)
        {
            _configuration = configuration;
            _sqlUiDbConnection = sqlUiDbConnection;
            _logger = logger;
            _languageViewModel = languageViewModel;
            _typeViewModel = typeViewModel;
            _seriesViewModel = seriesViewModel;
            _subSeriesViewModel = subSeriesViewModel;
            _genreViewModel = genreViewModel;
            _publisherViewModel = publisherViewModel;
            _authorViewModel = authorViewModel;
            _eventAggregator = eventAggregator;
            _windowManager = windowManager;
            _eventAggregator.SubscribeOnPublishedThread(this);
            _ = RetrieveComics();
        }
        #endregion

        #region private methods
        private async Task RetrieveComics()
        {
            ComicList.Clear();
            var list = await _sqlUiDbConnection.RetrieveComicsAsync();
            ComicList.AddRange(list);
            SelectedItem = ComicList[0];
        }

        private async Task RetrieveAuthors(int id)
        {
            AuthorList.Clear();
            var list = await _sqlUiDbConnection.RetrieveAuthorByComicIdAsync(id);
            AuthorList.AddRange(list);
        }

        private async Task RetrieveGenres(int id)
        {
            GenreList.Clear();
            var list = await _sqlUiDbConnection.RetrieveGenreByComicIdAsync(id);
            GenreList.AddRange(list);
        }

        private async Task RetrievePublishers(int id)
        {
            PublisherBox.Clear();
            var list = await _sqlUiDbConnection.RetrievePublisherByComicIdAsync(id);
            PublisherBox.AddRange(list);
            SelectedPublisher = list[0];
            NotifyOfPropertyChange(nameof(SelectedPublisher));
        }

        private async Task RetrieveLanguages()
        {
            LanguageBox.Clear();
            var list = await _sqlUiDbConnection.RetrieveLanguageAsync();
            LanguageBox.AddRange(list);
            var l = list.Where(x => x.Id == SelectedItem.LanguageId);
            SelectedLanguage = l.FirstOrDefault();
            NotifyOfPropertyChange(nameof(SelectedLanguage));
        }

        private async Task RetrieveTypes()
        {
            TypeBox.Clear();
            var list = await _sqlUiDbConnection.RetrieveTypeAsync();
            TypeBox.AddRange(list);
            var l = list.Where(x => x.Id == SelectedItem.TypeId);
            SelectedType = l.FirstOrDefault();
            NotifyOfPropertyChange(nameof(SelectedType));
        }

        private async Task RetrieveSeries()
        {
            SeriesBox.Clear();
            var list = await _sqlUiDbConnection.RetrieveSerieAsync();
            SeriesBox.AddRange(list);
            var l = list.Where(x => x.Id == SelectedItem.SeriesId);
            SelectedSerie = l.FirstOrDefault();
            NotifyOfPropertyChange(nameof(SelectedSerie));
        }

        private async Task RetrieveSubSeries()
        {
            SubSeriesBox.Clear();
            var list = await _sqlUiDbConnection.RetrieveSubSerieAsync();
            SubSeriesBox.AddRange(list);
            var l = list.Where(x => x.Id == SelectedItem.SubSeriesId);
            SelectedSubSerie = l.FirstOrDefault();
            NotifyOfPropertyChange(nameof(SelectedSubSerie));
        }
        #endregion

        #region public methods
        public async Task EditLanguageAsync()
        {
            await _windowManager.ShowWindowAsync(_languageViewModel);
        }
        public async Task EditTypeAsync()
        {
            await _windowManager.ShowWindowAsync(_typeViewModel);
        }
        public async Task EditSeriesAsync()
        {
            await _windowManager.ShowWindowAsync(_seriesViewModel);
        }
        public async Task EditSubSeriesAsync()
        {
            await _windowManager.ShowWindowAsync(_subSeriesViewModel);
        }
        public async Task EditGenreAsync()
        {
            await _windowManager.ShowWindowAsync(_genreViewModel);
        }
        public async Task EditPublisherAsync()
        {
            await _windowManager.ShowWindowAsync(_publisherViewModel);
        }
        public async Task EditAuthorAsync()
        {
            await _windowManager.ShowWindowAsync(_authorViewModel);
        }

        public void SaveComic()
        {
            if (SelectedLanguage == null)
            {
                SelectedItem.LanguageId = 1;
            }
            else
            {
                SelectedItem.LanguageId = SelectedLanguage.Id;
            }
            if (SelectedType == null)
            {
                SelectedItem.TypeId = 1;
            }
            else
            {
                SelectedItem.TypeId = SelectedType.Id;
            }
            if (SelectedSerie == null)
            {
                SelectedItem.SeriesId = 1;
            }
            else
            {
                SelectedItem.SeriesId = SelectedSerie.Id;
            }
            if (SelectedSubSerie == null)
            {
                SelectedItem.SubSeriesId = 1;
            }
            else
            {
                SelectedItem.SubSeriesId = SelectedSubSerie.Id;
            }
            var selecteditemsucceeded = _sqlUiDbConnection.SaveComic(SelectedItem);

            bool authorlistsucceeded = true;
            bool publisherlistsucceeded = true;
            bool genrelistsucceeded = true;
            if (AuthorList.Count > 0)
            {
                List<ComicToAuthorsDataModel> comicToAuthorAssoc = new List<ComicToAuthorsDataModel>();
                for (int i = 0; i < AuthorList.Count; i++)
                {
                    comicToAuthorAssoc.Add(new ComicToAuthorsDataModel { ComicId = SelectedItem.Id, AuthorId = AuthorList[i].Id });
                }
                authorlistsucceeded = _sqlUiDbConnection.SaveAuthorAssoc(comicToAuthorAssoc);
            }

            if (PublisherBox.Count > 0)
            {
                List<ComicToPublishersDataModel> comicToPublisherAssoc = new List<ComicToPublishersDataModel>();
                for (int i = 0; i < PublisherBox.Count; i++)
                {
                    comicToPublisherAssoc.Add(new ComicToPublishersDataModel { ComicId = SelectedItem.Id, PublisherId = PublisherBox[i].Id });
                }
                publisherlistsucceeded = _sqlUiDbConnection.SavePublisherAssoc(comicToPublisherAssoc);
            }

            if (GenreList.Count > 0)
            {
                List<ComicToGenreDataModel> comicToGenreAssoc = new List<ComicToGenreDataModel>();
                for (int i = 0; i < GenreList.Count; i++)
                {
                    comicToGenreAssoc.Add(new ComicToGenreDataModel { ComicId = SelectedItem.Id, GenreId = GenreList[i].Id });
                }
                genrelistsucceeded = _sqlUiDbConnection.SaveGenreAssoc(comicToGenreAssoc);
            }
            bool succeeded = selecteditemsucceeded && authorlistsucceeded && publisherlistsucceeded && genrelistsucceeded;
            if (succeeded == true)
            { StatusBar = $"Comic Saved"; }
            else
            { StatusBar = $"Save Failed, check log"; ; }
            _eventAggregator.PublishOnUIThreadAsync(new ComicChangedOnMainEvent());
        }

        public async Task HandleAsync(ComicChangedOnDialogEvent message, CancellationToken cancellationToken)
        {
            await RetrieveComics();
        }
        #endregion

        #region public properties
        public string ThumbUrl
        {
            get
            {
                return $"https://develop.de-baay.nl/ComicCloudDev/comic{SelectedItem.Path}?size=150";
            }
            set
            {
                NotifyOfPropertyChange(nameof(ThumbUrl));
            }
        }
        public string TitleInput
        {
            get
            {
                if (!string.IsNullOrEmpty(SelectedItem.Name))
                {
                    return SelectedItem.Name[0..^4];
                }
                else
                {
                    return "Loading...";
                }
            }
            set
            {
                _selectedItem.Name = value;
                NotifyOfPropertyChange(nameof(TitleInput));
            }
        }
        public string PathInput
        {
            get
            {
                return SelectedItem.Path;
            }
            set
            {
                _selectedItem.Path = value;
                NotifyOfPropertyChange(() => PathInput);
            }
        }
        public int TotalPagesInput
        {
            get
            {
                return SelectedItem.TotalPages;
            }
            set
            {
                _selectedItem.TotalPages = value;
                NotifyOfPropertyChange(() => TotalPagesInput);
            }
        }
        public DateTime PublicationDatePicker
        {
            get
            {
                return SelectedItem.PublicationDate;
            }
            set
            {
                _selectedItem.PublicationDate = value;
                NotifyOfPropertyChange(() => PublicationDatePicker);
            }
        }
        public DateTime ReleaseDatePicker
        {
            get
            {
                return SelectedItem.ReleaseDate;
            }
            set
            {
                _selectedItem.ReleaseDate = value;
                NotifyOfPropertyChange(() => ReleaseDatePicker);
            }
        }
        #endregion
    }
}
