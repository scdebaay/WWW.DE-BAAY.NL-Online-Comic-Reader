using ComicReaderClassLibrary.DataAccess.Implementations;
using ComicReaderClassLibrary.Models;
using ComicReaderClassLibrary.Resources;
using ComicReaderInventoryService.AbstractClasses;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ComicReaderInventoryService.Jobs
{
    class IngestComicJob : CronJobService
    {
        private readonly ILogger<IngestComicJob> _logger;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly IServiceProvider _serviceProvider;
        private readonly IFileSystem _fileSystem;

        /// <summary>
        /// Using the example at https://codeburst.io/schedule-cron-jobs-using-hostedservice-in-asp-net-core-e17c47ba06 implemented the following
        /// Derived background task from WebHost instead of Windows Service Host. 
        /// Using Cronos subclassed the IHost model and use the CronJobService as a base class to be able to cron schedule the job.
        /// Jobs are scheduled in Startup class.
        /// When jobs are started, a scope is built into which Scope classes are injected, like RootModel, FolderCrawler and Db context.
        /// The folder to crawl is resolved and when not encountered, the service throws an exception and is stopped.
        /// ToDo: Implement http endpoint to activate Job out of Band.
        /// </summary>
        public IngestComicJob(IScheduleConfig<IngestComicJob> config, IHostApplicationLifetime hostApplicationLifetime, IServiceProvider serviceProvider, ILogger<IngestComicJob> logger, IFileSystem fileSystem)
            : base(config.CronExpression, config.TimeZoneInfo)
        {
            _hostApplicationLifetime = hostApplicationLifetime;
            _fileSystem = fileSystem;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Ingest Comic Job scheduled.");
            _logger.LogInformation($"Next run at {base.GetNextOccurrence().Value:dd-MM-yy HH:mm:ss}.");
            return base.StartAsync(cancellationToken);
        }

        public override Task DoWork(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Ingest Comic Job is working at {DateTime.Now:HH:mm:ss}.");
                using (var scope = _serviceProvider.CreateScope())
                {
                    IConfiguration _configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                    ISqlIngestDbConnection _databaseConnection = scope.ServiceProvider.GetRequiredService<ISqlIngestDbConnection>();
                    IRootModel _filesToIngest = scope.ServiceProvider.GetRequiredService<IRootModel>();
                    IFolderCrawler _folderCrawler = scope.ServiceProvider.GetRequiredService<IFolderCrawler>();
                    var _folderToScan = _fileSystem.DirectoryInfo.FromDirectoryName(_configuration["foldersToScan:0:comicFolder"]);
                    if (Directory.Exists(_folderToScan.FullName) == false)
                    {
                        _logger.LogError($"Folder to scan does not exist: {_folderToScan.FullName}. See settings.json, foldersToScan:0:comicFolder");
                        throw new DirectoryNotFoundException();
                    }
                    _logger.LogInformation($"Job started {DateTime.Now:HH:mm:ss}");
                    _filesToIngest = _folderCrawler.GetDirectory(_folderToScan);
                    _logger.LogInformation($"Returned from crawling at: {DateTimeOffset.Now:HH:mm:ss}");
                    _databaseConnection.InsertComics(_filesToIngest.folder.file);
                    _logger.LogInformation($"Number of comics found: {_filesToIngest.folder.file.Count()}");
                    _logger.LogInformation($"Sleeping untill {base.GetNextOccurrence().Value:dd-MM-yy HH:mm:ss}.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Global exception occurred. service will stop. The Exception was { ex.Message }");
                _hostApplicationLifetime.StopApplication();
            }
            return Task.CompletedTask;
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Ingest Comic Job is stopping.");
            return base.StopAsync(cancellationToken);
        }
    }
}
