using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ComicReaderAPICore.Models;
using ComicReaderAPICore.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComicReaderAPICore.Controllers
{
    [Route("[controller]/{folder}/{currentfolderpage:int}")]
    [Route("[controller]/{folder}")]
    [ApiController]
    public class FolderController : ControllerBase
    {
        private readonly IPathResolver _pathResolver;
        private readonly IRootModel _rootModel;
        public FolderController(IPathResolver pathResolver, IRootModel rootModel)
        {
            _pathResolver = pathResolver;
            _rootModel = rootModel;
        }


        public IRootModel Get(string folder, int? pageLimit, int? currentfolderpage = 0)
        {

            if (pageLimit == null)
            {
                pageLimit = _pathResolver.PageLimit;
            }
            string requestedDir = string.Empty;
            switch (folder)
            {
                case "comic":
                    requestedDir = _pathResolver.ComicFolder;
                    break;
                case "file":
                    requestedDir = _pathResolver.FileFolder;
                    break;
                case "media":
                    requestedDir = _pathResolver.MediaFolder;
                    break;
                case "image":
                    requestedDir = _pathResolver.ImageFolder;
                    break;
                case "music":
                    requestedDir = _pathResolver.MusicFolder;
                    break;
                case "epub":
                    requestedDir = _pathResolver.EpubFolder;
                    break;
            }
            DirectoryInfo dir = new DirectoryInfo(requestedDir);
            IRootModel rootfolder = FolderCrawler.GetDirectory(dir, dir, pageLimit, currentfolderpage);
            return rootfolder;
        }
    }
}