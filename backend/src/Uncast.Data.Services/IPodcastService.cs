namespace Uncast.Data.Services
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using Uncast.Data.Entities;

    public interface IPodcastService
    {
        Task<IEnumerable<LibraryPodcast>> GetLibraryPodcastsAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<LibraryRssPodcast>> GetLibraryRssPodcastsAsync(CancellationToken cancellationToken = default);
        Task<LibraryRssPodcast> GetLibraryRssPodcastAsync(int id, CancellationToken cancellationToken = default);
    }
}