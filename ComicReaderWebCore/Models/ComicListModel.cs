using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace ComicReaderWebCore.Models
{
    public class ComicListModel
    {
        List<ComicModel> files = new List<ComicModel>();
        IConfiguration _config;
        public ComicListModel(IConfiguration config)
        {
            _config = config;
            Name = "Folder not defined.";
            AvailableFiles = 1;
            TotalPages = 1;
            CurrentPage = 1;
            ComicModel emtpyComic = new ComicModel("", "File not found", 0, _config);
            files.Add(emtpyComic);
        }

        public ComicListModel(JObject folder, IConfiguration config)
        {
            _config = config;
            Name = (string)folder["folder"]["name"];

            AvailableFiles = string.IsNullOrEmpty((string)folder["folder"]["files"]) ? 1 : int.TryParse((string)folder["folder"]["files"],out int testAFiles) ? AvailableFiles = testAFiles : AvailableFiles = 1;

            TotalPages = string.IsNullOrEmpty((string)folder["folder"]["totalPages"]) ? 1 : int.TryParse((string)folder["folder"]["totalPages"], out int testTPages) ? TotalPages = testTPages : TotalPages = 1;

            CurrentPage = string.IsNullOrEmpty((string)folder["folder"]["currentPage"]) ? 1 : int.TryParse((string)folder["folder"]["currentPage"], out int testCPage) ? CurrentPage = testCPage : CurrentPage = 1;

            JArray fileArray = (JArray)folder["folder"]["file"];

            if (fileArray.HasValues)
            {
                foreach (var comicFile in fileArray.Children())
                {
                    ComicModel comic = new ComicModel((string)comicFile["path"], ((string)comicFile["name"]).Substring(0, ((string)comicFile["name"]).Length - 4), comicFile["totalpages"].ToObject<int>(), _config);
                    files.Add(comic);
                }
            }
            else
            {
                ComicModel comic = new ComicModel(_config);
                files.Add(comic);
            }

        }

        public string Name { get; private set; }
        public int AvailableFiles { get; private set; }
        public int TotalPages { get; private set; }
        public int CurrentPage { get; private set; }
        public List<ComicModel> Files { get {
                return files;
            } private set { } }
    }
}
