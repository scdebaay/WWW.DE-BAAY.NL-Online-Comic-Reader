using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Configuration;

namespace WWW.DE_BAAY.NL_Online_Comic_Reader.Resources
{
    public class ApiConfiguration
    {
        //Start logger
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        /// <summary>
        /// Local backing field of type ApiConfiguration
        /// </summary>        
        private static ApiConfiguration _apiConfiguration;

        /// <summary>
        /// Public constructor for the ApiConfiguration singleton
        /// </summary>        
        public static ApiConfiguration Settings
        {
            get
            {
                if (_apiConfiguration == null)
                {
                    _apiConfiguration = new ApiConfiguration();
                }

                return _apiConfiguration;
            }
        }

        /// <summary>
        /// Singleton accessor
        /// </summary>
        /// <param name="name">Will try to find the requested property in the WebConfugrationManager.Appsettings</param>
        /// <returns>Emtpy string if setting key of type <name> is not found</name></returns>
        public string this[string name]
        {
            get
            {

                  try
                    {
                        return WebConfigurationManager.AppSettings[name].ToString();
                    }
                    catch
                    {
                        //Debug.WriteLine($"Unable to retrieve setting '{name}'");
                        Logger.Error($"Unable to retrieve setting '{name}'");
                    return string.Empty;
                    }
                
            }
        }

        /// <summary>
        /// Based on the requested file name the folder to browse is set. For now this can be either Comic or Image
        /// </summary>
        /// <param name="requestedFileName">Filename that was requested in the request being processed.</param>
        /// <returns></returns>
        public static string ServerPath(string requestedFileName)
        {
            ContType requestedFile;
            if (requestedFileName != null)
            {
                requestedFile = new ContType(requestedFileName);
            }
            else
            {
                requestedFile = new ContType(DefaultFileType);
            }
            DefaultFileType = requestedFile.Extension;
            //Based on the media type requested currentFolder is set to either comicFolder or imageFolder.
            currentFolder = _apiConfiguration.FindFolder(requestedFile.FolderType);
            //If the path is not relative to the Api App folder, serverPath property is set.
            if (Path.IsPathRooted(currentFolder))
            {
                return String.Empty;
            }
            else
            {
                return HttpRuntime.AppDomainAppPath;
            }
        }

        /// <summary>
        /// Based on the requested file extension the folder to browse is set. For now this can be either Comic or Image
        /// </summary>
        /// <param name="requestedFileFolder">Folder type is passed in and the path is returned.</param>
        /// <returns></returns>
        private string FindFolder(string requestedFileFolder)
        {
            return GetType().GetProperty(requestedFileFolder).GetValue(Settings[requestedFileFolder]).ToString();
        }

        public static string MediaFolder { get { return Settings["mediaFolder"]; } }
        public static string ImageFolder { get { return Settings["imageFolder"]; } }
        public static string MusicFolder { get { return Settings["musicFolder"]; } }
        public static string FileFolder { get { return Settings["fileFolder"]; } }
        public static string ComicFolder { get { return Settings["comicFolder"]; } }
        public static string EpubFolder { get { return Settings["epubFolder"]; } }
        public static string DefaultFile { get { return Settings["defaultFile"]; } }
        public static int PageLimit { get { return int.Parse(Settings["pageLimit"]); } }
        private static  string currentFolder;
        public static string DefaultFileType { private set; get; }
    }
}