namespace ComicReaderClassLibrary.Models
{
    public interface IFileModel
    {
        string name { get; set; }
        string path { get; set; }
        string totalpages { get; set; }
    }
}