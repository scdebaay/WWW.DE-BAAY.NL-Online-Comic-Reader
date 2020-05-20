using ComicReaderWebCore.Data;
using Microsoft.Extensions.Configuration;

namespace ComicReaderWebCore.Models
{
    /// <summary>
    /// Class representing a comic to be viewed on the website.
    /// </summary>
    public class ComicModel
    {
        private readonly IConfiguration _config;
        /// <summary>
        /// Default constructor, creates default instance of a Comic Model with placeholder properties
        /// </summary>
        /// <param name="config">Injected configuration object</param>
        public ComicModel(IConfiguration config)
        {
            _config = config;
            Path = "";
            Title = "Comic not found";
            ThumbUrl = $"{_config.GetValue<string>("ApiLocation")}image/notfound.png?size=150";
            TotalPages = 1;
        }

        /// <summary>
        /// Constructor with which comic models are created to be displayed on the website. Properties are set to those retrieved from the API.
        /// </summary>
        /// <param name="path">String, path for the comic retrieved</param>
        /// <param name="name">String, title of the comic retrieved, this defaults to the filename</param>
        /// <param name="totalpages">Int, avaiable pages for the comic retrieved</param>
        /// <param name="config">Injected configuration object</param>
        public ComicModel(string path, string name, int totalpages, IConfiguration config)
        {
            _config = config;
            Path = path;
            Title = name;
            ThumbUrl = $"{_config.GetValue<string>("ApiLocation")}comic{Path}/0?size=150";
            TotalPages = totalpages;
        }

        public string Path { get; private set; }

        public string ThumbUrl { get; set; }

        public string Title { get; set; }

        public int TotalPages { get; set; }
    }
}
