namespace ComicReaderClassLibrary.Resources
{
    public interface IAboutDataModel
    {
        string ApiLocation { get; set; }
        string ApiName { get; set; }
        string ApiVersion { get; set; }
        string LibraryName { get; set; }
        string LibraryVersion { get; set; }
    }
}