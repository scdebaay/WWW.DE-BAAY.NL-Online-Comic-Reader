using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.IO;
using System.Diagnostics;
using ComicReaderAPICore.ComicEngine;
using ComicReaderAPICore.Models;

namespace ComicReaderAPICore.Resources
{
    public class FolderCrawler
    {
        // Start logger
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
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
        public static IRootModel GetDirectory(DirectoryInfo dir, DirectoryInfo mediafolder, int? pageLimit, int? page)
        {
            // Data structure to hold names of subfolders to be
            // examined for files.
            Stack<DirectoryInfo> dirStack = new Stack<DirectoryInfo>(20);
            List<FileInfo> fileInfos = new List<FileInfo>();

            if (!System.IO.Directory.Exists(dir.FullName))
            {
                throw new ArgumentException();
            }
            dirStack.Push(dir);

            while (dirStack.Count > 0)
            {
                DirectoryInfo currentDir = dirStack.Pop();
                DirectoryInfo[] subDirs;
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
                    Logger.Error($"Access refused to : {currentDir.Name}");
                    continue;
                }
                catch (DirectoryNotFoundException)
                {
                    Logger.Error($"The folder was not found : {currentDir.Name}");
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
                    List<FileInfo> fileList = new List<FileInfo>();
                    foreach (var file in currentDir.GetFiles("*"))
                    {
                        if (!file.Attributes.HasFlag(FileAttributes.Hidden) | !file.Attributes.HasFlag(FileAttributes.System))
                        {
                            fileList.Add(file);
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
                    Logger.Error($"File access refused to : {currentDir.Name}");
                    continue;
                }
                catch (DirectoryNotFoundException)
                {
                    Logger.Error($"The folder was not found while processing files: {currentDir.Name}");
                    continue;
                }
                // Push the subdirectories onto the stack for traversal.
                // This could also be done before handing the files.
                foreach (DirectoryInfo dirInfo in subDirs)
                {
                    dirStack.Push(dirInfo);
                }
            }
            fileInfos.Reverse();
            FileInfo[] files = fileInfos.Skip(((int)page - 1) * (int)pageLimit).Take((int)pageLimit).ToArray();
            var info = new RootModel();
            info.folder = new FolderModel()
            {
                name = dir.Name,
                files = fileInfos.Count().ToString(),
                totalPages = ((fileInfos.Count() + (int)pageLimit - 1) / (int)pageLimit).ToString(),
                currentPage = ((int)page).ToString(),
                file = new List<Models.FileModel>()
            };

            foreach (var file in files)
            {
                int totalpages = Comic.LoadFromFile(file).Pages.Count;

                try
                {
                    info.folder.file.Add(new Models.FileModel()
                    {
                        name = file.Name,
                        path = GetRelativePath(file, file.Directory, mediafolder),
                        totalpages = totalpages.ToString()
                    });
                }
                catch
                {
                    Logger.Error($"Unable to find page count for: {file.Name}");
                    continue;
                }
            }
            return info;
        }

        private static string GetRelativePath(FileInfo file, DirectoryInfo dir, DirectoryInfo mediafolder)
        {
            var folder = dir.Name;

            if (mediafolder.Name != dir.Name)
            {
                do
                {
                    folder = $"{dir.Parent.Name}\\{folder}";
                    dir = dir.Parent;
                }
                while (mediafolder.Name != dir.Name);
            }
            folder = folder.Substring(mediafolder.Name.Length);
            folder = $"{folder}\\{file.Name}";
            return folder;
        }
    }
}
