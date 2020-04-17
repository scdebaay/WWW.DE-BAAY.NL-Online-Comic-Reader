using ComicReaderClassLibrary.Models;
using ComicReaderClassLibrary.Resources;
using ComicReaderClassLibrary.DataAccess.Implementations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;

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
            services.AddOpenApiDocument(config =>
            {
                config.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "Comic Reader API";
                    document.Info.Description = "API that serves up a list of comics that can be browsed using various methods.";
                    document.Info.TermsOfService = "None";
                    document.Info.Contact = new NSwag.OpenApiContact
                    {
                        Name = "Solino de Baay",
                        Email = "solino@de-baay.nl",
                        Url = "https://github.com/scdebaay/WWW.DE-BAAY.NL-Online-Comic-Reader"
                    };
                    //document.Info.License = new NSwag.OpenApiLicense
                    //{
                    //    Name = "Use under LICX",
                    //    Url = "https://example.com/license"
                    //};
                };
            });
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

            // Register the Swagger generator and the Swagger UI middlewares
            app.UseOpenApi();
            app.UseSwaggerUi3();
        }
    }
}
