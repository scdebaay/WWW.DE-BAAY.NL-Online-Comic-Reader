﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.IO;
using WWW.DE_BAAY.NL_Online_Comic_Reader.ComicEngine;
using System.Diagnostics;

namespace Resources
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
        public static XElement GetDirectoryXml(DirectoryInfo dir, DirectoryInfo mediafolder, int? pageLimit, int? page)
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
                    //Debug.WriteLine($"Access refused to : {currentDir.Name}");
                    Logger.Error($"Access refused to : {currentDir.Name}");
                    continue;
                }
                catch (DirectoryNotFoundException)
                {

                    //Debug.WriteLine($"The folder was not found : {currentDir.Name}");
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
                    //Debug.WriteLine($"File access refused to : {currentDir.Name}");
                    Logger.Error($"File access refused to : {currentDir.Name}");
                    continue;
                }
                catch (DirectoryNotFoundException)
                {
                    //Debug.WriteLine($"The folder was not found while processing files: {currentDir.Name}");
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
            var info = new XElement("folder",
                           new XAttribute("name", dir.Name),
                           new XAttribute("files", fileInfos.Count().ToString()),
                           new XAttribute("totalPages", ((fileInfos.Count() + pageLimit - 1) / pageLimit)),
                           new XAttribute("currentPage", page));

            foreach (var file in files)
            {
                int totalpages = Comic.LoadFromFile(file).Pages.Count;

                try
                {
                    info.Add(new XElement("file",
                                 new XAttribute("name", file.Name),
                                 new XAttribute("path", GetRelativePath(file, file.Directory, mediafolder)),
                                 new XAttribute("totalpages", totalpages)));
                }
                catch
                {
                    //Debug.WriteLine($"Unable to find page count for: {file.Name}");
                    Logger.Error($"Unable to find page count for: {file.Name}");
                    continue;
                }
            }
            SetDefaultXmlNamespace(info , "http://www.de-baay.nl/ComicCloud");
            return info;
        }

        public static void SetDefaultXmlNamespace(XElement xelem, XNamespace xmlns)
        {
            if (xelem.Name.NamespaceName == string.Empty)
                xelem.Name = xmlns + xelem.Name.LocalName;
            foreach (var e in xelem.Elements())
                SetDefaultXmlNamespace(e, xmlns);
        }

        private static string GetRelativePath(FileInfo file, DirectoryInfo dir, DirectoryInfo mediafolder)
        {
            var folder = dir.Name;

            if (mediafolder.Name != dir.Name)
            {
                do
                {
                    folder = dir.Parent.Name + "\\" + folder;
                    dir = dir.Parent;
                }
                while (mediafolder.Name != dir.Name);
            }
            folder = folder.Substring(mediafolder.Name.Length);
            folder = folder + "\\" + file.Name.ToString();
            return folder;
        }
    }
}
