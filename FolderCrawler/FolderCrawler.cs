using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.IO;

namespace FolderCrawler
{
    public partial class FolderCrawler
    {
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
            Stack<DirectoryInfo> dirs = new Stack<DirectoryInfo>(20);
            List<FileInfo> fileInfos = new List<FileInfo>();

            if (!System.IO.Directory.Exists(dir.FullName))
            {
                throw new ArgumentException();
            }
            dirs.Push(dir);

            while (dirs.Count > 0)
            {
                DirectoryInfo currentDir = dirs.Pop();
                string[] subDirs;
                try
                {
                    subDirs = System.IO.Directory.GetDirectories(currentDir.FullName);
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
                catch (UnauthorizedAccessException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
                catch (System.IO.DirectoryNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }

                //string[] files = null;
                try
                {
                    //files = System.IO.Directory.GetFiles(currentDir);
                    foreach (var file in currentDir.GetFiles("*"))
                    {
                        if (!file.Attributes.HasFlag(FileAttributes.Hidden) | !file.Attributes.HasFlag(FileAttributes.System))
                        {
                            fileInfos.Add(file);
                        }
                    }
                }
                catch (UnauthorizedAccessException e)
                {

                    //Console.WriteLine(e.Message);
                    continue;
                }
                catch (System.IO.DirectoryNotFoundException e)
                {
                    //Console.WriteLine(e.Message);
                    continue;
                }
                // Push the subdirectories onto the stack for traversal.
                // This could also be done before handing the files.
                foreach (string str in subDirs)
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(str);
                    dirs.Push(dirInfo);
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
                try
                {
                    info.Add(new XElement("file",
                                 new XAttribute("name", file.Name),
                                 new XAttribute("path", GetRelativePath(file, file.Directory, mediafolder))));
                }
                catch
                {
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
