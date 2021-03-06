﻿using ComicReaderClassLibrary.DataAccess.DataModels;
using ComicReaderClassLibrary.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace ComicReaderClassLibrary.DataAccess.Implementations
{
    /// <summary>
    /// Interface definition for SqlIngestDbConnection object with which the Ingest service inserts files into the database.
    /// </summary>
    public interface ISqlIngestDbConnection
    {
        /// <summary>
        /// Method to ingest Comics in FileModel format.
        /// </summary>
        /// <param name="comicsToIngest">List of FileModels, representing comics to be ingested into the database.</param>
        void InsertComics(List<FileModel> comicsToIngest);

        /// <summary>
        /// Public method to set internal connectionstring for multiple database use.
        /// </summary>
        /// <param name="connectionString">string representing the connectionstring to the database</param>
        public void SetConnectionString(string connectionString);
    }
}