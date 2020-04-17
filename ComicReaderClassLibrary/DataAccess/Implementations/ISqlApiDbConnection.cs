using ComicReaderClassLibrary.DataAccess.Public;
using ComicReaderClassLibrary.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace ComicReaderClassLibrary.DataAccess.Implementations
{
    public interface ISqlApiDbConnection
    {
        public IRootModel RetrieveComics(int? pageLimit, int? page);
    }
}