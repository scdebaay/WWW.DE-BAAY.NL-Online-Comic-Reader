using System.Threading;
using System.Threading.Tasks;

namespace ComicReaderInventoryService.Jobs
{
    public interface IIngestComicJob
    {
        Task DoWork(CancellationToken cancellationToken);
        Task StartAsync(CancellationToken cancellationToken);
        Task StopAsync(CancellationToken cancellationToken);
    }
}