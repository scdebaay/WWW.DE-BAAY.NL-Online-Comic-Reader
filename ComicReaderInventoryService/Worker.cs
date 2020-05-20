using ComicReaderClassLibrary.DataAccess.Implementations;
using ComicReaderClassLibrary.Models;
using ComicReaderClassLibrary.Resources;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ComicReaderInventoryService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public Worker(IHostApplicationLifetime hostApplicationLifetime, IServiceScopeFactory serviceScopeFactory, ILogger<Worker> logger)
        {
            _hostApplicationLifetime = hostApplicationLifetime;
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken stoppingToken)
        {
            return base.StartAsync(stoppingToken);
        }

        /// <summary>
        /// When executing the task, a scope is created for running the task into which scope based service objects are injected.
        /// Configuration is parsed into specific job settings.
        /// If any of the foudn settings is invalid, the service exits.
        /// If all settings are valid, the jobs starts a crawling process to find new files on the specified path.
        /// On returning the database is checked for existing records
        /// New foudn files are ingested into the database.
        /// The job sleeps for configured hours.
        /// </summary>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                int _jobContinueDelay;
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    IConfiguration _configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                    ISqlIngestDbConnection _databaseConnection = scope.ServiceProvider.GetRequiredService<ISqlIngestDbConnection>();
                    IRootModel _filesToIngest = scope.ServiceProvider.GetRequiredService<IRootModel>();
                    IFolderCrawler _folderCrawler = scope.ServiceProvider.GetRequiredService<IFolderCrawler>();
                    DirectoryInfo _folderToScan = new DirectoryInfo(_configuration["foldersToScan:0:comicFolder"]);
                    int _jobStartDelay;
                    
                    if (Directory.Exists(_folderToScan.FullName) == false)
                    {
                        _logger.LogError($"Folder to scan does not exist: {_folderToScan.FullName}. See settings.json, foldersToScan:0:comicFolder");
                        _hostApplicationLifetime.StopApplication();
                    }
                    if (int.TryParse(_configuration["TimetoWaitBeforeStartingJob"], out _jobStartDelay) == false)
                    {
                        _logger.LogError($"Unable to parse delay from settings: {_configuration["TimetoWaitBeforeStartingJob"]}. See settings.json, TimetoWaitBeforeStartingJob");
                        _hostApplicationLifetime.StopApplication();
                    }
                    if (int.TryParse(_configuration["TimeToWaitInBetweenIngests"], out _jobContinueDelay) == false)
                    {
                        _logger.LogError($"Unable to parse delay from settings: {_configuration["TimeToWaitInBetweenIngests"]}. See settings.json, TimeToWaitInBetweenIngests");
                        _hostApplicationLifetime.StopApplication();
                    }
                    _logger.LogInformation($"Worker running at: {DateTime.Now}");
                    _logger.LogInformation($"Creating timer at: {DateTime.Now} with job delay {_jobStartDelay} seconds and sleep delay of {_jobContinueDelay} hours");
                    await Task.Delay(TimeSpan.FromSeconds(_jobStartDelay), stoppingToken);
                    _logger.LogInformation("Job started at: {time}", DateTimeOffset.Now);
                    _filesToIngest = await Task.Run(() => _folderCrawler.GetDirectory(_folderToScan));
                    _logger.LogInformation($"Returned from crawling at: {DateTimeOffset.Now}");
                    _databaseConnection.InsertComics(_filesToIngest.folder.file);
                    _logger.LogInformation($"Number of comics found: {_filesToIngest.folder.file.Count()}");
                    _logger.LogInformation($"Sleeping for {_jobContinueDelay} hours");
                }            
                await Task.Delay(TimeSpan.FromHours(_jobContinueDelay), stoppingToken);
            }
        }

        public override Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");
            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
