namespace Uncast.Data.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Uncast.Entities;

    public interface IUserStateService
    {
        Task<UserAppState?> GetAppStateAsync(Guid userId, CancellationToken cancellationToken = default);
        Task UpdateAppStateAsync(UserAppState state, CancellationToken cancellationToken = default);

        Task<UserPodcastState?> GetPodcastStateAsync(Guid userId, Guid podcastId, CancellationToken cancellationToken = default);
        Task UpdatePodcastStateAsync(UserPodcastState state, CancellationToken cancellationToken = default);

        Task<UserPodcastEpisodeState?> GetEpisodeStateAsync(Guid userId, Guid episodeId, CancellationToken cancellationToken = default);
        Task UpdateEpisodeStateAsync(UserPodcastEpisodeState state, CancellationToken cancellationToken = default);

        Task<UserPodcastPlaybackQueue?> GetPlaybackQueueAsync(Guid userId, CancellationToken cancellationToken = default);
        Task UpdatePlaybackQueueAsync(UserPodcastPlaybackQueue queue, CancellationToken cancellationToken = default);
    }
}
