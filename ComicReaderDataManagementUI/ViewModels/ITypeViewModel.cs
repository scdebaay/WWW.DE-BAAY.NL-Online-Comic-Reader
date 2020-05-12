using Caliburn.Micro;
using ComicReaderClassLibrary.DataAccess.DataModels;
using ComicReaderDataManagementUI.Events;
using System.Threading;
using System.Threading.Tasks;

namespace ComicReaderDataManagementUI.ViewModels
{
    public interface ITypeViewModel
    {
        BindableCollection<ComicDataModel> ComicBox { get; }
        BindableCollection<ComicDataModel> ComicsInTypeBox { get; }
        ComicDataModel SelectedComic { get; set; }
        ComicDataModel SelectedComicInType { get; set; }
        TypeDataModel SelectedItem { get; set; }
        string StatusBar { get; set; }
        BindableCollection<TypeDataModel> TypeBox { get; }
        string TypeName { get; set; }

        void AddComicToType();
        void DeleteType();
        Task HandleAsync(ComicChangedOnMainEvent message, CancellationToken cancellationToken);
        void NewType();
        void SaveType();
    }
}