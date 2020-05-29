using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ComicReaderClassLibrary.ComicEngine;
using ComicReaderClassLibrary.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ComicReaderClassLibrary.Resources
{
    public class FolderCrawler : IFolderCrawler
    {
        private readonly ILogger<FolderCrawler> _logger;
        private readonly IComic _comic;
        private readonly IConfiguration _configuration;
        private readonly IRootModel _rootModel;

        public FolderCrawler(IConfiguration configuration, IRootModel rootModel, ILogger<FolderCrawler> logger, IComic comic)
        {
            _configuration = configuration;
            _rootModel = rootModel;
            _logger = logger;
            _comic = comic;
        }

        /// <summary>
        /// Iterate through mediaFolder subfolders and convert directory listing into XML format.
        /// Result contains 
        /// -Folder name. 
        /// -Total number of files available. 
        /// -Total number of pages available based on the number of file per page (pageLimit).
        /// -CurrentPage.
        /// </summary>
        /// <param name="dir">Folder to be processed</param>
        /// <param name="mediafolder">Mediafolder to be set as root for iteration. Will not iterate outside this folder.</param>
        /// <param name="pageLimit">Set the number of files per page being returned</param>
        /// <param name="page">The page we are looking at.</param>
        /// <returns></returns>
        public IRootModel GetDirectory(IDirectoryInfo dir)
        {
            // Data structure to hold names of subfolders to be
            // examined for files.
            Stack<IDirectoryInfo> dirStack = new Stack<IDirectoryInfo>(20);
            List<IFileInfo> fileInfos = new List<IFileInfo>();

            if (!System.IO.Directory.Exists(dir.FullName))
            {
                throw new ArgumentException();
            }
            dirStack.Push(dir);

            while (dirStack.Count > 0)
            {
                IDirectoryInfo currentDir = dirStack.Pop();
                IDirectoryInfo[] subDirs;
                try
                {
                    subDirs = currentDir.GetDirectories();
                }
                // An UnauthorizedAccessException exception will be thrown if we do not have
                // discovery permission on a folder or file. It may or may not be acceptable 
                // to ignore the exception and continue enumerating the remaining files and 
                // folders. It is also possible (but unlikely) that a DirectoryNotFound exception 
                // will be raised. This will happen if currentDir has been deleted by
                // another application or thread after our call to Directory.Exists. The 
                // choice of which exceptions to catch depends entirely on the specific task 
                // you are intending to perform and also on how much you know with certainty 
                // about the systems on which this code will run.
                catch (UnauthorizedAccessException)
                {
                    _logger.LogError($"Access refused to : {currentDir.Name}");
                    continue;
                }
                catch (DirectoryNotFoundException)
                {
                    _logger.LogError($"The folder was not found : {currentDir.Name}");
                    continue;
                }
                //Uncomment for troubleshooting
                //catch (Exception e)
                //{
                //    Exception Error = new Exception("Error",e);
                //    throw Error;
                //}

                try
                {
                    List<IFileInfo> fileList = new List<IFileInfo>();
                    foreach (var file in currentDir.GetFiles("*"))
                    {
                        if (!file.Attributes.HasFlag(FileAttributes.Hidden) | !file.Attributes.HasFlag(FileAttributes.System))
                        {
                            if ((file.Extension == ".cbz") | (file.Extension == ".cbr"))
                            {
                                fileList.Add(file);
                            }
                        }
                    }
                    fileList.Reverse();
                    foreach (var comic in fileList)
                    {
                        fileInfos.Add(comic);
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    _logger.LogError($"File access refused to : {currentDir.Name}");
                    continue;
                }
                catch (DirectoryNotFoundException)
                {
                    _logger.LogError($"The folder was not found while processing files: {currentDir.Name}");
                    continue;
                }
                // Push the subdirectories onto the stack for traversal.
                // This could also be done before handing the files.
                foreach (IDirectoryInfo dirInfo in subDirs)
                {
                    dirStack.Push(dirInfo);
                }
            }
            fileInfos.Reverse();
            IFileInfo[] files = fileInfos.ToArray();
            var info = new RootModel();
            info.folder = new FolderModel()
            {
                name = dir.Name,
                files = fileInfos.Count().ToString(),
                totalPages = "1",
                currentPage = "1",
                file = new List<Models.FileModel>()
            };

            foreach (var file in files)
            {
                _comic.LoadFromFile(file);
                int totalpages = _comic.Pages.Count;

                try
                {
                    info.folder.file.Add(new Models.FileModel()
                    {
                        name = file.Name,
                        path = GetRelativePath(file, file.Directory, dir),
                        totalpages = totalpages.ToString()
                    });
                }
                catch
                {
                    _logger.LogError($"Unable to find page count for: {file.Name}");
                    continue;
                }
            }
            return info;
        }

        private string GetRelativePath(IFileInfo file, IDirectoryInfo dir, IDirectoryInfo root)
        {
            var folder = dir.Name;

            if (root.Name != dir.Name)
            {
                do
                {
                    folder = $"{dir.Parent.Name}\\{folder}";
                    dir = dir.Parent;
                }
                while (root.Name != dir.Name);
            }
            folder = folder.Substring(root.Name.Length);
            folder = $"{folder}\\{file.Name}";
            return folder;
        }
    }
}
