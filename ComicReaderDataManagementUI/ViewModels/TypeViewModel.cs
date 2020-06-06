using Caliburn.Micro;
using ComicReaderClassLibrary.DataAccess.DataModels;
using ComicReaderClassLibrary.DataAccess.Implementations;
using ComicReaderDataManagementUI.Events;
using ComicReaderDataManagementUI.ViewModels.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ComicReaderDataManagementUI.ViewModels
{
    public class TypeViewModel : Screen, ITypeViewModel, IHandle<ComicChangedOnMainEvent>
    {
        #region injected objects
        private readonly IConfiguration _configuration;
        private readonly ISqlUiDbConnection _sqlUiDbConnection;
        private readonly ILogger _logger;
        private IEventAggregator _eventAggregator;
        #endregion

        #region (backed) fields
        private bool newTypeAdded { get; set; } = false;
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
        private ICommand _addComicToTypeCommand;
        public ICommand AddComicToTypeCommand
        {
            get
            {
                if (_addComicToTypeCommand == null)
                    _addComicToTypeCommand = new ParameteredComicDelegatedCommand(AddComicToType);
                return _addComicToTypeCommand;
            }
        }


        private TypeDataModel _selectedItem = new TypeDataModel();
        public TypeDataModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                if (SelectedItem != null)
                {
                    StatusBar = $"Selected type Id is {SelectedItem.Id}";
                    TypeName = SelectedItem.Term;
                }
                else
                {
                    TypeName = "Select a type";
                    StatusBar = $"";
                }
                NotifyOfPropertyChange(nameof(SelectedItem));
            }
        }
        private string _typeName = "Select a type";
        public string TypeName
        {
            get { return _typeName; }
            set
            {
                _typeName = value;
                NotifyOfPropertyChange(nameof(TypeName));
            }
        }
        public ComicDataModel SelectedComicInType { get; set; } = new ComicDataModel();

        private BindableCollection<TypeDataModel> _typeBox = new BindableCollection<TypeDataModel>();
        public BindableCollection<TypeDataModel> TypeBox
        {
            get { return _typeBox; }
            private set
            {
                _typeBox = value;
                NotifyOfPropertyChange(nameof(TypeBox));
            }
        }

        private BindableCollection<ComicDataModel> _comicsInTypeBox = new BindableCollection<ComicDataModel>();
        public BindableCollection<ComicDataModel> ComicsInTypeBox
        {
            get { return _comicsInTypeBox; }
            private set
            {
                _comicsInTypeBox = value;
                NotifyOfPropertyChange(nameof(ComicsInTypeBox));
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
        public TypeViewModel(IConfiguration configuration, ISqlUiDbConnection sqlUiDbConnection, ILogger<TypeViewModel> logger, IEventAggregator eventAggregator)
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
                    _ = RetrieveComicsForType();
                }
            };
            _ = RetrieveTypes();
        }
        #endregion

        #region private methods
        private async Task RetrieveTypes()
        {
            TypeDataModel selectedType = SelectedItem as TypeDataModel;
            TypeBox.Clear();
            var list = await _sqlUiDbConnection.RetrieveTypeAsync();
            if (selectedType != null)
            {
                foreach (TypeDataModel type in list)
                {
                    if (selectedType == type)
                    {
                        SelectedItem = selectedType;
                    }
                }
            }
            TypeBox.AddRange(list);
            SelectedItem = TypeBox[0];
            await RetrieveComicsForType();
        }

        private async Task RetrieveComicsForType()
        {
            ComicsInTypeBox.Clear();
            ComicList.Clear();
            var list = await _sqlUiDbConnection.RetrieveComicsAsync("");
            var l = list.Where(x => x.TypeId == SelectedItem.Id).Select(x => x);
            if (l != null)
            {
                SelectedComicInType = l.FirstOrDefault();
            }
            ComicsInTypeBox.AddRange(l);
            ComicList.AddRange(list);
        }

        private void AddComicToType(object selectedComicCollection)
        {
            var selectedComicIds = new List<int>();
            foreach (ComicDataModel comic in selectedComicCollection as IList)
            {
                selectedComicIds.Add(comic.Id);
            }

            var succeeded = _sqlUiDbConnection.SaveTypeAssoc(selectedComicIds, SelectedItem.Id);
            if (succeeded == true)
            {
                IList items = (IList)selectedComicCollection;
                var comicsAdded = items.Cast<ComicDataModel>().ToList();
                ComicsInTypeBox.AddRange(comicsAdded.Except(ComicsInTypeBox.ToList())) ;
                _eventAggregator.PublishOnUIThreadAsync(new ComicChangedOnDialogEvent());
                StatusBar = $"Comic(s) added to type";
            }
            else
            {
                IList items = (IList)selectedComicCollection;
                var comicsNotAdded = items.Cast<ComicDataModel>();
                ComicsInTypeBox.RemoveRange(comicsNotAdded);
                StatusBar = $"Add comic(s) failed, check log";
            }
        }
        #endregion

        #region public methods        
        public void NewType()
        {
            var lastId = TypeBox.OrderByDescending(i => i.Id).First().Id + 1;
            SelectedItem = new TypeDataModel
            {
                Id = lastId,
                Term = "New type"
            };
            TypeBox.Add(SelectedItem);
            newTypeAdded = true;
        }

        public void SaveType()
        {
            int typeToUpdatePos = TypeBox.IndexOf(SelectedItem);
            if (newTypeAdded == false)
            {
                var typeUpdated = _sqlUiDbConnection.UpdateType(SelectedItem.Id, TypeName);
                TypeDataModel typeToUpdate = new TypeDataModel
                {
                    Id = SelectedItem.Id,
                    Term = TypeName
                };
                if (typeUpdated == true)
                {
                    TypeBox.RemoveAt(typeToUpdatePos);
                    TypeBox.Insert(typeToUpdatePos, typeToUpdate);
                    SelectedItem = TypeBox[typeToUpdatePos];
                    _eventAggregator.PublishOnUIThreadAsync(new ComicChangedOnDialogEvent());
                    StatusBar = $"Type updated";
                }
                else
                {
                    StatusBar = $"Type update failed, check log";
                }
            }
            else
            {
                var newTypeId = _sqlUiDbConnection.InsertType(TypeName);
                SelectedItem.Id = newTypeId;
                if (SelectedItem.Id == 0)
                {
                    StatusBar = $"Type not saved to the database, check log";
                }
                else if (SelectedItem.Id == 1)
                {
                    StatusBar = $"Type existed in the database";
                }
                newTypeAdded = false;
            }
            _ = RetrieveTypes();
        }

        public void DeleteType()
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                int typeToDeletePos = TypeBox.IndexOf(SelectedItem);
                var typeDeleted = _sqlUiDbConnection.DeleteType(SelectedItem.Id);
                if (typeDeleted == true)
                {
                    TypeBox.RemoveAt(typeToDeletePos);
                    SelectedItem = TypeBox[0];
                    _eventAggregator.PublishOnUIThreadAsync(new ComicChangedOnDialogEvent());
                    StatusBar = $"Type deleted";
                }
                else
                {
                    StatusBar = $"Type deletion failed, check log";
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
                    _ = RetrieveComicsForType();
                }
            };
            return base.OnDeactivateAsync(close, cancellationToken);
        }
        async public Task HandleAsync(ComicChangedOnMainEvent message, CancellationToken cancellationToken)
        {
            await RetrieveComicsForType();
        }
        #endregion
    }
}
