using ComicReaderClassLibrary.DataAccess.Implementations;
using ComicReaderClassLibrary.Models;
using ComicReaderClassLibrary.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;

namespace ComicReaderAPICore.Controllers
{
    [Route("[controller]/{folder}/{currentfolderpage:int}")]
    [Route("[controller]/{folder}")]
    [ApiController]
    public class FolderController : ControllerBase
    {
        private readonly IPathResolver _pathResolver;
        private IRootModel _rootModel;
        private readonly ISqlApiDbConnection _sqlApiDbConnection;
        private readonly ILogger _logger;
        public FolderController(IPathResolver pathResolver, IRootModel rootModel, ISqlApiDbConnection sqlApiDbConnection, ILogger<FolderController> logger)
        {
            _pathResolver = pathResolver;
            _rootModel = rootModel;
            _sqlApiDbConnection = sqlApiDbConnection;
            _logger = logger;
        }


        [Produces("application/json")]
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
            _rootModel = _sqlApiDbConnection.RetrieveComics(pageLimit, currentfolderpage);
            return _rootModel;
        }
    }
}