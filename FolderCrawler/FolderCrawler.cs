using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;

namespace FolderCrawler
{
    public partial class FolderCrawler
    {
        public static XElement GetDirectoryXml(DirectoryInfo dir, DirectoryInfo mediafolder, int? pageLimit, int? page)
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
