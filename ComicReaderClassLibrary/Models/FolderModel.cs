using System.Collections.Generic;

namespace ComicReaderClassLibrary.Models
{
    /// <summary>
    /// FolderModel, implementing IFolderModel, containing a list of FileModels, representing Child-Items
    /// The FolderModel implements properties, name, files, totalPages, currentPage and file
    /// </summary>
    public class FolderModel : IFolderModel
    {
        //readonly IFileModel _file;

        public string name { get; set; }
        public string files { get; set; }
        public string totalPages { get; set; }
        public string currentPage { get; set; }
        public List<FileModel> file { get; set; }
    }
}
