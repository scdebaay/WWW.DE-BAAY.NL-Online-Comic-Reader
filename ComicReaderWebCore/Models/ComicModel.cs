using ComicReaderWebCore.Data;
using Microsoft.Extensions.Configuration;

namespace ComicReaderWebCore.Models
{
    public class ComicModel
    {
        private readonly IConfiguration _config;
        public ComicModel(IConfiguration config)
        {
            _config = config;
            Path = "";
            Title = "Comic not found";
            ThumbUrl = $"{_config.GetValue<string>("ApiLocation")}?file=NotFound&page=0&size=150";
        }

        public ComicModel(string path, string name, int totalpages, IConfiguration config)
        {
            _config = config;
            Path = path;
            Title = name;
            ThumbUrl = $"{_config.GetValue<string>("ApiLocation")}?file={Path}&page=0&size=150";
            TotalPages = totalpages;
        }

        public string Path { get; private set; }

        public string ThumbUrl { get; set; }

        public string Title { get; set; }

        public int TotalPages { get; set; }
    }
}
