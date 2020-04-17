using System.Collections.Generic;

namespace ComicReaderClassLibrary.Models
{
    public interface IFolderModel
    {
        string currentPage { get; set; }
        List<FileModel> file { get; set; }
        string files { get; set; }
        string name { get; set; }
        string totalPages { get; set; }
    }
}