namespace ComicReaderClassLibrary.Models
{
    /// <summary>
    /// FileModel, implementing IFileModel, containing file properties
    /// The FileModel implements properties, name, path, totalPages
    /// </summary>
    public class FileModel : IFileModel
    {
        public string name { get; set; }
        public string path { get; set; }
        public string totalpages { get; set; }
    }
}
