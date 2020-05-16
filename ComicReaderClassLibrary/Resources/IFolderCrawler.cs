using ComicReaderClassLibrary.Models;
using System.IO;

namespace ComicReaderClassLibrary.Resources
{
    public interface IFolderCrawler
    {
        IRootModel GetDirectory(DirectoryInfo dir);
    }
}