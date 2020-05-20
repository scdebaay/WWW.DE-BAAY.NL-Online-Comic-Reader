using ComicReaderClassLibrary.DataAccess.DataModels;
using ComicReaderClassLibrary.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace ComicReaderClassLibrary.DataAccess.Implementations
{    
    /// <summary>
    /// Class interface definition for the API, that defines the object to be returned from database queries.
    /// </summary>
    public interface ISqlApiDbConnection
    {
        /// <summary>
        /// RootFolder object retrieval method to retrieve paged list of files/comics from the database.
        /// </summary>
        /// <param name="pageLimit">Int, Optional, representing the amount of records to retrieve in one request.</param>
        /// <param name="page">Int, Optional, representing the set of records to be retrieved.</param>
        /// <returns>Ojects that implements IRootModel containing database query results or one dummy record.</returns>
        public IRootModel RetrieveComics(int? pageLimit, int? page);
    }
}