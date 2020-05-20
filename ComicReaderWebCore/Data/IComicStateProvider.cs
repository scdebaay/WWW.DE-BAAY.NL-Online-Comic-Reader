using ComicReaderWebCore.Models;
using System.Collections.Generic;

namespace ComicReaderWebCore.Data
{
    /// <summary>
    /// Interface that defines the Comic State provider
    /// </summary>
    public interface IComicStateProvider
    {
        /// <summary>
        /// Dictionary of string, int, Dictionary containing title of comics and current page for each comic that have been visited on the website
        /// </summary>
        Dictionary<string, int> ComicsVisited { get; set; }
        /// <summary>
        /// ComicModel, Current comic being viewed, provides current page property
        /// </summary>
        ComicModel CurrentComic { get; set; }
        /// <summary>
        /// Int, Current page size, defaults to size set in Configuration or Zoom factor set in ComicPage razor page.
        /// </summary>
        int PageSize { get; set; }
    }
}