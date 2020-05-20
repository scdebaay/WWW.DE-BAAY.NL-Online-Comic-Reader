namespace ComicReaderClassLibrary.Models
{
    /// <summary>
    /// Interface IRootModel, contains FolderModel as a child item.
    /// </summary>
    public interface IRootModel
    {
        FolderModel folder { get; set; }
    }
}