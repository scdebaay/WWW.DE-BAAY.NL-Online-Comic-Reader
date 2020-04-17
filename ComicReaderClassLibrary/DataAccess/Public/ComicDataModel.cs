using System;
using System.Collections.Generic;
using System.Text;

namespace ComicReaderClassLibrary.DataAccess.Public
{
    public class ComicDataModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Path { set; get; }
        public string Name { set; get; }
        public int TotalPages { set; get; }
        public DateTime PublicationDate { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int AuthorId { get; set; }
        public int LanguageId { get; set; }
        public int TypeId { get; set; }
        public int GenreId { get; set; }
        public int SeriesId { get; set; }
        public int SubSeriesId { get; set; }
        public int PublishersId { get; set; }
        public DateTime RecordUpdated { get; set; }
        public DateTime RecordDeleted { get; set; }
    }
}
