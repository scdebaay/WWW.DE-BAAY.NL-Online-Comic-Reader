using ComicReaderWebCore.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace ComicReaderWebCore.Data
{
    public class ComicStateProvider : IComicStateProvider
    {
        public ComicStateProvider()
        {
            ComicsVisited = new Dictionary<string, int>();
            ComicsVisited.Add("", 0);
        }
        public ComicModel CurrentComic { get; set; }
        public Dictionary<string, int> ComicsVisited { get; set; }
        public int PageSize { get; set; }
    }
}
