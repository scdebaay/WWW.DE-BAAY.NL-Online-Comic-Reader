using ComicReaderWebCore.Models;
using System.Threading.Tasks;

namespace ComicReaderWebCore.Data
{
    /// <summary>
    /// Interface that defines the Comic API Call Service for the website
    /// </summary>
    interface IComicApiCallService
    {
        /// <summary>
        /// Fields retrieved from the Configuration, used in the website to call the API or needed parameters
        /// </summary>
        string ApiLocation { get; set; }
        int DefaultComicSize { get; set; }
        int PageLimit { get; set; }
        string SiteRoot { get; set; }

        /// <summary>
        /// Method to retrieve Comic info from the database. The call is used to resolve available comic pages.
        /// </summary>
        /// <param name="comic">String, representing the comic for which comicinfo needs to be retrieved</param>
        /// <returns>ComicListModel containing one comic with info</returns>
        Task<ComicListModel> GetComicInfo(string comic);
        /// <summary>
        /// Method to retrieve a ComicListModel containing all comics to be displayed on the ComicList page.
        /// </summary>
        /// <param name="page">Int, Optional, representing the page number to be retrieved from the API.</param>
        /// <returns></returns>
        Task<ComicListModel> GetFolderListAsync(int? page = 1);
    }
}