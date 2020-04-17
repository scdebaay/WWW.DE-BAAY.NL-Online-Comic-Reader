using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ComicReaderClassLibrary.Models;
using System.IO;

namespace ComicReaderAPICore.Controllers
{
    
    

    [ApiController]
    public class ComicController : ControllerBase
    {
        public ComicController(IFetchedComic fetchedComic)
        {
            _fetchedComic = fetchedComic;
        }
        private string Path { get; set; }
        private string Filename { get; set; }
        private int CurrentComicPage { get; set; }
        private int? Size { get; set; }
        private readonly IFetchedComic _fetchedComic;
        
        [Route("[controller]/{path}/{filename}/{currentcomicpage:int}")]
        [Route("[controller]/{path}/{filename}")]
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

        [Route("[controller]/{path}/{filename}/comicinfo")]
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