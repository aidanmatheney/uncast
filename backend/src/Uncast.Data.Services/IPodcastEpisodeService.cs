namespace Uncast.Data.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using Uncast.Entities;

    public interface IPodcastEpisodeService
    {
        Task<IEnumerable<PodcastEpisodeBase>> GetAllEpisodesAsync(Guid podcastId, CancellationToken cancellationToken = default);
        Task<PodcastEpisodeBase?> FindEpisodeByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task CreateEpisodeAsync(PodcastEpisodeBase episode, CancellationToken cancellationToken = default);
        Task UpdateEpisodeAsync(PodcastEpisodeBase episode, CancellationToken cancellationToken = default);
        Task DeleteEpisodeAsync(PodcastEpisodeBase episode, CancellationToken cancellationToken = default);
    }
}