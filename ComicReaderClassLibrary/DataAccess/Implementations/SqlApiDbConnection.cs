using ComicReaderClassLibrary.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;

namespace ComicReaderClassLibrary.DataAccess.Implementations
{
    public class SqlApiDbConnection : ISqlApiDbConnection
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private IRootModel _rootModel;

        /// <summary>
        /// Constructor, creates a database context object for Api access
        /// </summary>
        /// <param name="configuration">Injects Configuration object from which connection string is retrieved</param>
        /// <param name="rootModel">Injects RootModel, with which data is returned from the database to the API</param>
        /// <param name="logger">Injects Logger object which logs error messages to file</param>
        public SqlApiDbConnection(IConfiguration configuration, IRootModel rootModel, ILogger<SqlApiDbConnection> logger)
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
        /// RootFolder object retrieval method to retrieve paged list of files/comics from the database.
        /// </summary>
        /// <param name="pageLimit">Int, Optional, representing the amount of records to retrieve in one request.</param>
        /// <param name="page">Int, Optional, representing the set of records to be retrieved.</param>
        /// <returns>Ojects that implements IRootModel containing database query results or one dummy record.</returns>
        public IRootModel RetrieveComics(int? pageLimit, int? page, string? searchText)
        {
            _rootModel.folder = new FolderModel { name = "pub", currentPage = page.ToString() };

            using (IDbConnection connection = new SqlConnection(CnnVal("Default")))
            {
                try
                {
                    if (!string.IsNullOrEmpty(searchText))
                    {
                        _rootModel.folder.files = connection.Query<int>($"SELECT COUNT(*) FROM [dbo].[Comics] WHERE [ComicDeleted] = 0 AND @SearchText = '' OR [ComicDeleted] = 0 AND [Name] LIKE '%' + @SearchText + '%' OR [ComicDeleted] = 0 AND[Path] LIKE '%' + @SearchText + '%';", new { SearchText = searchText }).AsList()[0].ToString();
                    }
                    else 
                    {
                        _rootModel.folder.files = connection.Query<int>($"SELECT COUNT(*) FROM [dbo].[Comics];").AsList()[0].ToString();
                        searchText = "";
                    }
                    _rootModel.folder.totalPages = (int.Parse(_rootModel.folder.files) / pageLimit).ToString();
                    _rootModel.folder.file = connection.Query<FileModel>("dbo.spRetrievePagedListOfComics @pageLimit, @page, @SearchText", new { pageLimit = pageLimit, page = page, SearchText = searchText }).AsList();
                    return _rootModel;
                }
                catch (SqlException ex)
                {
                    _logger.LogError($"Error retrieving comics from database: {ex.Message}");
                    _logger.LogError($"Stacktrace: {ex.StackTrace}");
                    FileModel comicNotFound = new FileModel
                    {
                        name = "NotFound",
                        path = "\\Images\\NotFound.jpg",
                        totalpages = "1"
                    };
                    _rootModel.folder.file.Add(comicNotFound);
                    return _rootModel;
                }
                finally
                {
                    connection.Close();
                }
            }
        }


    }
}
