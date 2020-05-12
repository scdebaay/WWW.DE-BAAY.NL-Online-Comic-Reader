using System;
using System.Collections.Generic;
using System.Text;

namespace ComicReaderClassLibrary.DataAccess.DataModels
{
    public class SeriesDataModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
    }
}
