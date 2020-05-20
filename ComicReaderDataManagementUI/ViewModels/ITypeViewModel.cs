using Caliburn.Micro;
using ComicReaderClassLibrary.DataAccess.DataModels;
using ComicReaderDataManagementUI.Events;
using System.Threading;
using System.Threading.Tasks;

namespace ComicReaderDataManagementUI.ViewModels
{
    public interface ITypeViewModel
    {
        /// <summary>
        /// Backing field for the Comic dropdown
        /// </summary>
        BindableCollection<ComicDataModel> ComicBox { get; }
        /// <summary>
        /// Backing field for the Comics in Type listbox
        /// </summary>
        BindableCollection<ComicDataModel> ComicsInTypeBox { get; }
        /// <summary>
        /// Backing field for the Selected Comic in the Comic dropdown
        /// </summary>
        ComicDataModel SelectedComic { get; set; }
        /// <summary>
        /// Backing field for the Selected Comic in Comic in Type listbox
        /// </summary>
        ComicDataModel SelectedComicInType { get; set; }
        /// <summary>
        /// Backing field for the Selected Type in the type dropdown
        /// </summary>
        TypeDataModel SelectedItem { get; set; }
        /// <summary>
        /// Backing field for the Statusbar label
        /// </summary>
        string StatusBar { get; set; }
        /// <summary>
        /// Backing field for the Type dropdown
        /// </summary>
        BindableCollection<TypeDataModel> TypeBox { get; }
        /// <summary>
        /// Backing field for the Type name
        /// </summary>
        string TypeName { get; set; }

        /// <summary>
        /// Method for the Add Comic to Type button
        /// </summary>
        void AddComicToType();
        /// <summary>
        /// Method for the Delete Type button
        /// </summary>
        void DeleteType();
        /// <summary>
        /// Method for the eventhandler when a Comic was changed in the MainViewModel
        /// </summary>
        Task HandleAsync(ComicChangedOnMainEvent message, CancellationToken cancellationToken);
        /// <summary>
        /// Method for the new Type button
        /// </summary>
        void NewType();
        /// <summary>
        /// Method for the Save Type button
        /// </summary>
        void SaveType();
    }
}