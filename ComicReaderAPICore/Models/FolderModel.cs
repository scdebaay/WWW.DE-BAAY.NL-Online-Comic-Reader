using System.Collections.Generic;

namespace ComicReaderAPICore.Models
{
    public class FolderModel : IFolderModel
    {
        readonly IFileModel _file;

        public string name { get; set; }
        public string files { get; set; }
        public string totalPages { get; set; }
        public string currentPage { get; set; }
        public List<FileModel> file { get; set; }
    }
}
