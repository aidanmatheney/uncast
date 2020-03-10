namespace Uncast.Data.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using Dapper;

    using Microsoft.Extensions.Logging;

    using MySql.Data.MySqlClient;

    using Uncast.Data.Naming;
    using Uncast.Entities;
    using Uncast.Utils;

    public sealed class PodcastEpisodeService : DbServiceBase, IPodcastEpisodeService
    {
        #region Queries

        private static readonly string SelectLibraryRssEpisode =
        $@"
SELECT
    episode.Id,
    episode.FileId,
    episode.Name,
    episode.Description,
    libraryRssEpisode.Url

    FROM {DbTable.PodcastEpisode} AS episode
    JOIN {DbTable.LibraryRssPodcastEpisode} AS libraryRssEpisode ON
        libraryRssEpisode.Id = episode.Id
        ";

        private static readonly string SelectLibraryYouTubeEpisode =
        $@"
SELECT
    episode.Id,
    episode.FileId,
    episode.Name,
    episode.Description,
    libraryYouTubeEpisode.Url

    FROM {DbTable.PodcastEpisode} AS episode
    JOIN {DbTable.LibraryYouTubePodcastEpisode} AS libraryYouTubeEpisode ON
        libraryYouTubeEpisode.Id = episode.Id
        ";

        private static readonly string SelectCustomRssEpisode =
        $@"
SELECT
    episode.Id,
    episode.FileId,
    episode.Name,
    episode.Description,
    customRssEpisode.Url

    FROM {DbTable.PodcastEpisode} AS episode
    JOIN {DbTable.CustomRssPodcastEpisode} AS customRssEpisode ON
        customRssEpisode.Id = episode.Id
        ";

        private static readonly string SelectCustomYouTubeEpisode =
        $@"
SELECT
    episode.Id,
    episode.FileId,
    episode.Name,
    episode.Description,
    customYouTubeEpisode.Url

    FROM {DbTable.PodcastEpisode} AS episode
    JOIN {DbTable.CustomYouTubePodcastEpisode} AS customYouTubeEpisode ON
        customYouTubeEpisode.Id = episode.Id
        ";

        private static readonly string SelectCustomFileEpisode =
        $@"
SELECT
    episode.Id,
    episode.FileId,
    episode.Name,
    episode.Description,
    customFileEpisode.FileId

    FROM {DbTable.PodcastEpisode} AS episode
    JOIN {DbTable.CustomFilePodcastEpisode} AS customFileEpisode ON
        customFileEpisode.Id = episode.Id
        ";

        #endregion

        public PodcastEpisodeService(MySqlConnection dbConnection, ILogger<PodcastEpisodeService> logger) : base(dbConnection, logger) { }

        public async Task<IEnumerable<PodcastEpisodeBase>> GetAllEpisodesAsync(Guid podcastId, CancellationToken cancellationToken = default)
        {
            using var reader = await QueryMultipleAsync
            (
                $@"
SELECT @podcastType = (SELECT
    Type

    FROM {DbTable.Podcast}
    WHERE Id = @podcastId
);
SELECT @podcastType;

SET @libraryPodcastType = (SELECT
    Type

    FROM {DbTable.LibraryPodcast}
    WHERE
        @podcastType = '{PodcastType.Library}'
        AND Id = @podcastId
);
SELECT @libraryPodcastType;

SET @customPodcastType = (SELECT
    Type

    FROM {DbTable.CustomPodcast}
    WHERE
        @podcastType = '{PodcastType.Custom}'
        AND Id = @podcastId
);
SELECT @customPodcastType;

{SelectLibraryRssEpisode}
    JOIN {DbTable.Podcast} AS podcast ON
        podcast.Id = episode.PodcastId

    WHERE
        @libraryPodcastType = '{LibraryPodcastType.Rss}'
        AND podcast.Id = @podcastId
;

{SelectLibraryYouTubeEpisode}
    JOIN {DbTable.Podcast} AS podcast ON
        podcast.Id = episode.PodcastId

    WHERE
        @libraryPodcastType = '{LibraryPodcastType.YouTube}'
        AND podcast.Id = @podcastId
;

{SelectCustomRssEpisode}
    JOIN {DbTable.Podcast} AS podcast ON
        podcast.Id = episode.PodcastId

    WHERE
        @customPodcastType = '{CustomPodcastType.Rss}'
        AND podcast.Id = @podcastId
;

{SelectCustomYouTubeEpisode}
    JOIN {DbTable.Podcast} AS podcast ON
        podcast.Id = episode.PodcastId

    WHERE
        @customPodcastType = '{CustomPodcastType.YouTube}'
        AND podcast.Id = @podcastId
;

{SelectCustomFileEpisode}
    JOIN {DbTable.Podcast} AS podcast ON
        podcast.Id = episode.PodcastId

    WHERE
        @customPodcastType = '{CustomPodcastType.File}'
        AND podcast.Id = @podcastId
;
                ",
                new { podcastId },
                cancellationToken
            ).ConfigureAwait(false);

            var podcastType = await reader.ReadSingleOrDefaultAsync<string?>().ConfigureAwait(false);
            if (podcastType is null)
                return Array.Empty<PodcastEpisodeBase>();

            var libraryPodcastType = await reader.ReadSingleOrDefaultAsync<string?>().ConfigureAwait(false);
            var customPodcastType = await reader.ReadSingleOrDefaultAsync<string?>().ConfigureAwait(false);

            var libraryRssEpisodes = await reader.ReadAsync<LibraryRssPodcastEpisode>().ConfigureAwait(false);
            var libraryYouTubeEpisodes = await reader.ReadAsync<LibraryYouTubePodcastEpisode>().ConfigureAwait(false);
            var customRssEpisodes = await reader.ReadAsync<CustomRssPodcastEpisode>().ConfigureAwait(false);
            var customYouTubeEpisodes = await reader.ReadAsync<CustomYouTubePodcastEpisode>().ConfigureAwait(false);
            var customFileEpisodes = await reader.ReadAsync<CustomFilePodcastEpisode>().ConfigureAwait(false);

            if (podcastType == PodcastType.Library)
            {
                if (libraryPodcastType == LibraryPodcastType.Rss)
                    return libraryRssEpisodes;
                if (libraryPodcastType == LibraryPodcastType.YouTube)
                    return libraryYouTubeEpisodes;

                Debug.Fail($"Podcast {podcastId} has unknown library podcast type: {libraryPodcastType}");
                return Array.Empty<PodcastEpisodeBase>();
            }

            if (podcastType == PodcastType.Custom)
            {
                if (customPodcastType == CustomPodcastType.Rss)
                    return customRssEpisodes;
                if (customPodcastType == CustomPodcastType.YouTube)
                    return customYouTubeEpisodes;
                if (customPodcastType == CustomPodcastType.File)
                    return customFileEpisodes;

                Debug.Fail($"Podcast {podcastId} has unknown custom podcast type: {customPodcastType}");
                return Array.Empty<PodcastEpisodeBase>();
            }

            Debug.Fail($"Podcast {podcastId} has unknown podcast type: {podcastType}");
            return Array.Empty<PodcastEpisodeBase>();
        }

        public async Task<PodcastEpisodeBase?> FindEpisodeByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            using var reader = await QueryMultipleAsync
            (
                $@"
SELECT @podcastId = (SELECT
    podcast.Id

    FROM {DbTable.Podcast} AS podcast
    JOIN {DbTable.PodcastEpisode} AS episode ON
        episode.PodcastId = podcast.Id

    WHERE episode.Id = @id
);

SELECT @podcastType = (SELECT
    Type

    FROM {DbTable.Podcast}
    WHERE Id = @podcastId
);
SELECT @podcastType;

SET @libraryPodcastType = (SELECT
    Type

    FROM {DbTable.LibraryPodcast}
    WHERE
        @podcastType = '{PodcastType.Library}'
        AND Id = @podcastId
);
SELECT @libraryPodcastType;

SET @customPodcastType = (SELECT
    Type

    FROM {DbTable.CustomPodcast}
    WHERE
        @podcastType = '{PodcastType.Custom}'
        AND Id = @podcastId
);
SELECT @customPodcastType;

{SelectLibraryRssEpisode}
    WHERE
        @libraryPodcastType = '{LibraryPodcastType.Rss}'
        AND episode.Id = @id
;

{SelectLibraryYouTubeEpisode}
    WHERE
        @libraryPodcastType = '{LibraryPodcastType.YouTube}'
        AND episode.Id = @id
;

{SelectCustomRssEpisode}
    WHERE
        @customPodcastType = '{CustomPodcastType.Rss}'
        AND episode.Id = @id
;

{SelectCustomYouTubeEpisode}
    WHERE
        @customPodcastType = '{CustomPodcastType.YouTube}'
        AND episode.Id = @id
;

{SelectCustomFileEpisode}
    WHERE
        @customPodcastType = '{CustomPodcastType.File}'
        AND episode.Id = @id
;
                ",
                new { id },
                cancellationToken
            ).ConfigureAwait(false);

            var podcastType = await reader.ReadSingleOrDefaultAsync<string?>().ConfigureAwait(false);
            if (podcastType is null)
                return null;

            var libraryPodcastType = await reader.ReadSingleOrDefaultAsync<string?>().ConfigureAwait(false);
            var customPodcastType = await reader.ReadSingleOrDefaultAsync<string?>().ConfigureAwait(false);

            var libraryRssEpisode = await reader.ReadSingleOrDefaultAsync<LibraryRssPodcastEpisode?>().ConfigureAwait(false);
            var libraryYouTubeEpisode = await reader.ReadSingleOrDefaultAsync<LibraryYouTubePodcastEpisode?>().ConfigureAwait(false);
            var customRssEpisode = await reader.ReadSingleOrDefaultAsync<CustomRssPodcastEpisode?>().ConfigureAwait(false);
            var customYouTubeEpisode = await reader.ReadSingleOrDefaultAsync<CustomYouTubePodcastEpisode?>().ConfigureAwait(false);
            var customFileEpisode = await reader.ReadSingleOrDefaultAsync<CustomFilePodcastEpisode?>().ConfigureAwait(false);

            if (podcastType == PodcastType.Library)
            {
                if (libraryPodcastType == LibraryPodcastType.Rss)
                    return libraryRssEpisode;
                if (libraryPodcastType == LibraryPodcastType.YouTube)
                    return libraryYouTubeEpisode;

                Debug.Fail($"Podcast episode {id} has unknown library podcast type: {libraryPodcastType}");
                return null;
            }

            if (podcastType == PodcastType.Custom)
            {
                if (customPodcastType == CustomPodcastType.Rss)
                    return customRssEpisode;
                if (customPodcastType == CustomPodcastType.YouTube)
                    return customYouTubeEpisode;
                if (customPodcastType == CustomPodcastType.File)
                    return customFileEpisode;

                Debug.Fail($"Podcast episode {id} has unknown custom podcast type: {customPodcastType}");
                return null;
            }

            Debug.Fail($"Podcast episode {id} has unknown podcast type: {podcastType}");
            return null;
        }

        public async Task CreateEpisodeAsync(PodcastEpisodeBase episode, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(episode, nameof(episode));

            var query = new StringBuilder(
            $@"
INSERT INTO {DbTable.PodcastEpisode}(
    Id,
    PodcastId,
    Name,
    Description
) SELECT
    @id,
    @podcastId,
    @name,
    @description
;
            ");

            var parameters = new DynamicParameters(new
            {
                id = episode.Id,
                podcastId = episode.PodcastId,
                name = episode.Name,
                description = episode.Description
            });

            if (episode is LibraryPodcastEpisodeBase libraryEpisode)
            {
                if (libraryEpisode is LibraryRssPodcastEpisode libraryRssEpisode)
                {
                    query.Append(
                    $@"
INSERT INTO {DbTable.LibraryRssPodcastEpisode}(
    Id,
    Url
) SELECT
    @id,
    @url
;
                    ");

                    parameters.Add("url", libraryRssEpisode.Url);
                }
                else if (libraryEpisode is LibraryYouTubePodcastEpisode libraryYouTubeEpisode)
                {
                    query.Append(
                    $@"
INSERT INTO {DbTable.LibraryYouTubePodcastEpisode}(
    Id,
    Url
) SELECT
    @id,
    @url
;
                    ");

                    parameters.Add("url", libraryYouTubeEpisode.Url);
                }
                else
                {
                    ThrowForUnrecognizedEpisodeType(libraryEpisode);
                }
            }
            else if (episode is CustomPodcastEpisodeBase customEpisode)
            {
                if (customEpisode is CustomRssPodcastEpisode customRssEpisode)
                {
                    query.Append(
                    $@"
INSERT INTO {DbTable.CustomRssPodcastEpisode}(
    Id,
    Url
) SELECT
    @id,
    @url
;
                    ");

                    parameters.Add("url", customRssEpisode.Url);
                }
                else if (customEpisode is CustomYouTubePodcastEpisode customYouTubeEpisode)
                {
                    query.Append(
                    $@"
INSERT INTO {DbTable.CustomYouTubePodcastEpisode}(
    Id,
    Url
) SELECT
    @id,
    @url
;
                    ");

                    parameters.Add("url", customYouTubeEpisode.Url);
                }
                else if (customEpisode is CustomFilePodcastEpisode customFileEpisode) 
                {
                    query.Append(
                    $@"
INSERT INTO {DbTable.CustomFilePodcastEpisode}(
    Id,
    FileId
) SELECT
    @id,
    @fileId
;
                    ");

                    parameters.Add("fileId", customFileEpisode.FileId);
                }
                else
                {
                    ThrowForUnrecognizedEpisodeType(customEpisode);
                }
            }
            else
            {
                ThrowForUnrecognizedEpisodeType(episode);
            }

            await ExecuteAsync
            (
                query.ToString(),
                parameters,
                cancellationToken
            ).ConfigureAwait(false);
        }

        public async Task UpdateEpisodeAsync(PodcastEpisodeBase episode, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(episode, nameof(episode));

            var query = new StringBuilder(
            $@"
UPDATE {DbTable.PodcastEpisode} SET
    PodcastId = @podcastId,
    Name = @name,
    Description = @description

    WHERE Id = @id
;
            ");

            var parameters = new DynamicParameters(new
            {
                id = episode.Id,
                podcastId = episode.PodcastId,
                name = episode.Name,
                description = episode.Description
            });

            if (episode is LibraryPodcastEpisodeBase libraryEpisode)
            {
                if (libraryEpisode is LibraryRssPodcastEpisode libraryRssEpisode)
                {
                    query.Append(
                    $@"
UPDATE {DbTable.LibraryRssPodcastEpisode} SET
    Url = @url

    WHERE Id = @id
;
                    ");

                    parameters.Add("url", libraryRssEpisode.Url);
                }
                else if (libraryEpisode is LibraryYouTubePodcastEpisode libraryYouTubeEpisode)
                {
                    query.Append(
                    $@"
UPDATE {DbTable.LibraryYouTubePodcastEpisode} SET
    Url = @url

    WHERE Id = @id
;
                    ");

                    parameters.Add("url", libraryYouTubeEpisode.Url);
                }
                else
                {
                    ThrowForUnrecognizedEpisodeType(episode);
                }
            }
            else if (episode is CustomPodcastEpisodeBase customEpisode)
            {
                if (customEpisode is CustomRssPodcastEpisode customRssEpisode)
                {
                    query.Append(
                    $@"
UPDATE {DbTable.CustomRssPodcastEpisode} SET
    Url = @url

    WHERE Id = @id
;
                    ");

                    parameters.Add("url", customRssEpisode.Url);
                }
                else if (customEpisode is CustomYouTubePodcastEpisode customYouTubeEpisode)
                {
                    query.Append(
                    $@"
UPDATE {DbTable.CustomYouTubePodcastEpisode} SET
    Url = @url

    WHERE Id = @id
;
                    ");

                    parameters.Add("url", customYouTubeEpisode.Url);
                }
                else if (customEpisode is CustomFilePodcastEpisode customFileEpisode)
                {
                    query.Append(
                    $@"
UPDATE {DbTable.CustomFilePodcastEpisode} SET
    FileId = @fileId

    WHERE Id = @id
;
                    ");

                    parameters.Add("fileId", customFileEpisode.FileId);
                }
                else
                {
                    ThrowForUnrecognizedEpisodeType(episode);
                }
            }
            else
            {
                ThrowForUnrecognizedEpisodeType(episode);
            }

            await ExecuteAsync
            (
                query.ToString(),
                parameters,
                cancellationToken
            ).ConfigureAwait(false);
        }

        public async Task DeleteEpisodeAsync(PodcastEpisodeBase episode, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(episode, nameof(episode));

            await ExecuteAsync
            (
                $@"
DELETE
    FROM {DbTable.PodcastEpisode}
    WHERE Id = @id
;
                ",
                new { id = episode.Id },
                cancellationToken
            ).ConfigureAwait(false);
        }

        private static void ThrowForUnrecognizedEpisodeType(PodcastEpisodeBase episode)
        {
            Debug.Assert(!(episode is null));
            throw new ArgumentException($"Unrecognized podcast episode type: {episode.GetType().FullName}", nameof(episode));
        }
    }
}