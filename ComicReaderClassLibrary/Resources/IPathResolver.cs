namespace ComicReaderClassLibrary.Resources
{
    public interface IPathResolver
    {
        dynamic this[string name] { get; }

        string ComicFolder { get; }
        string DefaultFile { get; }
        string DefaultFileType { get; }
        string EpubFolder { get; }
        string FileFolder { get; }
        string ImageFolder { get; }
        string MediaFolder { get; }
        string MusicFolder { get; }
        int PageLimit { get; }

        string ServerPath(string requestedFileName);
        public bool IsValidPath(string path, bool allowRelativePaths = true);
    }
}