namespace Uncast.Data.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using MySql.Data.MySqlClient;

    using Uncast.Data.Naming;
    using Uncast.Entities;
    using Uncast.Utils;

    public sealed class UserStateService : DbServiceBase, IUserStateService
    {
        public UserStateService(MySqlConnection dbConnection, ILogger<UserStateService> logger) : base(dbConnection, logger) { }

        public async Task<UserAppState?> GetAppStateAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await QuerySingleOrDefaultAsync<UserAppState?>
            (
                $@"
SELECT
    COALESCE(state.UserId, @userId) AS UserId,
    COALESCE(state.ColorScheme, '{AppColorScheme.System}') AS ColorScheme,
    COALESCE(state.DefaultPlaybackStyle, '{PodcastEpisodePlaybackStyle.Stream}') AS DefaultPlaybackStyle,
    COALESCE(state.DefaultPlaybackSpeed, 1) AS DefaultPlaybackSpeed

    FROM {DbTable.User} AS user
    LEFT JOIN {DbTable.UserAppState} AS state ON
        state.UserId = user.Id

    WHERE user.Id = @userId
;
                ",
                new { userId },
                cancellationToken
            ).ConfigureAwait(false);
        }

        public async Task UpdateAppStateAsync(UserAppState state, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(state, nameof(state));

            await ExecuteAsync
            (
                $@"
INSERT INTO {DbTable.UserAppState}(
    UserId,
    ColorScheme,
    DefaultPlaybackStyle,
    DefaultPlaybackSpeed
) SELECT
    @userId,
    @colorScheme,
    @defaultPlaybackStyle,
    @defaultPlaybackSpeed

    ON DUPLICATE KEY UPDATE
        ColorScheme = @colorScheme,
        DefaultPlaybackStyle = @defaultPlaybackStyle,
        DefaultPlaybackSpeed = @defaultPlaybackSpeed
;
                ",
                new
                {
                    userId = state.UserId,
                    colorScheme = state.ColorScheme,
                    defaultPlaybackStyle = state.DefaultPlaybackStyle,
                    defaultPlaybackSpeed = state.DefaultPlaybackSpeed
                },
                cancellationToken
            ).ConfigureAwait(false);
        }

        public async Task<UserPodcastState?> GetPodcastStateAsync(Guid userId, Guid podcastId, CancellationToken cancellationToken = default)
        {
            return await QuerySingleOrDefaultAsync<UserPodcastState?>
            (
                $@"
SELECT
    COALESCE(state.UserId, @userId) AS UserId,
    COALESCE(state.PodcastId, @podcastId) AS PodcastId,
    COALESCE(state.IsSubscription, 0) AS IsSubscription,
    COALESCE(state.IsFavorite, 0) AS IsFavorite,
    COALESCE(state.IsAutoDownload, 0) AS IsAutoDownload,
    COALESCE(state.PlaybackSpeed, NULL) AS PlaybackSpeed

    FROM {DbTable.User} AS user
    LEFT JOIN {DbTable.UserPodcastState} AS state ON
        state.UserId = user.Id
        AND state.PodcastId = @podcastId

    WHERE user.Id = @userId
;
                ",
                new
                {
                    userId,
                    podcastId
                },
                cancellationToken
            ).ConfigureAwait(false);
        }

        public async Task UpdatePodcastStateAsync(UserPodcastState state, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(state, nameof(state));

            await ExecuteAsync
            (
                $@"
INSERT INTO {DbTable.UserPodcastState}(
    UserId,
    PodcastId,
    IsSubscription,
    IsFavorite,
    IsAutoDownload,
    PlaybackSpeed
) SELECT
    @userId,
    @podcastId,
    @isSubscription,
    @isFavorite,
    @isAutoDownload,
    @playbackSpeed

    ON DUPLICATE KEY UPDATE
        IsSubscription = @isSubscription,
        IsFavorite = @isFavorite,
        IsAutoDownload = @isAutoDownload,
        PlaybackSpeed = @playbackSpeed
;
                ",
                new
                {
                    userId = state.UserId,
                    podcastId = state.PodcastId,
                    isSubscription = state.IsSubscription,
                    isFavorite = state.IsFavorite,
                    isAutoDownload = state.IsAutoDownload,
                    playbackSpeed = state.PlaybackSpeed
                },
                cancellationToken
            ).ConfigureAwait(false);
        }

        public async Task<UserPodcastEpisodeState?> GetEpisodeStateAsync(Guid userId, Guid episodeId, CancellationToken cancellationToken = default)
        {
            return await QuerySingleOrDefaultAsync<UserPodcastEpisodeState?>
            (
                $@"
SELECT
    COALESCE(state.UserId, @userId) AS UserId,
    COALESCE(state.EpisodeId, @episodeId) AS EpisodeId,
    COALESCE(state.PlaybackStatus, '{PodcastEpisodePlaybackStatus.Unplayed}') AS PlaybackStatus,
    COALESCE(state.ProgressMs, NULL) AS ProgressMs

    FROM {DbTable.User} AS user
    LEFT JOIN {DbTable.UserPodcastEpisodeState} AS state ON
        state.UserId = user.Id
        AND state.EpisodeId = @episodeId

    WHERE user.Id = @userId
;
                ",
                new
                {
                    userId,
                    episodeId
                },
                cancellationToken
            ).ConfigureAwait(false);
        }

        public async Task UpdateEpisodeStateAsync(UserPodcastEpisodeState state, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(state, nameof(state));

            await ExecuteAsync
            (
                $@"
INSERT INTO {DbTable.UserPodcastEpisodeState}(
    UserId,
    EpisodeId,
    PlaybackStatus,
    ProgressMs
) SELECT
    @userId,
    @episodeId,
    @playbackStatus,
    @progressMs

    ON DUPLICATE KEY UPDATE
        PlaybackStatus = @playbackStatus,
        ProgressMs = @progressMs
;
                ",
                new
                {
                    userId = state.UserId,
                    episodeId = state.EpisodeId,
                    playbackStatus = state.PlaybackStatus,
                    progressMs = state.ProgressMs
                },
                cancellationToken
            ).ConfigureAwait(false);
        }

        public async Task<UserPodcastPlaybackQueue?> GetPlaybackQueueAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            using var reader = await QueryMultipleAsync
            (
                $@"
SELECT IIF(EXISTS(SELECT *
    FROM {DbTable.User}
    WHERE Id = @userId
), 1, 0);

SELECT
    EpisodeId

    FROM {DbTable.UserPodcastEpisodeQueueItem}
    WHERE UserId = @userId

    ORDER BY Ordinal ASC
;
                ",
                new { userId },
                cancellationToken
            ).ConfigureAwait(false);

            var userExists = await reader.ReadSingleAsync<bool>().ConfigureAwait(false);
            if (!userExists)
                return null;

            var episodeIds = await reader.ReadAsync<Guid>().ConfigureAwait(false);

            return new UserPodcastPlaybackQueue
            {
                UserId = userId,
                EpisodeIds = episodeIds
            };
        }

        public async Task UpdatePlaybackQueueAsync(UserPodcastPlaybackQueue queue, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(queue, nameof(queue));
            ThrowIf.Null(queue.EpisodeIds, "queue.EpisodeIds");

            await using var episodesTable = await TempTableAsync(queue.EpisodeIds!.Index(), table =>
            {
                table.Column("EpisodeId", "char(36) NOT NULL", ep => ep.Element);
                table.Column("Ordinal", "int NOT NULL", ep => ep.Index);
            }, cancellationToken).ConfigureAwait(false);

            await ExecuteAsync
            (
                $@"
DELETE
    FROM {DbTable.UserPodcastEpisodeQueueItem}
    WHERE UserId = @userId
;

INSERT INTO {DbTable.UserPodcastEpisodeQueueItem}(
    UserId,
    EpisodeId,
    Ordinal
) SELECT
    @userId,
    EpisodeId,
    Ordinal

    FROM {episodesTable.Name}
;
                ",
                new { userId = queue.UserId },
                cancellationToken
            ).ConfigureAwait(false);
        }
    }
}
