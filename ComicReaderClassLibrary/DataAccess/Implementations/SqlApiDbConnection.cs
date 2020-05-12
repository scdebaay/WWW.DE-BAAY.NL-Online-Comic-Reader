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

        public SqlApiDbConnection(IConfiguration configuration, IRootModel rootModel, ILogger<SqlApiDbConnection> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _rootModel = rootModel;
        }

        private string CnnVal(string name)
        {   
            return _configuration[$"ConnectionStrings:{name}"];
        }

        public IRootModel RetrieveComics(int? pageLimit, int? page)
        {
            _rootModel.folder = new FolderModel { name = "pub", currentPage = page.ToString() };

            using (IDbConnection connection = new SqlConnection(CnnVal("Default")))
            {
                try
                {
                    _rootModel.folder.files = connection.Query<int>("SELECT COUNT(*) FROM [dbo].[Comics]").AsList()[0].ToString();
                    _rootModel.folder.totalPages = (int.Parse(_rootModel.folder.files) / pageLimit).ToString();
                    _rootModel.folder.file = connection.Query<FileModel>("dbo.spRetrievePagedListOfComics @pageLimit, @page", new { pageLimit = pageLimit, page = page }).AsList();
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
