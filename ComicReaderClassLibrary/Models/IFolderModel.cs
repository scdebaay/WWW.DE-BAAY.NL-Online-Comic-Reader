using System.Collections.Generic;

namespace ComicReaderClassLibrary.Models
{
    /// <summary>
    /// Interface IFolderModel, contains  a List of FileModels as a child item.
    /// </summary>
    public interface IFolderModel
    {
        string currentPage { get; set; }
        List<FileModel> file { get; set; }
        string files { get; set; }
        string name { get; set; }
        string totalPages { get; set; }
    }
}