using ComicReaderWebCore.Models;
using System.Collections.Generic;

namespace ComicReaderWebCore.Data
{
    public class ComicStateProvider
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
