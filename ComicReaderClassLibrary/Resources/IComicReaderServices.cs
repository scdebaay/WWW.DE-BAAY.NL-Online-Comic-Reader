using ComicReaderClassLibrary.ComicEngine;
using ComicReaderClassLibrary.DataAccess.Implementations;
using ComicReaderClassLibrary.Models;
using Microsoft.Extensions.DependencyInjection;

namespace ComicReaderClassLibrary.Resources
{
    public static class IComicReaderServices
    {
        public static IServiceCollection AddComicReaderLibrary(this IServiceCollection services)
        {
            services.AddScoped<ISqlApiDbConnection, SqlApiDbConnection>();
            services.AddScoped<IFetchedComic, FetchedComic>();
            services.AddScoped<IComic, Comic>();
            services.AddScoped<IFetchedImage, FetchedImage>();
            services.AddScoped<IRootModel, RootModel>();
            services.AddScoped<IFolderModel, FolderModel>();
            services.AddScoped<IFileModel, FileModel>();
            services.AddScoped<IContentType, ContentType>();
            services.AddScoped<IPathResolver, PathResolver>();
            return services;
        }
    }
}


