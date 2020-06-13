using Caliburn.Micro;
using ComicReaderClassLibrary.DataAccess.DataModels;
using ComicReaderDataManagementUI.Events;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ComicReaderDataManagementUI.ViewModels
{
    public interface IPublisherViewModel
    {
        /// <summary>
        /// Backing field for the Comic dropdown
        /// </summary>
        BindableCollection<ComicDataModel> ComicList { get; }
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
        /// Command to add Comics to Publisher
        /// </summary>
        ICommand AddComicToPublisherCommand { get; }
        /// <summary>
        /// Command for the Add New Publisher button
        /// </summary>
        ICommand NewPublisherCommand { get; }
        /// <summary>
        /// Command for the Save Publisher button
        /// </summary>
        ICommand SavePublisherCommand { get; }
        /// <summary>
        /// Command for the Delete puiblisher button
        /// </summary>
        ICommand DeletePublisherCommand { get; }
        /// <summary>
        /// Event handler for when a Comic was changed in the MainViewModel
        /// </summary>
        Task HandleAsync(ComicChangedOnMainEvent message, CancellationToken cancellationToken);
    }
}