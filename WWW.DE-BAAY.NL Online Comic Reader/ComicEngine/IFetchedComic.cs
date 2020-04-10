namespace WWW.DE_BAAY.NL_Online_Comic_Reader.ComicEngine
{
    public interface IFetchedComic
    {
        string Name { get; }
        string PageName { get; }
        string PageType { get; }
        string Path { get; }
        byte[] Result { get; }
        int TotalPages { get; }
    }
}