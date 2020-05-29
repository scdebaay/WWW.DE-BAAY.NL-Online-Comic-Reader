using ComicReaderClassLibrary.Models;
using System.IO;
using System.IO.Abstractions;

namespace ComicReaderClassLibrary.Resources
{
    /// <summary>
    /// Class with which a specific UNC path is converted to IRootFolder class, containing IFolder and IFile objects.
    /// This object can be used to serialize into JSON.
    /// </summary>
    public interface IFolderCrawler
    {
        /// <summary>
        /// Method with which the provided folder is crawled and for which an IRootModel is generated
        /// </summary>
        /// <param name="dir">DirectoryInfo, UNC path to the folder to be crawled</param>
        /// <returns>IRootModel, representing the object to be serialized into JSON.</returns>
        IRootModel GetDirectory(IDirectoryInfo dir);
    }
}