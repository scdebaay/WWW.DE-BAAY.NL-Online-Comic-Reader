namespace ComicReaderClassLibrary.Models
{
    /// <summary>
    /// RootModel implementing IRootModel, contains FolderModel as a child item.
    /// </summary>
    public class RootModel : IRootModel
    {
        public FolderModel folder { get; set; }
    }
}
