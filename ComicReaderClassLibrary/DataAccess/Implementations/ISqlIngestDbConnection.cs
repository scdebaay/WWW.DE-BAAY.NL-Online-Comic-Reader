using ComicReaderClassLibrary.DataAccess.DataModels;
using ComicReaderClassLibrary.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace ComicReaderClassLibrary.DataAccess.Implementations
{
    public interface ISqlIngestDbConnection
    {
        void InsertComics(List<FileModel> comicsToIngest, ILogger _logger);
        ComicDataModel RetrieveComic(string name);
    }
}