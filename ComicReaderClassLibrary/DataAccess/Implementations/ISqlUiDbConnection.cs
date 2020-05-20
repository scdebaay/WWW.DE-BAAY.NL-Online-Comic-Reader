using ComicReaderClassLibrary.DataAccess.DataModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComicReaderClassLibrary.DataAccess.Implementations
{
    /// <summary>
    /// UI Db Context for database related operations
    /// </summary>
    public interface ISqlUiDbConnection
    {
        #region Retrieval
        
        /// <summary>
        /// Async method to retrieve a list of ComicDataModels from the database
        /// </summary>
        /// <returns>Task of List of ComicDataModels representing all comics in the database</returns>
        public Task<List<ComicDataModel>> RetrieveComicsAsync();

        /// <summary>
        /// Async method to retrieve a list of AuthorDataModels from the database for one specific Comic
        /// </summary>
        /// <param name="id">Int, representing the Id of the Comic for which to retrieve all Authors</param>
        /// <returns>Task of List of AuthorDataModels</returns>
        public Task<List<AuthorDataModel>> RetrieveAuthorByComicIdAsync(int id);

        /// <summary>
        /// Async method to retrieve a list of GenreDataModels from the database for one specific Comic
        /// </summary>
        /// <param name="id">Int, representing the Id of the Comic for which to retrieve all Genres</param>
        /// <returns>Task of List of GenreDataModels</returns>
        public Task<List<GenreDataModel>> RetrieveGenreByComicIdAsync(int id);

        /// <summary>
        /// Async method to retrieve a list of PublisherDataModels from the database for one specific Comic
        /// </summary>
        /// <param name="id">Int, representing the Id of the Comic for which to retrieve all Publishers</param>
        /// <returns>Task of List of PublisherDataModels</returns>
        public Task<List<PublisherDataModel>> RetrievePublisherByComicIdAsync(int id);

        /// <summary>
        /// Async method to retrieve a list of LanguageDataModels from the database for one specific Comic
        /// </summary>        
        /// <returns>Task of List of LanguageDataModels, normally one language is associated with one comic</returns>
        public Task<List<LanguageDataModel>> RetrieveLanguageAsync();

        /// <summary>
        /// Async method to retrieve a list of TypeDataModels from the database for one specific Comic
        /// </summary>        
        /// <returns>Task of List of TypeDataModels, normally one type is associated with one comic</returns>
        public Task<List<TypeDataModel>> RetrieveTypeAsync();

        /// <summary>
        /// Async method to retrieve a list of SerieDataModels from the database for one specific Comic
        /// </summary>        
        /// <returns>Task of List of SerieDataModels, normally one serie is associated with multiple comics or subseries</returns>
        public Task<List<SeriesDataModel>> RetrieveSerieAsync();

        /// <summary>
        /// Async method to retrieve a list of SubSerieDataModels from the database for one specific Comic
        /// </summary>        
        /// <returns>Task of List of SubSerieDataModels, normally one subserie is associated with multiple comics</returns>
        public Task<List<SubSeriesDataModel>> RetrieveSubSerieAsync();

        /// <summary>
        /// Async method to retrieve a list of all PublisherDataModels from the database.
        /// </summary>        
        /// <returns>Task of List of PublisherDataModels</returns>
        public Task<List<PublisherDataModel>> RetrievePublisherAsync();

        /// <summary>
        /// Async method to retrieve a list of all ComicDataModels from the database for a specific Publisher.
        /// </summary>
        /// <param name="id">Int, representing the Id of the Publisher for which to retrieve all Comics</param>
        /// <returns>Task of List of ints, representing the Id's of the Comics pertaining to a Publisher</returns>
        public Task<List<int>> RetrieveComicsByPublisherIdAsync(int Id);

        /// <summary>
        /// Async method to retrieve a list of all GenreDataModels from the database.
        /// </summary>        
        /// <returns>Task of List of GenreDataModels</returns>
        public Task<List<GenreDataModel>> RetrieveGenreAsync();

        /// <summary>
        /// Async method to retrieve a list of all ComicDataModels from the database for a specific Genre.
        /// </summary>
        /// <param name="id">Int, representing the Id of the Genre for which to retrieve all Comics</param>
        /// <returns>Task of List of ints, representing the Id's of the Comics pertaining to a Genre</returns>
        public Task<List<int>> RetrieveComicsByGenreIdAsync(int Id);

        /// <summary>
        /// Async method to retrieve a list of all AuthorDataModels from the database.
        /// </summary>        
        /// <returns>Task of List of AuthorDataModels</returns>
        public Task<List<AuthorDataModel>> RetrieveAuthorAsync();

        /// <summary>
        /// Async method to retrieve a list of all ComicDataModels from the database for a specific Author.
        /// </summary>
        /// <param name="id">Int, representing the Id of the Author for which to retrieve all Comics</param>
        /// <returns>Task of List of ints, representing the Id's of the Comics pertaining to an Author</returns>
        public Task<List<int>> RetrieveComicsByAuthorIdAsync(int Id);
        #endregion

        #region updating
        /// <summary>
        /// Method to store a ComicDataModel in the database
        /// </summary>
        /// <param name="comic">ComicDataModel, representing the Comic to be stored int he database.</param>
        /// <returns>Boolean, true if storage was usccessfull and false if something went wrong.</returns>
        public bool SaveComic(ComicDataModel comic);

        /// <summary>
        /// Method to store a Author to Comic Association in the database
        /// </summary>
        /// <param name="comicAuthorAssoc">List of ComicToAuthorModels, representing the Comic to Author Association to store in the database.</param>
        /// <returns>Boolean, true if storage was usccessfull and false if something went wrong.</returns>
        public bool SaveAuthorAssoc(List<ComicToAuthorsDataModel> comicAuthorAssoc);

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
        public bool UpdateAuthor(int id, string firstname, string middlename, string lastname, DateTime datebirth, DateTime datedeceased, bool active);

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
        public int InsertAuthor(string firstname, string middlename, string lastname, DateTime datebirth, DateTime datedeceased, bool active);

        /// <summary>
        /// Method to store a Publisher to Comic Association in the database
        /// </summary>
        /// <param name="comicPublisherAssoc">List of ComicToPublisherModels, representing the Comic to Publisher Association to store in the database.</param>
        /// <returns>Boolean, true if storage was usccessfull and false if something went wrong.</returns>
        public bool SavePublisherAssoc(List<ComicToPublishersDataModel> comicPublisherAssoc);

        /// <summary>
        /// Method to update an Publisher record in the database. The publisher exists in the database.
        /// </summary>
        /// <param name="id">Int, representing the Id of the Publisher to store.</param>
        /// <param name="name">String, representing the Name field for the Publisher</param>
        /// <returns>Boolean, true if storage was usccessfull and false if something went wrong.</returns>
        public bool UpdatePublisher(int id, string name);

        /// <summary>
        /// Method to insert a Publisher record in the database. The publisher does not yet exists in the database.
        /// </summary>
        /// <param name="name">String, representing the Name field for the Publisher</param>
        /// <returns>Int, the Id for the new record stored in the database</returns>
        public int InsertPublisher(string name);

        /// <summary>
        /// Method to store a Genre to Comic Association in the database
        /// </summary>
        /// <param name="comicGenreAssoc">List of ComicToGenreModels, representing the Comic to Genre Association to store in the database.</param>
        /// <returns>Boolean, true if storage was usccessfull and false if something went wrong.</returns>
        public bool SaveGenreAssoc(List<ComicToGenreDataModel> comicGenreAssoc);

        /// <summary>
        /// Method to update an Genre record in the database. The genre exists in the database.
        /// </summary>
        /// <param name="id">Int, representing the Id of the Genre to store.</param>
        /// <param name="name">String, representing the Term field for the Genre</param>
        /// <returns>Boolean, true if storage was usccessfull and false if something went wrong.</returns>
        public bool UpdateGenre(int id, string name);

        /// <summary>
        /// Method to insert a Genre record in the database. The genre does not yet exists in the database.
        /// </summary>
        /// <param name="name">String, representing the Term field for the Genre</param>
        /// <returns>Int, the Id for the new record stored in the database</returns>
        public int InsertGenre(string name);

        /// <summary>
        /// Method to store a Language to Comic Association in the database
        /// </summary>
        /// <param name="comicId">Int, representing the Id field of the Comic with which the language is to be associated.</param>
        /// <param name="languageId">Int, representing the Id field of the Language with which the comic is to be associated.</param>
        /// <returns>Boolean, true if storage was usccessfull and false if something went wrong.</returns>
        public bool SaveLanguageAssoc(int comicId, int languageId);

        /// <summary>
        /// Method to insert a Language record in the database. The language does not yet exists in the database.
        /// </summary>
        /// <param name="name">String, representing the Term field for the Language</param>
        /// <returns>Int, the Id for the new record stored in the database</returns>
        public int InsertLanguage(string name);

        /// <summary>
        /// Method to update a Language record in the database. The language exists in the database.
        /// </summary>
        /// <param name="id">Int, representing the Id field of the Comic with which the language is to be associated.</param>
        /// <param name="name">String, representing the Term field of the Language with which the comic is to be associated.</param>
        /// <returns>Boolean, true if storage was usccessfull and false if something went wrong.</returns>
        public bool UpdateLanguage(int id, string name);

        /// <summary>
        /// Method to store a Type to Comic Association in the database
        /// </summary>
        /// <param name="comicId">Int, representing the Id field of the Comic with which the type is to be associated.</param>
        /// <param name="typeId">Int, representing the Id field of the Type with which the comic is to be associated.</param>
        /// <returns>Boolean, true if storage was usccessfull and false if something went wrong.</returns>
        public bool SaveTypeAssoc(int comicId, int TypeId);

        /// <summary>
        /// Method to insert a Type record in the database. The type does not yet exists in the database.
        /// </summary>
        /// <param name="name">String, representing the Term field for the Type</param>
        /// <returns>Int, the Id for the new record stored in the database</returns>
        public int InsertType(string name);

        /// <summary>
        /// Method to update a Type record in the database. The type exists in the database.
        /// </summary>
        /// <param name="id">Int, representing the Id field of the Comic with which the type is to be associated.</param>
        /// <param name="name">String, representing the Term field of the Type with which the comic is to be associated.</param>
        /// <returns>Boolean, true if storage was usccessfull and false if something went wrong.</returns>
        public bool UpdateType(int id, string name);

        /// <summary>
        /// Method to store a Serie to Comic Association in the database
        /// </summary>
        /// <param name="comicId">Int, representing the Id field of the Comic with which the serie is to be associated.</param>
        /// <param name="serieId">Int, representing the Id field of the Serie with which the comic is to be associated.</param>
        /// <returns>Boolean, true if storage was usccessfull and false if something went wrong.</returns>
        public bool SaveSerieAssoc(int comicId, int serieId);

        /// <summary>
        /// Method to update a Serie record in the database. The serie exists in the database.
        /// </summary>
        /// <param name="id">Int, representing the Id field of the Comic with which the language is to be associated.</param>
        /// <param name="title">String, representing the Term field of the Language with which the comic is to be associated.</param>
        /// <param name="serieStart">DateTime, representing the SeriesStart field of the Serie with which the comic is to be associated.</param>
        /// <param name="serieEnd">DateTime, representing the SeriesEnd field of the Serie with which the comic is to be associated.</param>
        /// <returns>Boolean, true if storage was usccessfull and false if something went wrong.</returns>
        public bool UpdateSerie(int id, string title, DateTime serieStart, DateTime serieEnd);

        /// <summary>
        /// Method to insert a Serie record in the database. The serie does not yet exists in the database.
        /// </summary>
        /// <param name="title">String, representing the Name field of the Serie with which the comic is to be associated.</param>
        /// <param name="serieStart">DateTime, representing the SeriesStart field of the Serie with which the comic is to be associated.</param>
        /// <param name="serieEnd">DateTime, representing the SeriesEnd field of the Serie with which the comic is to be associated.</param>
        /// <returns>Int, the Id for the new record stored in the database</returns>
        public int InsertSerie(string title, DateTime serieStart, DateTime serieEnd);

        /// <summary>
        /// Method to store a SubSerie to Comic Association in the database
        /// </summary>
        /// <param name="comicId">Int, representing the Id field of the Comic with which the subserie is to be associated.</param>
        /// <param name="subSerieId">Int, representing the Id field of the SubSerie with which the comic is to be associated.</param>
        /// <returns>Boolean, true if storage was usccessfull and false if something went wrong.</returns>
        public bool SaveSubSerieAssoc(int comicId, int subSerieId);

        /// <summary>
        /// Method to update a SubSerie record in the database. The subserie exists in the database.
        /// </summary>
        /// <param name="id">Int, representing the Id field of the Comic with which the subserie is to be associated.</param>
        /// <param name="title">String, representing the Term field of the SubSerie with which the comic is to be associated.</param>
        /// <param name="serieStart">DateTime, representing the SeriesStart field of the Serie with which the comic is to be associated.</param>
        /// <param name="serieEnd">DateTime, representing the SeriesEnd field of the Serie with which the comic is to be associated.</param>
        /// <param name="serieId">Int, representing the SeriesId field of the SubSerie with which the comic is to be associated.</param>
        /// <returns>Boolean, true if storage was usccessfull and false if something went wrong.</returns>
        public bool UpdateSubSerie(int id, string title, DateTime serieStart, DateTime serieEnd, int serieId);

        /// <summary>
        /// Method to insert a SubSerie record in the database. The subserie does not yet exists in the database.
        /// </summary>
        /// <param name="title">String, representing the Name field of the SubSerie with which the comic is to be associated.</param>
        /// <param name="serieStart">DateTime, representing the SeriesStart field of the SubSerie with which the comic is to be associated.</param>
        /// <param name="serieEnd">DateTime, representing the SeriesEnd field of the SubSerie with which the comic is to be associated.</param>
        /// <param name="serieId">Int, representing the SeriesId field of the SubSerie with which the comic is to be associated.</param>
        /// <returns>Int, the Id for the new record stored in the database</returns>
        public int InsertSubSerie(string title, DateTime serieStart, DateTime serieEnd, int serieId);
        #endregion

        #region deleting
        /// <summary>
        /// Method to Delete an Author record. The method removes the association. 
        /// If successful sets the status of the record to deleted.
        /// </summary>
        /// <param name="id">Int, the Id for the Author to be deleted</param>
        /// <returns>Boolean, returns true if the removal was successful, false if not</returns>
        public bool DeleteAuthor(int id);

        /// <summary>
        /// Method to Delete a Genre record. The method removes the association. 
        /// If successful sets the status of the record to deleted.
        /// </summary>
        /// <param name="id">Int, the Id for the Genre to be deleted</param>
        /// <returns>Boolean, returns true if the removal was successful, false if not</returns>
        public bool DeleteGenre(int id);

        /// <summary>
        /// Method to Delete a Publisher record. The method removes the association. 
        /// If successful sets the status of the record to deleted.
        /// </summary>
        /// <param name="id">Int, the Id for the Publisher to be deleted</param>
        /// <returns>Boolean, returns true if the removal was successful, false if not</returns>
        public bool DeletePublisher(int id);

        /// <summary>
        /// Method to Delete a Language record. The method removes the association. 
        /// If successful sets the status of the record to deleted.
        /// </summary>
        /// <param name="id">Int, the Id for the Language to be deleted</param>
        /// <returns>Boolean, returns true if the removal was successful, false if not</returns>
        public bool DeleteLanguage(int id);

        /// <summary>
        /// Method to Delete a Type record. The method removes the association. 
        /// If successful sets the status of the record to deleted.
        /// </summary>
        /// <param name="id">Int, the Id for the Type to be deleted</param>
        /// <returns>Boolean, returns true if the removal was successful, false if not</returns>
        public bool DeleteType(int id);

        /// <summary>
        /// Method to Delete a Serie record. The method removes the association. 
        /// If successful sets the status of the record to deleted.
        /// </summary>
        /// <param name="id">Int, the Id for the Serie to be deleted</param>
        /// <returns>Boolean, returns true if the removal was successful, false if not</returns>
        public bool DeleteSerie(int id);

        /// <summary>
        /// Method to Delete a SubSerie record. The method removes the association. 
        /// If successful sets the status of the record to deleted.
        /// </summary>
        /// <param name="id">Int, the Id for the SubSerie to be deleted</param>
        /// <returns>Boolean, returns true if the removal was successful, false if not</returns>
        public bool DeleteSubSerie(int id);
        #endregion
    }
}