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
        private readonly IConfiguration _configuration;
        private readonly ISqlIngestDbConnection _databaseConnection;
        private IRootModel _filesToIngest;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly DirectoryInfo _folderToScan;
        private int _jobStartDelay;
        private int _jobContinueDelay;

        AutoResetEvent autoEvent = new AutoResetEvent(false);
        private Timer stateTimer;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        }

        public Worker(IHostApplicationLifetime hostApplicationLifetime, ILogger<Worker> logger, IConfiguration configuration, IOptions<RootModel> filesModel, IOptions<SqlIngestDbConnection> sqlIngestDbConnection)
        {
            _hostApplicationLifetime = hostApplicationLifetime;
            _logger = logger;
            _configuration = configuration;
            _databaseConnection = sqlIngestDbConnection.Value;
            _filesToIngest = filesModel.Value;
            _folderToScan = new DirectoryInfo(_configuration["foldersToScan:0:comicFolder"]);

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await StartAsync(stoppingToken);
            }
        }

        public override Task StartAsync(CancellationToken stoppingToken)
        {
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
            stateTimer = new Timer(Crawler, autoEvent, TimeSpan.FromSeconds(_jobStartDelay), TimeSpan.FromHours(_jobContinueDelay));
            return Task.CompletedTask;
        }

        private async void Crawler(object state)
        {
            _logger.LogInformation("Job started at: {time}", DateTimeOffset.Now);
            _filesToIngest = await Task.Run(() => FolderCrawler.GetDirectory(_folderToScan));
            _logger.LogInformation($"Returned from crawling at: {DateTimeOffset.Now}");
            _databaseConnection.InsertComics(_filesToIngest.folder.file, _logger);
            _logger.LogInformation($"Number of comics found: {_filesToIngest.folder.file.Count()}");
            _logger.LogInformation($"Sleeping for {_jobContinueDelay} hours");
        }

        public override Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            stateTimer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            stateTimer?.Dispose();
            base.Dispose();
        }
    }
}
