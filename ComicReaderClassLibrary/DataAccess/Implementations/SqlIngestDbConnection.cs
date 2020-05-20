using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ComicReaderClassLibrary.Models;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;
using ComicReaderClassLibrary.DataAccess.DataModels;
using Microsoft.Extensions.Logging;

namespace ComicReaderClassLibrary.DataAccess.Implementations
{
    public class SqlIngestDbConnection : ISqlIngestDbConnection
    {
        private IConfiguration _configuration;
        private readonly ILogger _logger;
        private IRootModel _rootModel;

        /// <summary>
        /// Constructor, creates a database context object for Api access
        /// </summary>
        /// <param name="configuration">Injects Configuration object from which connection string is retrieved</param>
        /// <param name="rootModel">Injects RootModel, with which data is ingested into the database from the InventoryService</param>
        /// <param name="logger">Injects Logger object which logs error messages to file</param>
        public SqlIngestDbConnection(IConfiguration configuration, IRootModel rootModel, ILogger<SqlIngestDbConnection> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _rootModel = rootModel;
        }

        /// <summary>
        /// Connection string retrieval method
        /// </summary>
        /// <param name="name">String, represents the name of the connectionstring to retrieved from Settings.Json file.</param>
        /// <returns>String, representing the connection string, by name</returns>
        private string CnnVal(string name)
        {
            return _configuration[$"ConnectionStrings:{name}"];
        }

        /// <summary>
        /// ComicData object retrieval method to retrieve comic by name from the database.
        /// </summary>
        /// <param name="name">String, Representing whether the Comic to retrieve from the database.</param>
        /// <returns>Retrieves a ComicDataModel by name or, if the comic was nog found, one dummy record.</returns>
        private ComicDataModel RetrieveComic(string name)
        {
            using (IDbConnection connection = new SqlConnection(CnnVal("Default")))
            {
                var queryoutput = connection.Query<ComicDataModel>("dbo.spRetrieveComicByName @Name", new { Name = @name }).AsList();
                if (queryoutput.Count > 0)
                {
                    ComicDataModel result = queryoutput[0];
                    return result;
                }
                else 
                {
                    ComicDataModel result = new ComicDataModel { Name = "NotFound", Path = "NoPath", TotalPages = 0 };
                    return result;
                }                
            }
        }

        /// <summary>
        /// Method to ingest Comics in FileModel format.
        /// </summary>
        /// <param name="comicsToIngest">List of FileModels, representing comics to be ingested into the database.</param>
        public void InsertComics(List<FileModel> comicsToIngest)
        {

            using (IDbConnection connection = new SqlConnection(CnnVal("Default")))
            {
                List<ComicDataModel> comicsDataToIngest = new List<ComicDataModel>();
                List<ComicDataModel> comicsDataToUpdate = new List<ComicDataModel>();
                int comicsInDb = 0;
                foreach (var comic in comicsToIngest)
                {
                    if (!ComicExists(comic.name))
                    {
                        _logger.LogInformation($"Adding comic to ingest: {comic.name}");
                        comicsDataToIngest.Add(new ComicDataModel { Name = comic.name, Path = comic.path, TotalPages = int.Parse(comic.totalpages) });
                    }
                    else if (!ComicPathIsEqual(comic.name, comic.path))
                    {
                        _logger.LogInformation($"Adding comic to update: {comic.name}");
                        comicsDataToUpdate.Add(new ComicDataModel { Id = RetrieveComic(comic.name).Id, Name = comic.name, Path = comic.path, TotalPages = int.Parse(comic.totalpages) });
                    }
                    else
                    {
                        comicsInDb++;
                    }

                }
                _logger.LogInformation($"Starting Comic ingest at: {DateTimeOffset.Now}");
                connection.Execute("dbo.spInsertComics @Name, @Path, @TotalPages", comicsDataToIngest);
                _logger.LogInformation($"Starting Comic Updates at: {DateTimeOffset.Now}");
                connection.Execute("dbo.spUpdateComicById @Id, @Name, @Path, @TotalPages", comicsDataToUpdate);
                _logger.LogInformation($"{comicsInDb} were found but already in the DB at: {DateTimeOffset.Now}");
            }
        }

        /// <summary>
        /// Boolean method to check whether a Comic exists in theb database.
        /// </summary>
        /// <param name="name">String, representing the name of the comic to check for existence.</param>
        /// <returns>If the comic exists in the database the method returns true</returns>
        private bool ComicExists(string name)
        {
            if (RetrieveComic(name).Name == "NotFound")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Boolean method to check whether the requested comic is still at the same path as the comic stored in the database.
        /// </summary>
        /// <param name="name">String, representing the comic to check in the database.</param>
        /// <param name="path">String, the current path for the comic to be checked.</param>
        /// <returns>The name string parameter is checked in the database and if equal returns true.</returns>
        private bool ComicPathIsEqual(string name, string path)
        {
            if (RetrieveComic(name).Path != path)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
