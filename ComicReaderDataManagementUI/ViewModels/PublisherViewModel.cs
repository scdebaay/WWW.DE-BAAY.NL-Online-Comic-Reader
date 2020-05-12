using Caliburn.Micro;
using ComicReaderClassLibrary.DataAccess.DataModels;
using ComicReaderClassLibrary.DataAccess.Implementations;
using ComicReaderDataManagementUI.Events;
using Dapper;
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
    public class PublisherViewModel : Screen, IPublisherViewModel, IHandle<ComicChangedOnMainEvent>
    {
        private readonly IConfiguration _configuration;
        private readonly ISqlUiDbConnection _sqlUiDbConnection;
        private readonly ILogger _logger;
        private IEventAggregator _eventAggregator;
        private bool newPublisherAdded { get; set; } = false;
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


        private PublisherDataModel _selectedItem = new PublisherDataModel();
        public PublisherDataModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                if (SelectedItem != null)
                {
                    StatusBar = $"Selected publisher Id is {SelectedItem.Id}";
                    PublisherName = SelectedItem.Name;
                }
                else
                {
                    PublisherName = "Select a type";
                    StatusBar = $"";
                }
                NotifyOfPropertyChange(nameof(SelectedItem));
            }
        }
        private string _publisherName = "Select a publisher";
        public string PublisherName
        {
            get { return _publisherName; }
            set
            {
                _publisherName = value;
                NotifyOfPropertyChange(nameof(PublisherName));
            }
        }
        public ComicDataModel SelectedComic { get; set; } = new ComicDataModel();
        public ComicDataModel SelectedComicByPublisher { get; set; } = new ComicDataModel();

        private BindableCollection<PublisherDataModel> _publisherBox = new BindableCollection<PublisherDataModel>();
        public BindableCollection<PublisherDataModel> PublisherBox
        {
            get { return _publisherBox; }
            private set
            {
                _publisherBox = value;
                NotifyOfPropertyChange(nameof(PublisherBox));
            }
        }

        private BindableCollection<ComicDataModel> _comicsByPublisherBox = new BindableCollection<ComicDataModel>();
        public BindableCollection<ComicDataModel> ComicsByPublisherBox
        {
            get { return _comicsByPublisherBox; }
            private set
            {
                _comicsByPublisherBox = value;
                NotifyOfPropertyChange(nameof(ComicsByPublisherBox));
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

        public PublisherViewModel(IConfiguration configuration, ISqlUiDbConnection sqlUiDbConnection, ILogger<PublisherViewModel> logger, IEventAggregator eventAggregator)
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
                    _ = RetrieveComicsForPublisher();
                }
            };
            _ = RetrievePublishers();
        }

        private async Task RetrievePublishers()
        {
            PublisherDataModel selectedType = SelectedItem as PublisherDataModel;
            PublisherBox.Clear();
            var list = await _sqlUiDbConnection.RetrievePublisherAsync();
            if (selectedType != null)
            {
                foreach (PublisherDataModel publisher in list)
                {
                    if (selectedType == publisher)
                    {
                        SelectedItem = selectedType;
                    }
                }
            }
            PublisherBox.AddRange(list);
            SelectedItem = PublisherBox[0];
            await RetrieveComicsForPublisher();
        }

        private async Task RetrieveComicsForPublisher()
        {
            ComicsByPublisherBox.Clear();
            ComicBox.Clear();
            var comiclist = await _sqlUiDbConnection.RetrieveComicsAsync();
            var publisheridlist = await _sqlUiDbConnection.RetrieveComicsByPublisherIdAsync(SelectedItem.Id);
            var publisherlist = new List<ComicDataModel>();
            if (publisheridlist != null)
            {
                publisherlist = comiclist.Where(comic => publisheridlist.Contains(comic.Id)).AsList();
                SelectedComicByPublisher = publisherlist.FirstOrDefault();
            }
            ComicsByPublisherBox.AddRange(publisherlist);
            ComicBox.AddRange(comiclist);
            SelectedComic = ComicBox[0];
        }

        public void AddComicToPublisher()
        {
            List<ComicToPublishersDataModel> comicToPublisherAssoc = new List<ComicToPublishersDataModel>();
            comicToPublisherAssoc.Add(new ComicToPublishersDataModel { ComicId = SelectedComic.Id, PublisherId = SelectedItem.Id });
            var succeeded = _sqlUiDbConnection.SavePublisherAssoc(comicToPublisherAssoc);
            if (succeeded == true)
            {
                ComicsByPublisherBox.Add(SelectedComic);
                _eventAggregator.PublishOnUIThreadAsync(new ComicChangedOnDialogEvent());
                StatusBar = $"Comic added to publisher";
            }
            else
            {
                ComicsByPublisherBox.Remove(SelectedComic);
                StatusBar = $"Add comic failed, check log";
            }
        }

        public void NewPublisher()
        {
            var lastId = PublisherBox.OrderByDescending(i => i.Id).First().Id + 1;
            SelectedItem = new PublisherDataModel
            {
                Id = lastId,
                Name = "New publisher"
            };
            PublisherBox.Add(SelectedItem);
            newPublisherAdded = true;
        }

        public void SavePublisher()
        {
            int publisherToUpdatePos = PublisherBox.IndexOf(SelectedItem);
            if (newPublisherAdded == false)
            {
                var publisherUpdated = _sqlUiDbConnection.UpdatePublisher(SelectedItem.Id, PublisherName);
                PublisherDataModel publisherToUpdate = new PublisherDataModel
                {
                    Id = SelectedItem.Id,
                    Name = PublisherName
                };
                if (publisherUpdated == true)
                {
                    PublisherBox.RemoveAt(publisherToUpdatePos);
                    PublisherBox.Insert(publisherToUpdatePos, publisherToUpdate);
                    SelectedItem = PublisherBox[publisherToUpdatePos];
                    _eventAggregator.PublishOnUIThreadAsync(new ComicChangedOnDialogEvent());
                    StatusBar = $"Publisher updated";
                }
                else
                {
                    StatusBar = $"Publisher update failed, check log";
                }
            }
            else
            {
                var newPublisherId = _sqlUiDbConnection.InsertPublisher(PublisherName);
                SelectedItem.Id = newPublisherId;
                if (SelectedItem.Id == 0)
                {
                    StatusBar = $"Publisher not saved to the database, check log";
                }
                else if (SelectedItem.Id == 1)
                {
                    StatusBar = $"Publisher existed in the database";
                }
                newPublisherAdded = false;
            }
            _ = RetrievePublishers();
        }

        public void DeletePublisher()
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                int publisherToDeletePos = PublisherBox.IndexOf(SelectedItem);
                var publisherDeleted = _sqlUiDbConnection.DeletePublisher(SelectedItem.Id);
                if (publisherDeleted == true)
                {
                    PublisherBox.RemoveAt(publisherToDeletePos);
                    SelectedItem = PublisherBox[0];
                    _eventAggregator.PublishOnUIThreadAsync(new ComicChangedOnDialogEvent());
                    StatusBar = $"Publisher deleted";
                }
                else
                {
                    StatusBar = $"Publisher deletion failed, check log";
                }
            }
        }

        protected override Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            PropertyChanged -= (sender, args) =>
            {
                if (args.PropertyName == "SelectedItem" && SelectedItem != null)
                {
                    _ = RetrieveComicsForPublisher();
                }
            };
            return base.OnDeactivateAsync(close, cancellationToken);
        }
        async public Task HandleAsync(ComicChangedOnMainEvent message, CancellationToken cancellationToken)
        {
            await RetrieveComicsForPublisher();
        }
    }
}