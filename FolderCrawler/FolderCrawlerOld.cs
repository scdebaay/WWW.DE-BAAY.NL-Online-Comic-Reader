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
        public static XElement GetDirectoryXmlOld(DirectoryInfo dir, DirectoryInfo mediafolder, int? pageLimit, int? page)
        {
            List<FileInfo> fileInfos = new List<FileInfo>();
            try
            {            
            foreach (var file in dir.GetFiles("*", SearchOption.AllDirectories))
            {
                    if (!file.Attributes.HasFlag(FileAttributes.Hidden) | !file.Attributes.HasFlag(FileAttributes.System)) { 
                        fileInfos.Add(file);
                    }
                }
            }
                catch (UnauthorizedAccessException)
            {}            
            
            FileInfo[] files = fileInfos.Skip(((int)page - 1) * (int)pageLimit).Take((int)pageLimit).ToArray();

            var info = new XElement("folder",
                           new XAttribute("name", dir.Name),
                           new XAttribute("files", fileInfos.Count().ToString()),
                           new XAttribute("totalPages",((fileInfos.Count()+pageLimit - 1)/pageLimit)),
                           new XAttribute("currentPage",page));

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
    }
}
