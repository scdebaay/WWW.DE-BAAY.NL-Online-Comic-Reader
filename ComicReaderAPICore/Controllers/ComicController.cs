using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ComicReaderClassLibrary.Models;
using System.IO;
using Microsoft.Extensions.Logging;

namespace ComicReaderAPICore.Controllers
{
    /// <summary>
    /// ComicController, endpoint that returns comic pages in image format
    /// </summary>
    [ApiController]
    public class ComicController : ControllerBase
    {
        private readonly ILogger _logger;
        /// <summary>
        /// Constructor, injects FetchedComic model and Logging object
        /// </summary>
        /// <param name="fetchedComic">FetchedComic model representing the requested comic, the comic is used to fetch the comic pages.</param>
        /// <param name="logger">Log object that logs messages to file.</param>
        public ComicController(IFetchedComic fetchedComic, ILogger<ComicController> logger)
        {
            _fetchedComic = fetchedComic;
            _logger = logger;
        }
        private string Path { get; set; }
        private string Filename { get; set; }
        private int CurrentComicPage { get; set; }
        private int? Size { get; set; }
        private readonly IFetchedComic _fetchedComic;
        
        /// <summary>
        /// Get method for comic pages
        /// </summary>
        /// <param name="path">String, subfolder of comic requested</param>
        /// <param name="filename">String, comic file requested</param>
        /// <param name="size">Int, size in pixels of returned page</param>
        /// <param name="currentcomicpage">Int, current comic page number</param>
        /// <returns>Image, png, jpg or gif, current page for display in filestream format</returns>
        [Route("[controller]/{path}/{filename}/{currentcomicpage:int}")]
        [Route("[controller]/{path}/{filename}")]
        [Produces("image/png", "image/jpg", "image/gif")]
        [HttpGet]
        public IActionResult Get(string path, string filename, int? size, int currentcomicpage = 0)
        {
            Path = path;
            Filename = filename;
            CurrentComicPage = currentcomicpage;
            Size = size;


            string comicFile = $"{Path}\\{Filename}";
            _fetchedComic.LoadComic(comicFile, Size, CurrentComicPage);

            var stream = _fetchedComic.Result;
            return File(stream, _fetchedComic.PageType);
        }

        /// <summary>
        /// Get method for comic inforation
        /// </summary>
        /// <param name="path">String, path to the comic from which to display information</param>
        /// <param name="filename">String, filename of the comic for which to display information</param>
        /// <returns>JSON object with comic information, total pages per comic needed for comic display</returns>
        [Route("[controller]/{path}/{filename}/comicinfo")]
        [Produces("application/json")]
        [HttpGet]
        public IRootModel Get(string path, string filename)
        {
            Path = path;
            Filename = filename;
            CurrentComicPage = 0;
            Size = 10;

            string comicFile = $"{Path}\\{Filename}";
            _fetchedComic.LoadComic(comicFile, Size, CurrentComicPage);

            var info = new RootModel();
            info.folder = new FolderModel()
            {
                name = "pub",
                files = "1",
                totalPages = "1",
                currentPage = "1",
                file = new List<FileModel>()
            };
            info.folder.file.Add(new FileModel
            {
                name = _fetchedComic.Name,
                path = $"\\{_fetchedComic.Path}",
                totalpages = _fetchedComic.TotalPages.ToString()
            }); ; ;

            return info;
        }
    }
}