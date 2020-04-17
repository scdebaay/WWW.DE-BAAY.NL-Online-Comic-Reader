using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IO;

namespace ComicReaderClassLibrary.Resources
{
    public class PathResolver : IPathResolver
    {
        //Start logger
        readonly ILogger<PathResolver> _logger;
        readonly IConfiguration _config;
        readonly IWebHostEnvironment _env;
        readonly IContentType _contentType;

        public PathResolver(IConfiguration config, ILogger<PathResolver> logger, IWebHostEnvironment env, IContentType contentType)
        {
            _config = config;
            _logger = logger;
            _env = env;
            _contentType = contentType;
        }

        /// <summary>
        /// Singleton accessor
        /// </summary>
        /// <param name="name">Will try to find the requested property in the IConfiguration _config object</param>
        /// <returns>Emtpy string if setting key of type <name> is not found</name></returns>
        public dynamic this[string name]
        {
            get
            {
                try
                {
                    if (int.TryParse(_config.GetValue<string>(name), out int result))
                    {
                        return result;
                    }
                    else
                    {
                        return _config.GetValue<string>(name);
                    }

                }
                catch
                {
                    _logger.LogError($"Unable to retrieve setting '{name}'");
                    return string.Empty;
                }

            }
        }

        /// <summary>
        /// Based on the requested file name the folder to browse is set. For now this can be either Comic or Image
        /// </summary>
        /// <param name="requestedFileName">Filename that was requested in the request being processed.</param>
        /// <returns></returns>
        public string ServerPath(string requestedFileName)
        {

            //ContentType requestedFile;
            if (requestedFileName != null)
            {
                //requestedFile = new ContentType();
                _contentType.SetType(requestedFileName);
            }
            else
            {
                //requestedFile = new ContentType();
                _contentType.SetType(DefaultFile);
            }
            DefaultFileType = _contentType.Extension;
            //Based on the media type requested currentFolder is set to either comicFolder or imageFolder.
            currentFolder = FindFolder(_contentType.FolderType);
            //If the path is not relative to the Api App folder, serverPath property is set.
            if (Path.IsPathRooted(currentFolder))
            {
                return string.Empty;
            }
            else
            {
                return $"{_env.ContentRootPath}\\";
            }
        }

        /// <summary>
        /// Based on the requested file extension the folder to browse is set. For now this can be either Comic or Image
        /// </summary>
        /// <param name="requestedFileFolder">Folder type is passed in and the path is returned.</param>
        /// <returns></returns>
        private string FindFolder(string requestedFileFolder)
        {
            return this[requestedFileFolder];
        }

        /// <summary>
        /// Check whether the requested file/path is valid.
        /// </summary>
        /// <param name="path">The file or path to be checked</param>
        /// <param name="allowRelativePaths">Whether relative paths are allowed.</param>
        /// <returns></returns>
        public bool IsValidPath(string path, bool allowRelativePaths = true)
        {
            {
                bool isValid;

                try
                {
                    string fullPath = Path.GetFullPath(path);

                    if (allowRelativePaths)
                    {
                        isValid = Path.IsPathRooted(path) && File.Exists(path);
                    }
                    else
                    {
                        string root = Path.GetPathRoot(path);
                        isValid = (string.IsNullOrEmpty(root.Trim(new char[] { '\\', '/' })) == false) && File.Exists(path);
                    }
                }
                catch
                {
                    _logger.LogError($"File not found: {path}");
                    isValid = false;
                }

                return isValid;
            }
        }

        public string MediaFolder { get { return this["mediaFolder"]; } }
        public string ImageFolder { get { return this["imageFolder"]; } }
        public string MusicFolder { get { return this["musicFolder"]; } }
        public string FileFolder { get { return this["fileFolder"]; } }
        public string ComicFolder { get { return this["comicFolder"]; } }
        public string EpubFolder { get { return this["epubFolder"]; } }
        public string DefaultFile { get { return this["defaultFile"]; } }
        public int PageLimit { get { return this["pageLimit"]; } }
        private string currentFolder;
        public string DefaultFileType { private set; get; }
    }
}