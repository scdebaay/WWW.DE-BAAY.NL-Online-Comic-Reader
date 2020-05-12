using ComicReaderClassLibrary.DataAccess.Implementations;
using ComicReaderClassLibrary.DataAccess.DataModels;
using ComicReaderClassLibrary.Models;
using ComicReaderClassLibrary.Resources;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ComicReaderClassLibrary.Resources
{
    public static class IComicReaderServices
    {
        public static IServiceCollection AddComicReaderLibrary(this IServiceCollection services)
        {
            services.AddScoped<ISqlApiDbConnection, SqlApiDbConnection>();
            services.AddScoped<IFetchedComic, FetchedComic>();
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


