using Caliburn.Micro;
using ComicReaderClassLibrary.DataAccess.DataModels;

namespace ComicReaderDataManagementUI.ViewModels
{
    public interface ILanguageViewModel
    {
        public string StatusBar { get; }
        BindableCollection<ComicDataModel> ComicBox { get; }
        BindableCollection<ComicDataModel> ComicsInLanguageBox { get; }
        BindableCollection<LanguageDataModel> LanguageBox { get; }
        string LanguageName { get; set; }
        ComicDataModel SelectedComic { get; set; }
        ComicDataModel SelectedComicInLanguage { get; set; }
        LanguageDataModel SelectedItem { get; set; }
        public void AddComicToLanguage();
    }
}