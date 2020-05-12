using Caliburn.Micro;
using ComicReaderClassLibrary.DataAccess.DataModels;
using ComicReaderDataManagementUI.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ComicReaderDataManagementUI.ViewModels
{
    public interface IAuthorViewModel
    {
        BindableCollection<AuthorDataModel> AuthorBox { get; }
        BindableCollection<ComicDataModel> ComicBox { get; }
        BindableCollection<ComicDataModel> ComicsByAuthorBox { get; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string MiddleName { get; set; }
        public DateTime DateBirth { get; set; }
        public DateTime DateDeceased { get; set; }
        ComicDataModel SelectedComic { get; set; }
        ComicDataModel SelectedComicByAuthor { get; set; }
        AuthorDataModel SelectedItem { get; set; }
        string StatusBar { get; set; }

        void AddComicToAuthor();
        void DeleteAuthor();
        Task HandleAsync(ComicChangedOnMainEvent message, CancellationToken cancellationToken);
        void NewAuthor();
        void SaveAuthor();
    }
}