using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ComicReaderClassLibrary.Models;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;
using ComicReaderClassLibrary.DataAccess.Public;
using Microsoft.Extensions.Logging;

namespace ComicReaderClassLibrary.DataAccess.Implementations
{
    public class SqlIngestDbConnection : ISqlIngestDbConnection
    {
        private IConfiguration _configuration;

        private string CnnVal(string name)
        {
            _configuration = LoadAppConfiguration();
            return _configuration[$"ConnectionStrings:{name}"];
        }

        public ComicDataModel RetrieveComic(string name)
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

        public void InsertComics(List<FileModel> comicsToIngest, ILogger _logger)
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

        private static IConfigurationRoot LoadAppConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddJsonFile("appsettings.local.json", optional: true)
                .Build();
        }
    }
}
