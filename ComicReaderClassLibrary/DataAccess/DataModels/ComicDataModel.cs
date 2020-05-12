using System;
using System.Collections.Generic;
using System.Text;

namespace ComicReaderClassLibrary.DataAccess.DataModels
{ 
    public class ComicDataModel
    {
        public int Id { get; set; }
        public string Path { set; get; }
        public string Name { set; get; }
        public int TotalPages { set; get; }
        public DateTime PublicationDate { get; set; } = DateTime.Now;
        public DateTime ReleaseDate { get; set; } = DateTime.Now;
        public int LanguageId { get; set; }
        public int TypeId { get; set; }
        public int SeriesId { get; set; }
        public int SubSeriesId { get; set; }
        public DateTime RecordUpdated { get; set; } = DateTime.Now;
        public DateTime RecordDeleted { get; set; } = DateTime.Now;
    }
}
