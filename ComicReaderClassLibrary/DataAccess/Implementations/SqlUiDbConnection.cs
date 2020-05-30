using ComicReaderClassLibrary.Models;
using ComicReaderClassLibrary.DataAccess.DataModels;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Globalization;
using Caliburn.Micro;

namespace ComicReaderClassLibrary.DataAccess.Implementations
{
    /// <summary>
    /// UI Db Context for database related operations
    /// </summary>
    public class SqlUiDbConnection : ISqlUiDbConnection
    {
        #region Private fields
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        #endregion

        #region Instantiation and general configuration
        /// <summary>
        /// Constructor, creates a database context object for Api access
        /// </summary>
        /// <param name="configuration">Injects Configuration object from which connection string is retrieved</param>
        /// <param name="logger">Injects Logger object which logs error messages to file</param>
        public SqlUiDbConnection(IConfiguration configuration, ILogger<SqlUiDbConnection> logger)
        {
            _configuration = configuration;
            _logger = logger;
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
        #endregion

        #region Retrieval
        /// <summary>
        /// Async method to retrieve a list of ComicDataModels from the database
        /// </summary>
        /// <returns>Task of List of ComicDataModels representing all comics in the database</returns>
        async public Task<List<ComicDataModel>> RetrieveComicsAsync(string searchtext)
        {
            using (IDbConnection connection = new SqlConnection(CnnVal("Default")))
            {
                List<ComicDataModel> comicsResult = new List<ComicDataModel>();
                try
                {
                    var result = await connection.QueryAsync<ComicDataModel>("dbo.spRetrieveComics @SearchText", new { SearchText = searchtext });
                    comicsResult = result.AsList();
                    return comicsResult;
                }
                catch (SqlException ex)
                {
                    _logger.LogError($"Error retrieving comics from database: {ex.Message}");
                    _logger.LogError($"Stacktrace: {ex.StackTrace}");
                    ComicDataModel comicNotFound = new ComicDataModel
                    {
                        Id = 1,
                        Name = "Error connecting to database",
                        Path = "\\",
                        TotalPages = 0
                    };
                    comicsResult.Add(comicNotFound);
                    return comicsResult;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// Async method to retrieve a list of AuthorDataModels from the database for one specific Comic
        /// </summary>
        /// <param name="id">Int, representing the Id of the Comic for which to retrieve all Authors</param>
        /// <returns>Task of List of AuthorDataModels</returns>
        async public Task<List<AuthorDataModel>> RetrieveAuthorByComicIdAsync(int id)
        {
            using (IDbConnection connection = new SqlConnection(CnnVal("Default")))
            {
                List<AuthorDataModel> authorsResult = new List<AuthorDataModel>();
                try
                {
                    var result = await connection.QueryAsync<AuthorDataModel>("dbo.spRetrieveAuthorsByComicId @id", new { id = id });
                    authorsResult = result.AsList();
                    return authorsResult;
                }
                catch (SqlException ex)
                {
                    _logger.LogError($"Error retrieving authors from database: {ex.Message}");
                    _logger.LogError($"Stacktrace: {ex.StackTrace}");
                    AuthorDataModel authorNotFound = new AuthorDataModel
                    {
                        FirstName = "Author not found",
                        MiddleName = "",
                        LastName = "",
                        DateBirth = DateTime.Parse("1975-01-25"),
                        DateDeceased = DateTime.Parse("1975-01-25"),
                        Active = false
                    };
                    authorsResult.Add(authorNotFound);
                    return authorsResult;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// Async method to retrieve a list of GenreDataModels from the database for one specific Comic
        /// </summary>
        /// <param name="id">Int, representing the Id of the Comic for which to retrieve all Genres</param>
        /// <returns>Task of List of GenreDataModels</returns>
        async public Task<List<GenreDataModel>> RetrieveGenreByComicIdAsync(int id)
        {
            using (IDbConnection connection = new SqlConnection(CnnVal("Default")))
            {
                List<GenreDataModel> genresResult = new List<GenreDataModel>();
                try
                {
                    var result = await connection.QueryAsync<GenreDataModel>("dbo.spRetrieveGenresByComicId @id", new { id = id });
                    genresResult = result.AsList();
                    return genresResult;
                }
                catch (SqlException ex)
                {
                    _logger.LogError($"Error retrieving authors from database: {ex.Message}");
                    _logger.LogError($"Stacktrace: {ex.StackTrace}");
                    GenreDataModel genreNotFound = new GenreDataModel
                    {
                        Term = "Genre not found",
                    };
                    genresResult.Add(genreNotFound);
                    return genresResult;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// Async method to retrieve a list of PublisherDataModels from the database for one specific Comic
        /// </summary>
        /// <param name="id">Int, representing the Id of the Comic for which to retrieve all Publishers</param>
        /// <returns>Task of List of PublisherDataModels</returns>
        async public Task<List<PublisherDataModel>> RetrievePublisherByComicIdAsync(int id)
        {
            using (IDbConnection connection = new SqlConnection(CnnVal("Default")))
            {
                List<PublisherDataModel> publishersResult = new List<PublisherDataModel>();
                try
                {
                    var result = await connection.QueryAsync<PublisherDataModel>("dbo.spRetrievePublishersByComicId @id", new { id = id });
                    publishersResult = result.AsList();
                    return publishersResult;
                }
                catch (SqlException ex)
                {
                    _logger.LogError($"Error retrieving publishers from database: {ex.Message}");
                    _logger.LogError($"Stacktrace: {ex.StackTrace}");
                    PublisherDataModel genreNotFound = new PublisherDataModel
                    {
                        Name = "Genre not found",
                    };
                    publishersResult.Add(genreNotFound);
                    return publishersResult;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// Async method to retrieve a list of LanguageDataModels from the database for one specific Comic
        /// </summary>        
        /// <returns>Task of List of LanguageDataModels, normally one language is associated with one comic</returns>
        async public Task<List<LanguageDataModel>> RetrieveLanguageAsync()
        {
            using (IDbConnection connection = new SqlConnection(CnnVal("Default")))
            {
                List<LanguageDataModel> languageResult = new List<LanguageDataModel>();
                try
                {
                    var result = await connection.QueryAsync<LanguageDataModel>("dbo.spRetrieveLanguages");
                    languageResult = result.AsList();
                    return languageResult;
                }
                catch (SqlException ex)
                {
                    _logger.LogError($"Error retrieving languages from database: {ex.Message}");
                    _logger.LogError($"Stacktrace: {ex.StackTrace}");
                    LanguageDataModel languageNotFound = new LanguageDataModel
                    {
                        Term = "Language not found",
                    };
                    languageResult.Add(languageNotFound);
                    return languageResult;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// Async method to retrieve a list of TypeDataModels from the database for one specific Comic
        /// </summary>        
        /// <returns>Task of List of TypeDataModels, normally one type is associated with one comic</returns>
        async public Task<List<TypeDataModel>> RetrieveTypeAsync()
        {
            using (IDbConnection connection = new SqlConnection(CnnVal("Default")))
            {
                List<TypeDataModel> typeResult = new List<TypeDataModel>();
                try
                {
                    var result = await connection.QueryAsync<TypeDataModel>("dbo.spRetrieveTypes");
                    typeResult = result.AsList();
                    return typeResult;
                }
                catch (SqlException ex)
                {
                    _logger.LogError($"Error retrieving types from database: {ex.Message}");
                    _logger.LogError($"Stacktrace: {ex.StackTrace}");
                    TypeDataModel typeNotFound = new TypeDataModel
                    {
                        Term = "Type not found",
                    };
                    typeResult.Add(typeNotFound);
                    return typeResult;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// Async method to retrieve a list of SerieDataModels from the database for one specific Comic
        /// </summary>        
        /// <returns>Task of List of SerieDataModels, normally one serie is associated with multiple comics or subseries</returns>
        async public Task<List<SeriesDataModel>> RetrieveSerieAsync()
        {
            using (IDbConnection connection = new SqlConnection(CnnVal("Default")))
            {
                List<SeriesDataModel> serieResult = new List<SeriesDataModel>();
                try
                {
                    var result = await connection.QueryAsync<SeriesDataModel>("dbo.spRetrieveSeries");
                    serieResult = result.AsList();
                    return serieResult;
                }
                catch (SqlException ex)
                {
                    _logger.LogError($"Error retrieving series from database: {ex.Message}");
                    _logger.LogError($"Stacktrace: {ex.StackTrace}");
                    SeriesDataModel serieNotFound = new SeriesDataModel
                    {
                        Title = "Series not found",
                    };
                    serieResult.Add(serieNotFound);
                    return serieResult;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// Async method to retrieve a list of SubSerieDataModels from the database for one specific Comic
        /// </summary>        
        /// <returns>Task of List of SubSerieDataModels, normally one subserie is associated with multiple comics</returns>
        async public Task<List<SubSeriesDataModel>> RetrieveSubSerieAsync()
        {
            using (IDbConnection connection = new SqlConnection(CnnVal("Default")))
            {
                List<SubSeriesDataModel> subSerieResult = new List<SubSeriesDataModel>();
                try
                {
                    var result = await connection.QueryAsync<SubSeriesDataModel>("dbo.spRetrieveSubSeries");
                    subSerieResult = result.AsList();
                    return subSerieResult;
                }
                catch (SqlException ex)
                {
                    _logger.LogError($"Error retrieving subseries from database: {ex.Message}");
                    _logger.LogError($"Stacktrace: {ex.StackTrace}");
                    SubSeriesDataModel subseriesNotFound = new SubSeriesDataModel
                    {
                        Title = "Subseries not found",
                    };
                    subSerieResult.Add(subseriesNotFound);
                    return subSerieResult;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// Async method to retrieve a list of all PublisherDataModels from the database.
        /// </summary>        
        /// <returns>Task of List of PublisherDataModels</returns>
        async public Task<List<PublisherDataModel>> RetrievePublisherAsync()
        {
            using (IDbConnection connection = new SqlConnection(CnnVal("Default")))
            {
                List<PublisherDataModel> publisherResult = new List<PublisherDataModel>();
                try
                {
                    var result = await connection.QueryAsync<PublisherDataModel>("dbo.spRetrievePublishers");
                    publisherResult = result.AsList();
                    return publisherResult;
                }
                catch (SqlException ex)
                {
                    _logger.LogError($"Error retrieving publishers from database: {ex.Message}");
                    _logger.LogError($"Stacktrace: {ex.StackTrace}");
                    PublisherDataModel publisherNotFound = new PublisherDataModel
                    {
                        Name = "Publisher not found",
                    };
                    publisherResult.Add(publisherNotFound);
                    return publisherResult;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// Async method to retrieve a list of all ComicDataModels from the database for a specific Publisher.
        /// </summary>
        /// <param name="id">Int, representing the Id of the Publisher for which to retrieve all Comics</param>
        /// <returns>Task of List of ints, representing the Id's of the Comics pertaining to a Publisher</returns>
        async public Task<List<int>> RetrieveComicsByPublisherIdAsync(int Id)
        {
            using (IDbConnection connection = new SqlConnection(CnnVal("Default")))
            {
                List<int> comicResult = new List<int>();
                try
                {
                    var publisherIdParam = new DynamicParameters();
                    publisherIdParam.Add("@PublisherId", Id);
                    var result = await connection.QueryAsync<int>("dbo.spRetrieveComicsByPublisherId", publisherIdParam, null, commandType: CommandType.StoredProcedure); ;
                    comicResult = result.AsList();
                    return comicResult;
                }
                catch (SqlException ex)
                {
                    _logger.LogError($"Error retrieving comics for publisher with id {Id} from database: {ex.Message}");
                    _logger.LogError($"Stacktrace: {ex.StackTrace}");
                    int comicNotFound = 0;                    
                    comicResult.Add(comicNotFound);
                    return comicResult;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// Async method to retrieve a list of all GenreDataModels from the database.
        /// </summary>        
        /// <returns>Task of List of GenreDataModels</returns>
        async public Task<List<GenreDataModel>> RetrieveGenreAsync()
        {
            using (IDbConnection connection = new SqlConnection(CnnVal("Default")))
            {
                List<GenreDataModel> genreResult = new List<GenreDataModel>();
                try
                {
                    var result = await connection.QueryAsync<GenreDataModel>("dbo.spRetrieveGenres");
                    genreResult = result.AsList();
                    return genreResult;
                }
                catch (SqlException ex)
                {
                    _logger.LogError($"Error retrieving genre from database: {ex.Message}");
                    _logger.LogError($"Stacktrace: {ex.StackTrace}");
                    GenreDataModel genreNotFound = new GenreDataModel
                    {
                        Term = "Genre not found",
                    };
                    genreResult.Add(genreNotFound);
                    return genreResult;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// Async method to retrieve a list of all ComicDataModels from the database for a specific Genre.
        /// </summary>
        /// <param name="id">Int, representing the Id of the Genre for which to retrieve all Comics</param>
        /// <returns>Task of List of ints, representing the Id's of the Comics pertaining to a Genre</returns>
        async public Task<List<int>> RetrieveComicsByGenreIdAsync(int Id)
        {
            using (IDbConnection connection = new SqlConnection(CnnVal("Default")))
            {
                List<int> comicResult = new List<int>();
                try
                {
                    var genreIdParam = new DynamicParameters();
                    genreIdParam.Add("@GenreId", Id);
                    var result = await connection.QueryAsync<int>("dbo.spRetrieveComicsByGenreId", genreIdParam, null, commandType: CommandType.StoredProcedure); ;
                    comicResult = result.AsList();
                    return comicResult;
                }
                catch (SqlException ex)
                {
                    _logger.LogError($"Error retrieving comics for genre with id {Id} from database: {ex.Message}");
                    _logger.LogError($"Stacktrace: {ex.StackTrace}");
                    int comicNotFound = 0;
                    comicResult.Add(comicNotFound);
                    return comicResult;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// Async method to retrieve a list of all AuthorDataModels from the database.
        /// </summary>        
        /// <returns>Task of List of AuthorDataModels</returns>
        async public Task<List<AuthorDataModel>> RetrieveAuthorAsync()
        {
            using (IDbConnection connection = new SqlConnection(CnnVal("Default")))
            {
                List<AuthorDataModel> authorResult = new List<AuthorDataModel>();
                try
                {
                    var result = await connection.QueryAsync<AuthorDataModel>("dbo.spRetrieveAuthors");
                    authorResult = result.AsList();
                    return authorResult;
                }
                catch (SqlException ex)
                {
                    _logger.LogError($"Error retrieving authors from database: {ex.Message}");
                    _logger.LogError($"Stacktrace: {ex.StackTrace}");
                    AuthorDataModel authorNotFound = new AuthorDataModel
                    {
                        FirstName = "Author not found",
                        MiddleName = "",
                        LastName = "",
                        DateBirth = DateTime.Now,
                        DateDeceased = DateTime.Now
                    };
                    authorResult.Add(authorNotFound);
                    return authorResult;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// Async method to retrieve a list of all ComicDataModels from the database for a specific Author.
        /// </summary>
        /// <param name="id">Int, representing the Id of the Author for which to retrieve all Comics</param>
        /// <returns>Task of List of ints, representing the Id's of the Comics pertaining to an Author</returns>
        async public Task<List<int>> RetrieveComicsByAuthorIdAsync(int Id)
        {
            using (IDbConnection connection = new SqlConnection(CnnVal("Default")))
            {
                List<int> comicResult = new List<int>();
                try
                {
                    var authorIdParam = new DynamicParameters();
                    authorIdParam.Add("@AuthorId", Id);
                    var result = await connection.QueryAsync<int>("dbo.spRetrieveComicsByAuthorId", authorIdParam, null, commandType: CommandType.StoredProcedure); ;
                    comicResult = result.AsList();
                    return comicResult;
                }
                catch (SqlException ex)
                {
                    _logger.LogError($"Error retrieving comics for author with id {Id} from database: {ex.Message}");
                    _logger.LogError($"Stacktrace: {ex.StackTrace}");
                    int comicNotFound = 0;
                    comicResult.Add(comicNotFound);
                    return comicResult;
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        #endregion

        #region Updating
        /// <summary>
        /// Method to store a ComicDataModel in the database
        /// </summary>
        /// <param name="comic">ComicDataModel, representing the Comic to be stored int he database.</param>
        /// <returns>Boolean, true if storage was usccessfull and false if something went wrong.</returns>
        public bool SaveComic(ComicDataModel comic)
        {
            using (IDbConnection connection = new SqlConnection(CnnVal("Default")))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var insertComic = new DynamicParameters();
                        insertComic.Add("@Id", comic.Id);
                        insertComic.Add("@Name", comic.Name);
                        insertComic.Add("@Path", comic.Path);
                        insertComic.Add("@TotalPages", comic.TotalPages);
                        insertComic.Add("@PublicationDate", comic.PublicationDate);
                        insertComic.Add("@ReleaseDate", comic.ReleaseDate);
                        insertComic.Add("@LanguageId", comic.LanguageId);
                        insertComic.Add("@TypeId", comic.TypeId);
                        if (comic.SeriesId > 0)
                        {
                            insertComic.Add("@SeriesId", comic.SeriesId);
                        }
                        if (comic.SeriesId > 0)
                        {
                            insertComic.Add("@SubSeriesId", comic.SubSeriesId);
                        }

                        var exec = connection.Query<int>("dbo.spUpdateComicById", insertComic, transaction, commandType: CommandType.StoredProcedure);
                        var result = exec.AsList();
                        var success = (result.Count == 0) ? true : false;
                        transaction.Commit();
                        return success;

                    }
                    catch (SqlException ex)
                    {
                        _logger.LogError($"Error saving comic to database: {ex.Message}");
                        _logger.LogError($"SQL Error number is: {ex.Number}");
                        _logger.LogError($"Stacktrace: {ex.StackTrace}");
                        transaction.Rollback();
                        return false;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Method to store a Author to Comic Association in the database
        /// </summary>
        /// <param name="comicAuthorAssoc">List of ComicToAuthorModels, representing the Comic to Author Association to store in the database.</param>
        /// <returns>Boolean, true if storage was usccessfull and false if something went wrong.</returns>
        public bool SaveAuthorAssoc(List<ComicToAuthorsDataModel> comicAuthorAssoc)
        {
            using (IDbConnection connection = new SqlConnection(CnnVal("Default")))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var insertAssoc = new DynamicParameters();
                        for (int i = 0; i < comicAuthorAssoc.Count; i++)
                        {
                            insertAssoc.Add("@ComicId", comicAuthorAssoc[i].ComicId);
                            insertAssoc.Add("@AuthorId", comicAuthorAssoc[i].AuthorId);
                        }
                        var exec = connection.Query<int>("dbo.spInsertComicToAuthor", insertAssoc, transaction, commandType: CommandType.StoredProcedure);
                        var result = exec.AsList();
                        var success = (result.Count == 0) ? true : false;
                        transaction.Commit();
                        return success;

                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 2601 || ex.Number == 2627)
                        {
                            return true;
                        }
                        else
                        {
                            _logger.LogError($"Error saving author association to database: {ex.Message}");
                            _logger.LogError($"SQL Error number is: {ex.Number}");
                            _logger.LogError($"Stacktrace: {ex.StackTrace}");
                            transaction.Rollback();
                            return false;
                        }
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Method to update an Author record in the database. The author exists in the database.
        /// </summary>
        /// <param name="id">Int, representing the Id of the Author to store.</param>
        /// <param name="firstname">String, representing the FirstName field for the Author</param>
        /// <param name="middlename">String, representing the MiddleName field for the Author</param>
        /// <param name="lastname">String, representing the LastName field for the Author</param>
        /// <param name="datebirth">DateTime, representing the DateBirth field for the Author</param>
        /// <param name="datedeceased">DateTime, representing the DateDeceased field for the Author</param>
        /// <param name="active">Boolean, representing the Active field for the Author</param>
        /// <returns>Boolean, true if storage was usccessfull and false if something went wrong.</returns>
        public bool UpdateAuthor(int id, string firstname, string middlename, string lastname, DateTime datebirth, DateTime datedeceased, bool active)
        {
            using (IDbConnection connection = new SqlConnection(CnnVal("Default")))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var insertAuthor = new DynamicParameters();
                        insertAuthor.Add("@Id", id);
                        insertAuthor.Add("@FirstName", firstname);
                        insertAuthor.Add("@MiddleName", middlename);
                        insertAuthor.Add("@LastName", lastname);
                        insertAuthor.Add("@DateBirth", datebirth);
                        insertAuthor.Add("@DateDeceased", datedeceased);
                        insertAuthor.Add("@Active", active);

                        var exec = connection.Query<int>("dbo.spUpdateAuthorById", insertAuthor, transaction, commandType: CommandType.StoredProcedure);
                        var result = exec.AsList();
                        var success = (result.Count == 0) ? true : false;
                        transaction.Commit();
                        return success;

                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 2601 || ex.Number == 2627)
                        {
                            return true;
                        }
                        else
                        {
                            _logger.LogError($"Error saving author to database: {ex.Message}");
                            _logger.LogError($"SQL Error number is: {ex.Number}");
                            _logger.LogError($"Stacktrace: {ex.StackTrace}");
                            transaction.Rollback();
                            return false;
                        }
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Method to insert an Author record in the database. The author does not yet exists in the database.
        /// </summary>
        /// <param name="firstname">String, representing the FirstName field for the Author</param>
        /// <param name="middlename">String, representing the MiddleName field for the Author</param>
        /// <param name="lastname">String, representing the LastName field for the Author</param>
        /// <param name="datebirth">DateTime, representing the DateBirth field for the Author</param>
        /// <param name="datedeceased">DateTime, representing the DateDeceased field for the Author</param>
        /// <param name="active">Boolean, representing the Active field for the Author</param>
        /// <returns>Int, the Id for the new record stored in the database</returns>
        public int InsertAuthor(string firstname, string middlename, string lastname, DateTime datebirth, DateTime datedeceased, bool active)
        {
            using (IDbConnection connection = new SqlConnection(CnnVal("Default")))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var insertAuthor = new DynamicParameters();
                        insertAuthor.Add("@FirstName", firstname);
                        insertAuthor.Add("@MiddleName", middlename);
                        insertAuthor.Add("@LastName", lastname);
                        insertAuthor.Add("@DateBirth", datebirth);
                        insertAuthor.Add("@DateDeceased", datedeceased);
                        insertAuthor.Add("@Active", active);
                        insertAuthor.Add(
                            name: "@Retval",
                            dbType: DbType.Int32,
                            direction: ParameterDirection.ReturnValue);

                        var exec = connection.Query<int>("dbo.spInsertAuthors", insertAuthor, transaction, commandType: CommandType.StoredProcedure);
                        int result = insertAuthor.Get<int>("@Retval");
                        int success = result > 0 ? result : 0;
                        transaction.Commit();
                        return success;

                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 2601 || ex.Number == 2627)
                        {
                            var insertAuthor = new DynamicParameters();
                            insertAuthor.Add("@FirstName", firstname);
                            insertAuthor.Add("@MiddleName", middlename);
                            insertAuthor.Add("@LastName", lastname);
                            insertAuthor.Add("@DateBirth", datebirth);
                            insertAuthor.Add("@DateDeceased", datedeceased);
                            insertAuthor.Add("@Active", active);
                            insertAuthor.Add(
                                name: "@Retval",
                                dbType: DbType.Int32,
                                direction: ParameterDirection.ReturnValue);

                            var exec = connection.Query<int>("dbo.spUndeleteAuthorByName", insertAuthor, transaction, commandType: CommandType.StoredProcedure);
                            int result = insertAuthor.Get<int>("@Retval");
                            int success = result > 0 ? result : 1;
                            transaction.Commit();
                            return success;
                        }
                        else
                        {
                            _logger.LogError($"Error storing author in the database: {ex.Message}");
                            _logger.LogError($"SQL Error number is: {ex.Number}");
                            _logger.LogError($"Stacktrace: {ex.StackTrace}");
                            transaction.Rollback();
                            return 0;
                        }
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Method to store a Publisher to Comic Association in the database
        /// </summary>
        /// <param name="comicPublisherAssoc">List of ComicToPublisherModels, representing the Comic to Publisher Association to store in the database.</param>
        /// <returns>Boolean, true if storage was usccessfull and false if something went wrong.</returns>
        public bool SavePublisherAssoc(List<ComicToPublishersDataModel> comicPublisherAssoc)
        {
            using (IDbConnection connection = new SqlConnection(CnnVal("Default")))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var insertAssoc = new DynamicParameters();
                        for (int i = 0; i < comicPublisherAssoc.Count; i++)
                        {
                            insertAssoc.Add("@ComicId", comicPublisherAssoc[i].ComicId);
                            insertAssoc.Add("@PublisherId", comicPublisherAssoc[i].PublisherId);
                        }
                        var exec = connection.Query<int>("dbo.spInsertComicToPublisher", insertAssoc, transaction, commandType: CommandType.StoredProcedure);
                        var result = exec.AsList();
                        var success = (result.Count == 0) ? true : false;
                        transaction.Commit();
                        return success;

                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 2601 || ex.Number == 2627)
                        {
                            return true;
                        }
                        else
                        {
                            _logger.LogError($"Error saving publisher association to database: {ex.Message}");
                            _logger.LogError($"SQL Error number is: {ex.Number}");
                            _logger.LogError($"Stacktrace: {ex.StackTrace}");
                            transaction.Rollback();
                            return false;
                        }
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Method to update an Publisher record in the database. The publisher exists in the database.
        /// </summary>
        /// <param name="id">Int, representing the Id of the Publisher to store.</param>
        /// <param name="name">String, representing the Name field for the Publisher</param>
        /// <returns>Boolean, true if storage was usccessfull and false if something went wrong.</returns>
        public bool UpdatePublisher(int id, string name)
        {
            using (IDbConnection connection = new SqlConnection(CnnVal("Default")))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var insertPublisher = new DynamicParameters();
                        insertPublisher.Add("@Name", name);
                        insertPublisher.Add("@Id", id);

                        var exec = connection.Query<int>("dbo.spUpdatePublisherById", insertPublisher, transaction, commandType: CommandType.StoredProcedure);
                        var result = exec.AsList();
                        var success = (result.Count == 0) ? true : false;
                        transaction.Commit();
                        return success;

                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 2601 || ex.Number == 2627)
                        {
                            return true;
                        }
                        else
                        {
                            _logger.LogError($"Error saving publisher to database: {ex.Message}");
                            _logger.LogError($"SQL Error number is: {ex.Number}");
                            _logger.LogError($"Stacktrace: {ex.StackTrace}");
                            transaction.Rollback();
                            return false;
                        }
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Method to insert a Publisher record in the database. The publisher does not yet exists in the database.
        /// </summary>
        /// <param name="name">String, representing the Name field for the Publisher</param>
        /// <returns>Int, the Id for the new record stored in the database</returns>
        public int InsertPublisher(string name)
        {
            using (IDbConnection connection = new SqlConnection(CnnVal("Default")))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var insertPublisher = new DynamicParameters();
                        insertPublisher.Add("@Name", name);
                        insertPublisher.Add(
                            name: "@Retval",
                            dbType: DbType.Int32,
                            direction: ParameterDirection.ReturnValue);

                        var exec = connection.Query<int>("dbo.spInsertPublishers", insertPublisher, transaction, commandType: CommandType.StoredProcedure);
                        int result = insertPublisher.Get<int>("@Retval");
                        int success = result > 0 ? result : 0;
                        transaction.Commit();
                        return success;

                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 2601 || ex.Number == 2627)
                        {
                            var insertPublisher = new DynamicParameters();
                            insertPublisher.Add("@Name", name);
                            insertPublisher.Add(
                                name: "@Retval",
                                dbType: DbType.Int32,
                                direction: ParameterDirection.ReturnValue);

                            var exec = connection.Query<int>("dbo.spUndeletePublisherByName", insertPublisher, transaction, commandType: CommandType.StoredProcedure);
                            int result = insertPublisher.Get<int>("@Retval");
                            int success = result > 0 ? result : 1;
                            transaction.Commit();
                            return success;
                        }
                        else
                        {
                            _logger.LogError($"Error storing publisher in the database: {ex.Message}");
                            _logger.LogError($"SQL Error number is: {ex.Number}");
                            _logger.LogError($"Stacktrace: {ex.StackTrace}");
                            transaction.Rollback();
                            return 0;
                        }
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Method to store a Genre to Comic Association in the database
        /// </summary>
        /// <param name="comicGenreAssoc">List of ComicToGenreModels, representing the Comic to Genre Association to store in the database.</param>
        /// <returns>Boolean, true if storage was usccessfull and false if something went wrong.</returns>
        public bool SaveGenreAssoc(List<ComicToGenreDataModel> comicGenreAssoc)
        {
            using (IDbConnection connection = new SqlConnection(CnnVal("Default")))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var insertAssoc = new DynamicParameters();
                        for (int i = 0; i < comicGenreAssoc.Count; i++)
                        {
                            insertAssoc.Add("@ComicId", comicGenreAssoc[i].ComicId);
                            insertAssoc.Add("@GenreId", comicGenreAssoc[i].GenreId);
                        }
                        var exec = connection.Query<int>("dbo.spInsertComicToGenre", insertAssoc, transaction, commandType: CommandType.StoredProcedure);
                        var result = exec.AsList();
                        var success = (result.Count == 0) ? true : false;
                        transaction.Commit();
                        return success;

                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 2601 || ex.Number == 2627)
                        {
                            return true;
                        }
                        else
                        {
                            _logger.LogError($"Error saving genre association to database: {ex.Message}");
                            _logger.LogError($"SQL Error number is: {ex.Number}");
                            _logger.LogError($"Stacktrace: {ex.StackTrace}");
                            transaction.Rollback();
                            return false;
                        }
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Method to update an Genre record in the database. The genre exists in the database.
        /// </summary>
        /// <param name="id">Int, representing the Id of the Genre to store.</param>
        /// <param name="name">String, representing the Term field for the Genre</param>
        /// <returns>Boolean, true if storage was usccessfull and false if something went wrong.</returns>
        public bool UpdateGenre(int id, string name)
        {
            using (IDbConnection connection = new SqlConnection(CnnVal("Default")))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var insertGenre = new DynamicParameters();
                        insertGenre.Add("@Term", name);
                        insertGenre.Add("@Id", id);

                        var exec = connection.Query<int>("dbo.spUpdateGenreById", insertGenre, transaction, commandType: CommandType.StoredProcedure);
                        var result = exec.AsList();
                        var success = (result.Count == 0) ? true : false;
                        transaction.Commit();
                        return success;

                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 2601 || ex.Number == 2627)
                        {
                            return true;
                        }
                        else
                        {
                            _logger.LogError($"Error saving genre to database: {ex.Message}");
                            _logger.LogError($"SQL Error number is: {ex.Number}");
                            _logger.LogError($"Stacktrace: {ex.StackTrace}");
                            transaction.Rollback();
                            return false;
                        }
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Method to insert a Genre record in the database. The genre does not yet exists in the database.
        /// </summary>
        /// <param name="name">String, representing the Term field for the Genre</param>
        /// <returns>Int, the Id for the new record stored in the database</returns>
        public int InsertGenre(string name)
        {
            using (IDbConnection connection = new SqlConnection(CnnVal("Default")))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var insertGenre = new DynamicParameters();
                        insertGenre.Add("@Term", name);
                        insertGenre.Add(
                            name: "@Retval",
                            dbType: DbType.Int32,
                            direction: ParameterDirection.ReturnValue);

                        var exec = connection.Query<int>("dbo.spInsertGenres", insertGenre, transaction, commandType: CommandType.StoredProcedure);
                        int result = insertGenre.Get<int>("@Retval");
                        int success = result > 0 ? result : 0;
                        transaction.Commit();
                        return success;

                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 2601 || ex.Number == 2627)
                        {
                            var insertGenre = new DynamicParameters();
                            insertGenre.Add("@Term", name);
                            insertGenre.Add(
                                name: "@Retval",
                                dbType: DbType.Int32,
                                direction: ParameterDirection.ReturnValue);

                            var exec = connection.Query<int>("dbo.spUndeleteGenreByName", insertGenre, transaction, commandType: CommandType.StoredProcedure);
                            int result = insertGenre.Get<int>("@Retval");
                            int success = result > 0 ? result : 1;
                            transaction.Commit();
                            return success;
                        }
                        else
                        {
                            _logger.LogError($"Error storing genre in the database: {ex.Message}");
                            _logger.LogError($"SQL Error number is: {ex.Number}");
                            _logger.LogError($"Stacktrace: {ex.StackTrace}");
                            transaction.Rollback();
                            return 0;
                        }
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Method to store a Language to Comic Association in the database
        /// </summary>
        /// <param name="comicId">Int, representing the Id field of the Comic with which the language is to be associated.</param>
        /// <param name="languageId">Int, representing the Id field of the Language with which the comic is to be associated.</param>
        /// <returns>Boolean, true if storage was usccessfull and false if something went wrong.</returns>
        public bool SaveLanguageAssoc(int comicId, int languageId)
        {
            using (IDbConnection connection = new SqlConnection(CnnVal("Default")))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var insertAssoc = new DynamicParameters();
                        insertAssoc.Add("@Id", comicId);
                        insertAssoc.Add("@LanguageId", languageId);

                        var exec = connection.Query<int>("dbo.spUpdateComicToLanguageById", insertAssoc, transaction, commandType: CommandType.StoredProcedure);
                        var result = exec.AsList();
                        var success = (result.Count == 0) ? true : false;
                        transaction.Commit();
                        return success;

                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 2601 || ex.Number == 2627)
                        {
                            return true;
                        }
                        else
                        {
                            _logger.LogError($"Error updating language in database: {ex.Message}");
                            _logger.LogError($"SQL Error number is: {ex.Number}");
                            _logger.LogError($"Stacktrace: {ex.StackTrace}");
                            transaction.Rollback();
                            return false;
                        }
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Method to update a Language record in the database. The language exists in the database.
        /// </summary>
        /// <param name="id">Int, representing the Id field of the Comic with which the language is to be associated.</param>
        /// <param name="name">String, representing the Term field of the Language with which the comic is to be associated.</param>
        /// <returns>Boolean, true if storage was usccessfull and false if something went wrong.</returns>
        public bool UpdateLanguage(int id, string name)
        {
            using (IDbConnection connection = new SqlConnection(CnnVal("Default")))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var insertLanguage = new DynamicParameters();
                        insertLanguage.Add("@Term", name);
                        insertLanguage.Add("@Id", id);

                        var exec = connection.Query<int>("dbo.spUpdateLanguageById", insertLanguage, transaction, commandType: CommandType.StoredProcedure);
                        var result = exec.AsList();
                        var success = (result.Count == 0) ? true : false;
                        transaction.Commit();
                        return success;

                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 2601 || ex.Number == 2627)
                        {
                            return true;
                        }
                        else
                        {
                            _logger.LogError($"Error saving language association to database: {ex.Message}");
                            _logger.LogError($"SQL Error number is: {ex.Number}");
                            _logger.LogError($"Stacktrace: {ex.StackTrace}");
                            transaction.Rollback();
                            return false;
                        }
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Method to insert a Language record in the database. The language does not yet exists in the database.
        /// </summary>
        /// <param name="name">String, representing the Term field for the Language</param>
        /// <returns>Int, the Id for the new record stored in the database</returns>
        public int InsertLanguage(string name)
        {
            using (IDbConnection connection = new SqlConnection(CnnVal("Default")))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var insertLanguage = new DynamicParameters();
                        insertLanguage.Add("@Term", name);
                        insertLanguage.Add(
                            name: "@Retval",
                            dbType: DbType.Int32,
                            direction: ParameterDirection.ReturnValue);

                        var exec = connection.Query<int>("dbo.spInsertLanguages", insertLanguage, transaction, commandType: CommandType.StoredProcedure);
                        int result = insertLanguage.Get<int>("@Retval");
                        int success = result > 0 ? result : 0;
                        transaction.Commit();
                        return success;

                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 2601 || ex.Number == 2627)
                        {
                            var insertLanguage = new DynamicParameters();
                            insertLanguage.Add("@Term", name);
                            insertLanguage.Add(
                                name: "@Retval",
                                dbType: DbType.Int32,
                                direction: ParameterDirection.ReturnValue);

                            var exec = connection.Query<int>("dbo.spUndeleteLanguageByName", insertLanguage, transaction, commandType: CommandType.StoredProcedure);
                            int result = insertLanguage.Get<int>("@Retval");
                            int success = result > 0 ? result : 1;
                            transaction.Commit();
                            return success;
                        }
                        else
                        {
                            _logger.LogError($"Error storing language in the database: {ex.Message}");
                            _logger.LogError($"SQL Error number is: {ex.Number}");
                            _logger.LogError($"Stacktrace: {ex.StackTrace}");
                            transaction.Rollback();
                            return 0;
                        }
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Method to store a Type to Comic Association in the database
        /// </summary>
        /// <param name="comicId">Int, representing the Id field of the Comic with which the type is to be associated.</param>
        /// <param name="typeId">Int, representing the Id field of the Type with which the comic is to be associated.</param>
        /// <returns>Boolean, true if storage was usccessfull and false if something went wrong.</returns>
        public bool SaveTypeAssoc(int comicId, int typeId)
        {
            using (IDbConnection connection = new SqlConnection(CnnVal("Default")))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var insertAssoc = new DynamicParameters();
                        insertAssoc.Add("@Id", comicId);
                        insertAssoc.Add("@TypeId", typeId);

                        var exec = connection.Query<int>("dbo.spUpdateComicToTypeById", insertAssoc, transaction, commandType: CommandType.StoredProcedure);
                        var result = exec.AsList();
                        var success = (result.Count == 0) ? true : false;
                        transaction.Commit();
                        return success;

                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 2601 || ex.Number == 2627)
                        {
                            return true;
                        }
                        else
                        {
                            _logger.LogError($"Error updating type in database: {ex.Message}");
                            _logger.LogError($"SQL Error number is: {ex.Number}");
                            _logger.LogError($"Stacktrace: {ex.StackTrace}");
                            transaction.Rollback();
                            return false;
                        }
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Method to update a Type record in the database. The type exists in the database.
        /// </summary>
        /// <param name="id">Int, representing the Id field of the Comic with which the type is to be associated.</param>
        /// <param name="name">String, representing the Term field of the Type with which the comic is to be associated.</param>
        /// <returns>Boolean, true if storage was usccessfull and false if something went wrong.</returns>
        public bool UpdateType(int id, string name)
        {
            using (IDbConnection connection = new SqlConnection(CnnVal("Default")))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var insertType = new DynamicParameters();
                        insertType.Add("@Term", name);
                        insertType.Add("@Id", id);

                        var exec = connection.Query<int>("dbo.spUpdateTypeById", insertType, transaction, commandType: CommandType.StoredProcedure);
                        var result = exec.AsList();
                        var success = (result.Count == 0) ? true : false;
                        transaction.Commit();
                        return success;

                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 2601 || ex.Number == 2627)
                        {
                            return true;
                        }
                        else
                        {
                            _logger.LogError($"Error saving type association to database: {ex.Message}");
                            _logger.LogError($"SQL Error number is: {ex.Number}");
                            _logger.LogError($"Stacktrace: {ex.StackTrace}");
                            transaction.Rollback();
                            return false;
                        }
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Method to insert a Type record in the database. The type does not yet exists in the database.
        /// </summary>
        /// <param name="name">String, representing the Term field for the Type</param>
        /// <returns>Int, the Id for the new record stored in the database</returns>
        public int InsertType(string name)
        {
            using (IDbConnection connection = new SqlConnection(CnnVal("Default")))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var insertType = new DynamicParameters();
                        insertType.Add("@Term", name);
                        insertType.Add(
                            name: "@Retval",
                            dbType: DbType.Int32,
                            direction: ParameterDirection.ReturnValue);

                        var exec = connection.Query<int>("dbo.spInsertTypes", insertType, transaction, commandType: CommandType.StoredProcedure);
                        int result = insertType.Get<int>("@Retval");
                        int success = result > 0 ? result : 0;
                        transaction.Commit();
                        return success;

                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 2601 || ex.Number == 2627)
                        {
                            var insertType = new DynamicParameters();
                            insertType.Add("@Term", name);
                            insertType.Add(
                                name: "@Retval",
                                dbType: DbType.Int32,
                                direction: ParameterDirection.ReturnValue);

                            var exec = connection.Query<int>("dbo.spUndeleteTypeByName", insertType, transaction, commandType: CommandType.StoredProcedure);
                            int result = insertType.Get<int>("@Retval");
                            int success = result > 0 ? result : 1;
                            transaction.Commit();
                            return success;
                        }
                        else
                        {
                            _logger.LogError($"Error storing type in the database: {ex.Message}");
                            _logger.LogError($"SQL Error number is: {ex.Number}");
                            _logger.LogError($"Stacktrace: {ex.StackTrace}");
                            transaction.Rollback();
                            return 0;
                        }
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Method to store a Serie to Comic Association in the database
        /// </summary>
        /// <param name="comicId">Int, representing the Id field of the Comic with which the serie is to be associated.</param>
        /// <param name="serieId">Int, representing the Id field of the Serie with which the comic is to be associated.</param>
        /// <returns>Boolean, true if storage was usccessfull and false if something went wrong.</returns>
        public bool SaveSerieAssoc(int comicId, int serieId)
        {
            using (IDbConnection connection = new SqlConnection(CnnVal("Default")))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var insertAssoc = new DynamicParameters();
                        insertAssoc.Add("@Id", comicId);
                        insertAssoc.Add("@SerieId", serieId);

                        var exec = connection.Query<int>("dbo.spUpdateComicToSerieById", insertAssoc, transaction, commandType: CommandType.StoredProcedure);
                        var result = exec.AsList();
                        var success = (result.Count == 0) ? true : false;
                        transaction.Commit();
                        return success;

                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 2601 || ex.Number == 2627)
                        {
                            return true;
                        }
                        else
                        {
                            _logger.LogError($"Error updating serie in database: {ex.Message}");
                            _logger.LogError($"SQL Error number is: {ex.Number}");
                            _logger.LogError($"Stacktrace: {ex.StackTrace}");
                            transaction.Rollback();
                            return false;
                        }
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Method to update a Serie record in the database. The serie exists in the database.
        /// </summary>
        /// <param name="id">Int, representing the Id field of the Comic with which the language is to be associated.</param>
        /// <param name="title">String, representing the Term field of the Language with which the comic is to be associated.</param>
        /// <param name="serieStart">DateTime, representing the SeriesStart field of the Serie with which the comic is to be associated.</param>
        /// <param name="serieEnd">DateTime, representing the SeriesEnd field of the Serie with which the comic is to be associated.</param>
        /// <returns>Boolean, true if storage was usccessfull and false if something went wrong.</returns>
        public bool UpdateSerie(int id, string title, DateTime serieStart, DateTime serieEnd)
        {
            using (IDbConnection connection = new SqlConnection(CnnVal("Default")))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var insertSerie = new DynamicParameters();
                        insertSerie.Add("@Id", id);
                        insertSerie.Add("@Title", title);
                        insertSerie.Add("@DateStart", serieStart);
                        insertSerie.Add("@DateEnd", serieEnd);

                        var exec = connection.Query<int>("dbo.spUpdateSerieById", insertSerie, transaction, commandType: CommandType.StoredProcedure);
                        var result = exec.AsList();
                        var success = (result.Count == 0) ? true : false;
                        transaction.Commit();
                        return success;

                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 2601 || ex.Number == 2627)
                        {
                            return true;
                        }
                        else
                        {
                            _logger.LogError($"Error saving serie to database: {ex.Message}");
                            _logger.LogError($"SQL Error number is: {ex.Number}");
                            _logger.LogError($"Stacktrace: {ex.StackTrace}");
                            transaction.Rollback();
                            return false;
                        }
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Method to insert a Serie record in the database. The serie does not yet exists in the database.
        /// </summary>
        /// <param name="title">String, representing the Name field of the Serie with which the comic is to be associated.</param>
        /// <param name="serieStart">DateTime, representing the SeriesStart field of the Serie with which the comic is to be associated.</param>
        /// <param name="serieEnd">DateTime, representing the SeriesEnd field of the Serie with which the comic is to be associated.</param>
        /// <returns>Int, the Id for the new record stored in the database</returns>
        public int InsertSerie(string title, DateTime serieStart, DateTime serieEnd)
        {
            using (IDbConnection connection = new SqlConnection(CnnVal("Default")))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var insertSerie = new DynamicParameters();
                        insertSerie.Add("@Title", title);
                        insertSerie.Add("@DateStart", serieStart);
                        insertSerie.Add("@DateEnd", serieEnd);
                        insertSerie.Add(
                            name: "@Retval",
                            dbType: DbType.Int32,
                            direction: ParameterDirection.ReturnValue);

                        var exec = connection.Query<int>("dbo.spInsertSeries", insertSerie, transaction, commandType: CommandType.StoredProcedure);
                        int result = insertSerie.Get<int>("@Retval");
                        int success = result > 0 ? result : 0;
                        transaction.Commit();
                        return success;

                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 2601 || ex.Number == 2627)
                        {
                            var insertSerie = new DynamicParameters();
                            insertSerie.Add("@Title", title);
                            insertSerie.Add(
                                name: "@Retval",
                                dbType: DbType.Int32,
                                direction: ParameterDirection.ReturnValue);

                            var exec = connection.Query<int>("dbo.spUndeleteSerieByName", insertSerie, transaction, commandType: CommandType.StoredProcedure);
                            int result = insertSerie.Get<int>("@Retval");
                            int success = result > 0 ? result : 1;
                            transaction.Commit();
                            return success;
                        }
                        else
                        {
                            _logger.LogError($"Error storing serie in the database: {ex.Message}");
                            _logger.LogError($"SQL Error number is: {ex.Number}");
                            _logger.LogError($"Stacktrace: {ex.StackTrace}");
                            transaction.Rollback();
                            return 0;
                        }
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Method to store a SubSerie to Comic Association in the database
        /// </summary>
        /// <param name="comicId">Int, representing the Id field of the Comic with which the subserie is to be associated.</param>
        /// <param name="subSerieId">Int, representing the Id field of the SubSerie with which the comic is to be associated.</param>
        /// <returns>Boolean, true if storage was usccessfull and false if something went wrong.</returns>
        public bool SaveSubSerieAssoc(int comicId, int subSerieId)
        {
            using (IDbConnection connection = new SqlConnection(CnnVal("Default")))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var insertAssoc = new DynamicParameters();
                        insertAssoc.Add("@Id", comicId);
                        insertAssoc.Add("@SubSerieId", subSerieId);

                        var exec = connection.Query<int>("dbo.spUpdateComicToSubSerieById", insertAssoc, transaction, commandType: CommandType.StoredProcedure);
                        var result = exec.AsList();
                        var success = (result.Count == 0) ? true : false;
                        transaction.Commit();
                        return success;

                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 2601 || ex.Number == 2627)
                        {
                            return true;
                        }
                        else
                        {
                            _logger.LogError($"Error updating subserie in database: {ex.Message}");
                            _logger.LogError($"SQL Error number is: {ex.Number}");
                            _logger.LogError($"Stacktrace: {ex.StackTrace}");
                            transaction.Rollback();
                            return false;
                        }
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Method to update a SubSerie record in the database. The subserie exists in the database.
        /// </summary>
        /// <param name="id">Int, representing the Id field of the Comic with which the subserie is to be associated.</param>
        /// <param name="title">String, representing the Term field of the SubSerie with which the comic is to be associated.</param>
        /// <param name="serieStart">DateTime, representing the SeriesStart field of the Serie with which the comic is to be associated.</param>
        /// <param name="serieEnd">DateTime, representing the SeriesEnd field of the Serie with which the comic is to be associated.</param>
        /// <param name="serieId">Int, representing the SeriesId field of the SubSerie with which the comic is to be associated.</param>
        /// <returns>Boolean, true if storage was usccessfull and false if something went wrong.</returns>
        public bool UpdateSubSerie(int id, string title, DateTime serieStart, DateTime serieEnd, int serieId)
        {
            using (IDbConnection connection = new SqlConnection(CnnVal("Default")))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var insertSubSerie = new DynamicParameters();
                        insertSubSerie.Add("@Id", id);
                        insertSubSerie.Add("@Title", title);
                        insertSubSerie.Add("@DateStart", serieStart);
                        insertSubSerie.Add("@DateEnd", serieEnd);
                        insertSubSerie.Add("@SerieId", serieId);

                        var exec = connection.Query<int>("dbo.spUpdateSubSerieById", insertSubSerie, transaction, commandType: CommandType.StoredProcedure);
                        var result = exec.AsList();
                        var success = (result.Count == 0) ? true : false;
                        transaction.Commit();
                        return success;

                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 2601 || ex.Number == 2627)
                        {
                            return true;
                        }
                        else
                        {
                            _logger.LogError($"Error saving subserie to database: {ex.Message}");
                            _logger.LogError($"SQL Error number is: {ex.Number}");
                            _logger.LogError($"Stacktrace: {ex.StackTrace}");
                            transaction.Rollback();
                            return false;
                        }
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Method to insert a SubSerie record in the database. The subserie does not yet exists in the database.
        /// </summary>
        /// <param name="title">String, representing the Name field of the SubSerie with which the comic is to be associated.</param>
        /// <param name="serieStart">DateTime, representing the SeriesStart field of the SubSerie with which the comic is to be associated.</param>
        /// <param name="serieEnd">DateTime, representing the SeriesEnd field of the SubSerie with which the comic is to be associated.</param>
        /// <param name="serieId">Int, representing the SeriesId field of the SubSerie with which the comic is to be associated.</param>
        /// <returns>Int, the Id for the new record stored in the database</returns>
        public int InsertSubSerie(string title, DateTime serieStart, DateTime serieEnd, int serieId)
        {
            using (IDbConnection connection = new SqlConnection(CnnVal("Default")))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var insertSerie = new DynamicParameters();
                        insertSerie.Add("@Title", title);
                        insertSerie.Add("@DateStart", serieStart);
                        insertSerie.Add("@DateEnd", serieEnd);
                        insertSerie.Add("@SerieId", serieId);
                        insertSerie.Add(
                            name: "@Retval",
                            dbType: DbType.Int32,
                            direction: ParameterDirection.ReturnValue);

                        var exec = connection.Query<int>("dbo.spInsertSubSeries", insertSerie, transaction, commandType: CommandType.StoredProcedure);
                        int result = insertSerie.Get<int>("@Retval");
                        int success = result > 0 ? result : 0;
                        transaction.Commit();
                        return success;

                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 2601 || ex.Number == 2627)
                        {
                            var insertSerie = new DynamicParameters();
                            insertSerie.Add("@Title", title);
                            insertSerie.Add(
                                name: "@Retval",
                                dbType: DbType.Int32,
                                direction: ParameterDirection.ReturnValue);

                            var exec = connection.Query<int>("dbo.spUndeleteSerieByName", insertSerie, transaction, commandType: CommandType.StoredProcedure);
                            int result = insertSerie.Get<int>("@Retval");
                            int success = result > 0 ? result : 1;
                            transaction.Commit();
                            return success;
                        }
                        else
                        {
                            _logger.LogError($"Error storing serie in the database: {ex.Message}");
                            _logger.LogError($"SQL Error number is: {ex.Number}");
                            _logger.LogError($"Stacktrace: {ex.StackTrace}");
                            transaction.Rollback();
                            return 0;
                        }
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
        #endregion

        #region Deleting
        /// <summary>
        /// Method to Delete an Author record. The method removes the association. 
        /// If successful sets the status of the record to deleted.
        /// </summary>
        /// <param name="id">Int, the Id for the Author to be deleted</param>
        /// <returns>Boolean, returns true if the removal was successful, false if not</returns>
        public bool DeleteAuthor(int id)
        {
            using (IDbConnection connection = new SqlConnection(CnnVal("Default")))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var deleteAuthor = new DynamicParameters();
                        var removeComicAssoc = new DynamicParameters();
                        deleteAuthor.Add("@Id", id);
                        removeComicAssoc.Add("@AuthorId", id);
                        var comicremove = connection.Query<int>("dbo.spRemoveAuthorFromComicToAuthorByAuthorId", removeComicAssoc, transaction, commandType: CommandType.StoredProcedure);
                        var comicremoveresult = comicremove.AsList();
                        var comicsuccess = (comicremoveresult.Count == 0) ? true : false;
                        var authorremove = connection.Query<int>("dbo.spDeleteAuthorById", deleteAuthor, transaction, commandType: CommandType.StoredProcedure);
                        var authorremoveresult = authorremove.AsList();
                        var authorsuccess = (authorremoveresult.Count == 0) ? true : false;
                        var success = comicsuccess && authorsuccess;
                        if (success == true)
                        {
                            transaction.Commit();
                        }
                        else
                        {
                            _logger.LogError($"Error removing author from database: {id}");
                            if (comicsuccess == false)
                            { _logger.LogError($"Check ComicToAuthor table for comics with AuthorId: {id}"); }
                            else if (authorsuccess == false)
                            { _logger.LogError($"Check Author table for author with Id: {id}"); }
                            transaction.Rollback();
                        }
                        return success;

                    }
                    catch (SqlException ex)
                    {
                        _logger.LogError($"Error deleting author from database: {ex.Message}");
                        _logger.LogError($"SQL Error number is: {ex.Number}");
                        _logger.LogError($"Stacktrace: {ex.StackTrace}");
                        transaction.Rollback();
                        return false;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Method to Delete a Publisher record. The method removes the association. 
        /// If successful sets the status of the record to deleted.
        /// </summary>
        /// <param name="id">Int, the Id for the Publisher to be deleted</param>
        /// <returns>Boolean, returns true if the removal was successful, false if not</returns>
        public bool DeletePublisher(int id)
        {
            using (IDbConnection connection = new SqlConnection(CnnVal("Default")))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var deletePublisher = new DynamicParameters();
                        var removeComicAssoc = new DynamicParameters();
                        deletePublisher.Add("@Id", id);
                        removeComicAssoc.Add("@PublisherId", id);
                        var comicremove = connection.Query<int>("dbo.spRemovePublisherFromComicToPublisherByPublisherId", removeComicAssoc, transaction, commandType: CommandType.StoredProcedure);
                        var comicremoveresult = comicremove.AsList();
                        var comicsuccess = (comicremoveresult.Count == 0) ? true : false;
                        var publisherremove = connection.Query<int>("dbo.spDeletePublisherById", deletePublisher, transaction, commandType: CommandType.StoredProcedure);
                        var publisherremoveresult = publisherremove.AsList();
                        var publishersuccess = (publisherremoveresult.Count == 0) ? true : false;
                        var success = comicsuccess && publishersuccess;
                        if (success == true)
                        {
                            transaction.Commit();
                        }
                        else
                        {
                            _logger.LogError($"Error removing publisher from database: {id}");
                            if (comicsuccess == false)
                            { _logger.LogError($"Check ComicToPublisher table for comics with PublisherId: {id}"); }
                            else if (publishersuccess == false)
                            { _logger.LogError($"Check Publisher table for publisher with Id: {id}"); }
                            transaction.Rollback();
                        }
                        return success;

                    }
                    catch (SqlException ex)
                    {
                        _logger.LogError($"Error deleting publisher from database: {ex.Message}");
                        _logger.LogError($"SQL Error number is: {ex.Number}");
                        _logger.LogError($"Stacktrace: {ex.StackTrace}");
                        transaction.Rollback();
                        return false;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Method to Delete a Genre record. The method removes the association. 
        /// If successful sets the status of the record to deleted.
        /// </summary>
        /// <param name="id">Int, the Id for the Genre to be deleted</param>
        /// <returns>Boolean, returns true if the removal was successful, false if not</returns>
        public bool DeleteGenre(int id)
        {
            using (IDbConnection connection = new SqlConnection(CnnVal("Default")))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var deleteGenre = new DynamicParameters();
                        var removeComicAssoc = new DynamicParameters();
                        deleteGenre.Add("@Id", id);
                        removeComicAssoc.Add("@GenreId", id);
                        var comicremove = connection.Query<int>("dbo.spRemoveGenreFromComicToGenreByGenreId", removeComicAssoc, transaction, commandType: CommandType.StoredProcedure);
                        var comicremoveresult = comicremove.AsList();
                        var comicsuccess = (comicremoveresult.Count == 0) ? true : false;
                        var genreremove = connection.Query<int>("dbo.spDeleteGenreById", deleteGenre, transaction, commandType: CommandType.StoredProcedure);
                        var genreremoveresult = genreremove.AsList();
                        var genresuccess = (genreremoveresult.Count == 0) ? true : false;
                        var success = comicsuccess && genresuccess;
                        if (success == true)
                        {
                            transaction.Commit();
                        }
                        else
                        {
                            _logger.LogError($"Error removing genre from database: {id}");
                            if (comicsuccess == false)
                            { _logger.LogError($"Check ComicToGenre table for comics with GenreId: {id}"); }
                            else if (genresuccess == false)
                            { _logger.LogError($"Check Genres table for genres with Id: {id}"); }
                            transaction.Rollback();
                        }
                        return success;

                    }
                    catch (SqlException ex)
                    {
                        _logger.LogError($"Error deleting genre from database: {ex.Message}");
                        _logger.LogError($"SQL Error number is: {ex.Number}");
                        _logger.LogError($"Stacktrace: {ex.StackTrace}");
                        transaction.Rollback();
                        return false;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Method to Delete a Language record. The method removes the association. 
        /// If successful sets the status of the record to deleted.
        /// </summary>
        /// <param name="id">Int, the Id for the Language to be deleted</param>
        /// <returns>Boolean, returns true if the removal was successful, false if not</returns>
        public bool DeleteLanguage(int id)
        {
            using (IDbConnection connection = new SqlConnection(CnnVal("Default")))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var deleteLanguage = new DynamicParameters();
                        var removeComicAssoc = new DynamicParameters();
                        deleteLanguage.Add("@Id", id);
                        removeComicAssoc.Add("@LanguageId", id);
                        var comicremove = connection.Query<int>("dbo.spRemoveLanguageFromComicsByLanguageId", removeComicAssoc, transaction, commandType: CommandType.StoredProcedure);
                        var comicremoveresult = comicremove.AsList();
                        var comicsuccess = (comicremoveresult.Count == 0) ? true : false;
                        var languageremove = connection.Query<int>("dbo.spDeleteLanguageById", deleteLanguage, transaction, commandType: CommandType.StoredProcedure);
                        var languageremoveresult = languageremove.AsList();
                        var languagesuccess = (languageremoveresult.Count == 0) ? true : false;
                        var success = comicsuccess && languagesuccess;
                        if (success == true)
                        {
                            transaction.Commit();
                        }
                        else
                        {
                            _logger.LogError($"Error removing language from database: {id}");
                            if (comicsuccess == false)
                            { _logger.LogError($"Check comic table for comics with LanguageId: {id}"); }
                            else if (languagesuccess == false)
                            { _logger.LogError($"Check language table for language with Id: {id}"); }
                            transaction.Rollback();
                        }
                        return success;

                    }
                    catch (SqlException ex)
                    {
                        _logger.LogError($"Error deleting language from database: {ex.Message}");
                        _logger.LogError($"SQL Error number is: {ex.Number}");
                        _logger.LogError($"Stacktrace: {ex.StackTrace}");
                        transaction.Rollback();
                        return false;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Method to Delete a Type record. The method removes the association. 
        /// If successful sets the status of the record to deleted.
        /// </summary>
        /// <param name="id">Int, the Id for the Type to be deleted</param>
        /// <returns>Boolean, returns true if the removal was successful, false if not</returns>
        public bool DeleteType(int id)
        {
            using (IDbConnection connection = new SqlConnection(CnnVal("Default")))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var deleteType = new DynamicParameters();
                        var removeComicAssoc = new DynamicParameters();
                        deleteType.Add("@Id", id);
                        removeComicAssoc.Add("@TypeId", id);
                        var comicremove = connection.Query<int>("dbo.spRemoveTypeFromComicsByTypeId", removeComicAssoc, transaction, commandType: CommandType.StoredProcedure);
                        var comicremoveresult = comicremove.AsList();
                        var comicsuccess = (comicremoveresult.Count == 0) ? true : false;
                        var typeremove = connection.Query<int>("dbo.spDeleteTypeById", deleteType, transaction, commandType: CommandType.StoredProcedure);
                        var typeremoveresult = typeremove.AsList();
                        var typesuccess = (typeremoveresult.Count == 0) ? true : false;
                        var success = comicsuccess && typesuccess;
                        if (success == true)
                        {
                            transaction.Commit();
                        }
                        else
                        {
                            _logger.LogError($"Error removing type from database: {id}");
                            if (comicsuccess == false)
                            { _logger.LogError($"Check comic table for comics with TypeId: {id}"); }
                            else if (typesuccess == false)
                            { _logger.LogError($"Check type table for type with Id: {id}"); }
                            transaction.Rollback();
                        }
                        return success;

                    }
                    catch (SqlException ex)
                    {
                        _logger.LogError($"Error deleting type from database: {ex.Message}");
                        _logger.LogError($"SQL Error number is: {ex.Number}");
                        _logger.LogError($"Stacktrace: {ex.StackTrace}");
                        transaction.Rollback();
                        return false;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Method to Delete a Serie record. The method removes the association. 
        /// If successful sets the status of the record to deleted.
        /// </summary>
        /// <param name="id">Int, the Id for the Serie to be deleted</param>
        /// <returns>Boolean, returns true if the removal was successful, false if not</returns>
        public bool DeleteSerie(int id)
        {
            using (IDbConnection connection = new SqlConnection(CnnVal("Default")))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var deleteSerie = new DynamicParameters();
                        var removeComicAssoc = new DynamicParameters();
                        deleteSerie.Add("@Id", id);
                        removeComicAssoc.Add("@SerieId", id);
                        var comicremove = connection.Query<int>("dbo.spRemoveSerieFromComicBySerieId", removeComicAssoc, transaction, commandType: CommandType.StoredProcedure);
                        var comicremoveresult = comicremove.AsList();
                        var comicsuccess = (comicremoveresult.Count == 0) ? true : false;
                        var serieremove = connection.Query<int>("dbo.spDeleteSerieById", deleteSerie, transaction, commandType: CommandType.StoredProcedure);
                        var serieremoveresult = serieremove.AsList();
                        var seriesuccess = (serieremoveresult.Count == 0) ? true : false;
                        var success = comicsuccess && seriesuccess;
                        if (success == true)
                        {
                            transaction.Commit();
                        }
                        else
                        {
                            _logger.LogError($"Error removing serie from database: {id}");
                            if (comicsuccess == false)
                            { _logger.LogError($"Check comic table for comics with SeriesId: {id}"); }
                            else if (seriesuccess == false)
                            { _logger.LogError($"Check series table for serie with Id: {id}"); }
                            transaction.Rollback();
                        }
                        return success;

                    }
                    catch (SqlException ex)
                    {
                        _logger.LogError($"Error deleting serie from database: {ex.Message}");
                        _logger.LogError($"SQL Error number is: {ex.Number}");
                        _logger.LogError($"Stacktrace: {ex.StackTrace}");
                        transaction.Rollback();
                        return false;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Method to Delete a SubSerie record. The method removes the association. 
        /// If successful sets the status of the record to deleted.
        /// </summary>
        /// <param name="id">Int, the Id for the SubSerie to be deleted</param>
        /// <returns>Boolean, returns true if the removal was successful, false if not</returns>
        public bool DeleteSubSerie(int id)
        {
            using (IDbConnection connection = new SqlConnection(CnnVal("Default")))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var deleteSubSerie = new DynamicParameters();
                        var removeComicAssoc = new DynamicParameters();
                        deleteSubSerie.Add("@Id", id);
                        removeComicAssoc.Add("@SubSerieId", id);
                        var comicremove = connection.Query<int>("dbo.spRemoveSubSerieFromComicBySerieId", removeComicAssoc, transaction, commandType: CommandType.StoredProcedure);
                        var comicremoveresult = comicremove.AsList();
                        var comicsuccess = (comicremoveresult.Count == 0) ? true : false;
                        var subserieremove = connection.Query<int>("dbo.spDeleteSubSerieById", deleteSubSerie, transaction, commandType: CommandType.StoredProcedure);
                        var subserieremoveresult = subserieremove.AsList();
                        var subseriesuccess = (subserieremoveresult.Count == 0) ? true : false;
                        var success = comicsuccess && subseriesuccess;
                        if (success == true)
                        {
                            transaction.Commit();
                        }
                        else
                        {
                            _logger.LogError($"Error removing subserie from database: {id}");
                            if (comicsuccess == false)
                            { _logger.LogError($"Check comic table for comics with SubSerieId: {id}"); }
                            else if (subseriesuccess == false)
                            { _logger.LogError($"Check SubSeries table for SubSerie with Id: {id}"); }
                            transaction.Rollback();
                        }
                        return success;

                    }
                    catch (SqlException ex)
                    {
                        _logger.LogError($"Error deleting subserie from database: {ex.Message}");
                        _logger.LogError($"SQL Error number is: {ex.Number}");
                        _logger.LogError($"Stacktrace: {ex.StackTrace}");
                        transaction.Rollback();
                        return false;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
        #endregion
    }
}
