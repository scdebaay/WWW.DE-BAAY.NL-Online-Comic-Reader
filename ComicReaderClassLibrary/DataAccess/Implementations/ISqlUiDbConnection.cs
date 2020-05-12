using ComicReaderClassLibrary.DataAccess.DataModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComicReaderClassLibrary.DataAccess.Implementations
{
    public interface ISqlUiDbConnection
    {
        #region Retrieval
        public Task<List<ComicDataModel>> RetrieveComicsAsync();
        public Task<List<AuthorDataModel>> RetrieveAuthorByComicIdAsync(int id);
        public Task<List<GenreDataModel>> RetrieveGenreByComicIdAsync(int id);
        public Task<List<PublisherDataModel>> RetrievePublisherByComicIdAsync(int id);
        public Task<List<LanguageDataModel>> RetrieveLanguageAsync();
        public Task<List<TypeDataModel>> RetrieveTypeAsync();
        public Task<List<SeriesDataModel>> RetrieveSerieAsync();
        public Task<List<SubSeriesDataModel>> RetrieveSubSerieAsync();
        public Task<List<PublisherDataModel>> RetrievePublisherAsync();
        public Task<List<int>> RetrieveComicsByPublisherIdAsync(int Id);
        public Task<List<GenreDataModel>> RetrieveGenreAsync();
        public Task<List<int>> RetrieveComicsByGenreIdAsync(int Id);
        public Task<List<AuthorDataModel>> RetrieveAuthorAsync();
        public Task<List<int>> RetrieveComicsByAuthorIdAsync(int Id);
        #endregion

        #region updating
        public bool SaveComic(ComicDataModel comic);
        public bool SaveAuthorAssoc(List<ComicToAuthorsDataModel> comicAuthorAssoc);
        public bool UpdateAuthor(int id, string firstname, string middlename, string lastname, DateTime datebirth, DateTime datedeceased, bool active);
        public int InsertAuthor(string firstname, string middlename, string lastname, DateTime datebirth, DateTime datedeceased, bool active);
        public bool SavePublisherAssoc(List<ComicToPublishersDataModel> comicPublisherAssoc);
        public bool UpdatePublisher(int id, string name);
        public int InsertPublisher(string name);
        public bool SaveGenreAssoc(List<ComicToGenreDataModel> comicGenreAssoc);
        public bool UpdateGenre(int id, string name);
        public int InsertGenre(string name);
        public bool SaveLanguageAssoc(int comicId, int languageId);
        public int InsertLanguage(string name);
        public bool UpdateLanguage(int id, string name);        
        public bool SaveTypeAssoc(int comicId, int TypeId);
        public int InsertType(string name);
        public bool UpdateType(int id, string name);
        public bool SaveSerieAssoc(int comicId, int serieId);
        public bool UpdateSerie(int id, string title, DateTime serieStart, DateTime serieEnd);
        public int InsertSerie(string title, DateTime serieStart, DateTime serieEnd);
        public bool SaveSubSerieAssoc(int comicId, int subSerieId);
        public bool UpdateSubSerie(int id, string title, DateTime serieStart, DateTime serieEnd, int serieId);
        public int InsertSubSerie(string title, DateTime serieStart, DateTime serieEnd, int serieId);
        #endregion

        #region deleting
        public bool DeleteAuthor(int id);
        public bool DeleteGenre(int id);
        public bool DeletePublisher(int id);
        public bool DeleteLanguage(int id);
        public bool DeleteType(int id);
        public bool DeleteSerie(int id);
        public bool DeleteSubSerie(int id);
        #endregion
    }
}