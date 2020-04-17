using ComicReaderClassLibrary.DataAccess.Public;
using ComicReaderClassLibrary.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace ComicReaderClassLibrary.DataAccess.Implementations
{
    public class SqlApiDbConnection : ISqlApiDbConnection
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private IRootModel _rootModel;

        public SqlApiDbConnection(IConfiguration configuration, IRootModel rootModel)
        {
            _configuration = configuration;
            //_logger = logger;
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
                _rootModel.folder.files = connection.Query<int>("SELECT COUNT(*) FROM [dbo].[Comics]").AsList()[0].ToString();
                _rootModel.folder.totalPages = (connection.Query<int>("SELECT COUNT(*) FROM [dbo].[Comics]").AsList()[0] / pageLimit).ToString();
                _rootModel.folder.file = connection.Query<FileModel>("dbo.spRetrievePagedListOfComics @pageLimit, @page", new { pageLimit = pageLimit, page = page }).AsList();
                return _rootModel;
                                
            }
        }


    }
}
