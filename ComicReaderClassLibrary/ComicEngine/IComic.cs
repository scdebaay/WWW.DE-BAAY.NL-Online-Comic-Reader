using ComicReaderClassLibrary.Resources;
using System.IO.Abstractions;

namespace ComicReaderClassLibrary.ComicEngine
{
    public interface IComic
    {
        ComicInfo ComicInfo { get; }
        int CurrentIndex { get; set; }
        Page CurrentPage { get; set; }
        SortableObservableCollection<Page> DeletedPages { get; }
        SortableObservableCollection<Page> Pages { get; }
        string Title { get; }
        int ViewingIndex { get; }

        void Dispose();
        Comic LoadFromFile(IFileInfo file);
    }
}