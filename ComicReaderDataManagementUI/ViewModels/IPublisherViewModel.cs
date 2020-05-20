using Caliburn.Micro;
using ComicReaderClassLibrary.DataAccess.DataModels;
using ComicReaderDataManagementUI.Events;
using System.Threading;
using System.Threading.Tasks;

namespace ComicReaderDataManagementUI.ViewModels
{
    public interface IPublisherViewModel
    {
        /// <summary>
        /// Backing field for the Comic dropdown
        /// </summary>
        BindableCollection<ComicDataModel> ComicBox { get; }
        /// <summary>
        /// Backing field for the Comics by Publisher listbox
        /// </summary>
        BindableCollection<ComicDataModel> ComicsByPublisherBox { get; }
        /// <summary>
        /// Backing field for the Publisher dropdown
        /// </summary>
        BindableCollection<PublisherDataModel> PublisherBox { get; }
        /// <summary>
        /// Backing field for the Publisher name textbox
        /// </summary>
        string PublisherName { get; set; }
        /// <summary>
        /// Backing field for the Selected Comic in the Comic dropdown
        /// </summary>
        ComicDataModel SelectedComic { get; set; }
        /// <summary>
        /// Backing field for the Selected Comic in the Comic by Publisher Listbox
        /// </summary>
        ComicDataModel SelectedComicByPublisher { get; set; }
        /// <summary>
        /// Backing field for the Selected Publisher in the Publisher dropdown
        /// </summary>
        PublisherDataModel SelectedItem { get; set; }
        /// <summary>
        /// Backing field for the Statusbar label
        /// </summary>
        string StatusBar { get; set; }

        /// <summary>
        /// Method for the Add Comic to Publisher button
        /// </summary>
        void AddComicToPublisher();
        /// <summary>
        /// Method for the Delete puiblisher button
        /// </summary>
        void DeletePublisher();
        /// <summary>
        /// Event handler for when a Comic was changed in the MainViewModel
        /// </summary>
        Task HandleAsync(ComicChangedOnMainEvent message, CancellationToken cancellationToken);
        /// <summary>
        /// Method for the Add New Publisher button
        /// </summary>
        void NewPublisher();
        /// <summary>
        /// Method for the Save Publisher button
        /// </summary>
        void SavePublisher();
    }
}