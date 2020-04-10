namespace ComicReaderAPICore.Models
{
    public interface IFetchedComic
    {
        string Name { get; }
        string PageName { get; }
        string PageType { get; }
        string Path { get; }
        byte[] Result { get; }
        int TotalPages { get; }

        void LoadComic(string comicFile, int? size, int page);
    }
}