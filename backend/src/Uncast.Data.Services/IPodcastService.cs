namespace Uncast.Data.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using Uncast.Entities;

    public interface IPodcastService
    {
        Task<IEnumerable<PodcastBase>> GetAllPodcastsAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<IEnumerable<LibraryPodcastBase>> GetAllLibraryPodcastsAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<LibraryRssPodcast>> GetAllLibraryRssPodcastsAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<LibraryYouTubePodcast>> GetAllLibraryYouTubePodcastsAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<CustomPodcastBase>> GetAllCustomPodcastsAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<IEnumerable<CustomRssPodcast>> GetAllCustomRssPodcastsAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<IEnumerable<CustomYouTubePodcast>> GetAllCustomYouTubePodcastsAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<IEnumerable<CustomFilePodcast>> GetAllCustomFilePodcastsAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<PodcastBase?> FindPodcastByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task CreatePodcastAsync(PodcastBase podcast, CancellationToken cancellationToken = default);
        Task UpdatePodcastAsync(PodcastBase podcast, CancellationToken cancellationToken = default);
        Task DeletePodcastAsync(PodcastBase podcast, CancellationToken cancellationToken = default);
    }
}
