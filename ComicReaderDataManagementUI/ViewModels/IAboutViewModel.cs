namespace ComicReaderDataManagementUI.ViewModels
{
    public interface IAboutViewModel
    {
        string ApiLocation { get; set; }
        string ApiName { get; set; }
        string ApiVersion { get; set; }
        string LibraryName { get; set; }
        string LibraryVersion { get; set; }
    }
}