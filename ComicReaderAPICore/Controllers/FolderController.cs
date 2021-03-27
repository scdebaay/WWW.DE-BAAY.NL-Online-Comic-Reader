using ComicReaderClassLibrary.DataAccess.Implementations;
using ComicReaderClassLibrary.Models;
using ComicReaderClassLibrary.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;

namespace ComicReaderAPICore.Controllers
{
    /// <summary>
    /// FolderController, endpoint that returns JSON formated, paged listing of files within the requested folder.
    /// </summary>
    [Route("[controller]/{folder}/{currentfolderpage:int}")]
    [Route("[controller]/{folder}")]
    [ApiController]
    public class FolderController : ControllerBase
    {
        private readonly IPathResolver _pathResolver;
        private IRootModel _rootModel;
        private readonly ISqlApiDbConnection _sqlApiDbConnection;
        private readonly ILogger _logger;
        
        /// <summary>
        /// Constructor, injects PathResolver, RootModel, SQL DBContext and logger object
        /// Injected objects are mapped to private fields.
        /// </summary>
        /// <param name="pathResolver">PathResolver object which resolves requested paths to actual filesystem paths</param>
        /// <param name="rootModel">RootFolder model that will store paged list of files in the requested folder from the database</param>
        /// <param name="sqlApiDbConnection">SQL DB context object that provides database access</param>
        /// <param name="logger">Log object that logs messages to file.</param>
        public FolderController(IPathResolver pathResolver, IRootModel rootModel, ISqlApiDbConnection sqlApiDbConnection, ILogger<FolderController> logger)
        {
            _pathResolver = pathResolver;
            _rootModel = rootModel;
            _sqlApiDbConnection = sqlApiDbConnection;
            _logger = logger;
        }

        /// <summary>
        /// Get Method for the folder class 
        /// </summary>
        /// <param name="folder">String, Requested folder type, comic, file, image etc.</param>
        /// <param name="pageLimit">Int, items to fetch per page.</param>
        /// <param name="currentfolderpage">Int, which page is begin requested. JSON also includes total pages available using current pageLimit</param>
        /// <returns>Returns JSON object containing file list, folder name, total pages, current page and total pages per comic</returns>
        [Produces("application/json")]
        #nullable enable
        public IRootModel Get(string folder, int? pageLimit, string? searchText, int? currentfolderpage = 0)
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
            _rootModel = _sqlApiDbConnection.RetrieveComics(pageLimit, currentfolderpage, searchText);
            return _rootModel;
        }
    }
}