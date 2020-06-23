using ComicReaderClassLibrary.ComicEngine;
using ComicReaderClassLibrary.DataAccess.Implementations;
using ComicReaderClassLibrary.Models;
using ComicReaderClassLibrary.Resources;
using ComicReaderInventoryService.AbstractClasses;
using ComicReaderInventoryService.Jobs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace ComicReaderInventoryService
{
    public class Startup
    {
        public IConfiguration Configuration { get; }


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddScoped<IRootModel, RootModel>();
            services.AddScoped<IComic, Comic>();
            services.AddScoped<IFolderCrawler, FolderCrawler>();
            services.AddSingleton<IFileSystem, FileSystem>();
            services.AddScoped<ISqlIngestDbConnection, SqlIngestDbConnection>();
            //Jobs are defined in Jobs.json. This file is read from location defined in Settings.Json.
            var jobConfigList = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(Configuration["JobsFile"]));
            //Jobs are scheduled using cron expression from JobConfiguration object configured in Jobs.Json.
            JArray jobArray = (JArray)jobConfigList["Jobs"];

            services.AddCronJob<IngestComicJob>(c =>
                            {
                                c.Name = (string)jobArray[1]["Name"];
                                c.Database = (string)jobArray[1]["Database"];
                                c.Folder = (string)jobArray[1]["Folder"];
                                c.TimeZoneInfo = TimeZoneInfo.Local;
                                c.CronExpression = (string)jobArray[1]["CronExpression"];
                            });
            services.AddCronJob<AltIngestComicJob>(c =>
                            {
                                c.Name = (string)jobArray[0]["Name"];
                                c.Database = (string)jobArray[0]["Database"];
                                c.Folder = (string)jobArray[0]["Folder"];
                                c.TimeZoneInfo = TimeZoneInfo.Local;
                                c.CronExpression = (string)jobArray[0]["CronExpression"];
                            });

            ///foreach (var job in jobArray.Children())
            ///{
            ///    services.AddCronJob<IngestComicJob>(c =>
            ///                {
            ///                    c.Name = (string)job["Name"];
            ///                    c.Database = (string)job["Database"];
            ///                    c.Folder = (string)job["Folder"];
            ///                    c.TimeZoneInfo = TimeZoneInfo.Local;
            ///                    c.CronExpression = (string)job["CronExpression"];
            ///                });
            ///}
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
