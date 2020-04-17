namespace ComicReaderClassLibrary.Models
{
    public class FileModel : IFileModel
    {
        public string name { get; set; }
        public string path { get; set; }
        public string totalpages { get; set; }
    }
}
