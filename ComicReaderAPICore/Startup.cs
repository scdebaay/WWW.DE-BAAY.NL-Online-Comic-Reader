using ComicReaderClassLibrary.Models;
using ComicReaderClassLibrary.Resources;
using ComicReaderClassLibrary.DataAccess.Implementations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ComicReaderAPICore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddScoped<IPathResolver, PathResolver>();
            services.AddScoped<IContentType, ContentType>();
            services.AddScoped<IFetchedImage, FetchedImage>();
            services.AddScoped<IFetchedComic, FetchedComic>();
            services.AddScoped<IRootModel, RootModel>();
            services.AddScoped<IFolderModel, FolderModel>();
            services.AddScoped<IFileModel, FileModel>();
            services.AddScoped<ISqlApiDbConnection, SqlApiDbConnection>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
