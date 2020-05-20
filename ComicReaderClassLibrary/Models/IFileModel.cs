namespace ComicReaderClassLibrary.Models
{
    /// <summary>
    /// Interface IFileModel, contains file properties for files retrieved from the API.
    /// </summary>
    public interface IFileModel
    {
        string name { get; set; }
        string path { get; set; }
        string totalpages { get; set; }
    }
}