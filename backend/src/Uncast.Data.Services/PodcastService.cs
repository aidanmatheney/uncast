namespace Uncast.Data.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Dapper;
    using Microsoft.Extensions.Logging;

    using MySql.Data.MySqlClient;
    using Uncast.Data.Naming;
    using Uncast.Entities;
    using Uncast.Utils;

    public sealed class PodcastService : DbServiceBase, IPodcastService
    {
        #region Queries

        private static readonly string SelectLibraryRssPodcast =
        $@"
SELECT
    podcast.Id,
    podcast.Name,
    podcast.Description,
    podcast.Author,
    podcast.ThumbnailFileId,
    libraryRssPodcast.FeedUrl

    FROM {DbTable.Podcast} AS podcast
    JOIN {DbTable.LibraryPodcast} AS libraryPodcast ON
        libraryPodcast.Id = podcast.Id
    JOIN {DbTable.LibraryRssPodcast} AS libraryRssPodcast ON
        libraryRssPodcast.Id = libraryPodcast.Id
        ";

        private static readonly string GetAllLibraryRssPodcastsQuery =
        $@"
{SelectLibraryRssPodcast}
    WHERE
        podcast.Type = '{PodcastType.Library}'
        AND libraryPodcast.Type = '{LibraryPodcastType.Rss}'
;
        ";

        private static readonly string SelectLibraryYouTubePodcast =
        $@"
SELECT
    podcast.Id,
    podcast.Name,
    podcast.Description,
    podcast.Author,
    podcast.ThumbnailFileId,
    libraryYouTubePodcast.ChannelId

    FROM {DbTable.Podcast} AS podcast
    JOIN {DbTable.LibraryPodcast} AS libraryPodcast ON
        libraryPodcast.Id = podcast.Id
    JOIN {DbTable.LibraryYouTubePodcast} AS libraryYouTubePodcast ON
        libraryYouTubePodcast.Id = libraryPodcast.Id
        ";

        private static readonly string GetAllLibraryYouTubePodcastsQuery =
        $@"
{SelectLibraryYouTubePodcast}
    WHERE
        podcast.Type = '{PodcastType.Library}'
        AND libraryPodcast.Type = '{LibraryPodcastType.YouTube}'
;
        ";

        private static readonly string SelectCustomRssPodcast =
        $@"
SELECT
    podcast.Id,
    customPodcast.UserId,
    podcast.Name,
    podcast.Description,
    podcast.Author,
    podcast.ThumbnailFileId,
    customRssPodcast.FeedUrl

    FROM {DbTable.Podcast} AS podcast
    JOIN {DbTable.CustomPodcast} AS customPodcast ON
        customPodcast.Id = podcast.Id
    JOIN {DbTable.CustomRssPodcast} AS customRssPodcast ON
        customRssPodcast.Id = customPodcast.Id
        ";

        private static readonly string GetAllCustomRssPodcastsQuery =
        $@"
{SelectCustomRssPodcast}
    WHERE
        podcast.Type = '{PodcastType.Custom}'
        AND customPodcast.Type = '{CustomPodcastType.Rss}'
;
        ";

        private static readonly string SelectCustomYouTubePodcast =
        $@"
SELECT
    podcast.Id,
    customPodcast.UserId,
    podcast.Name,
    podcast.Description,
    podcast.Author,
    podcast.ThumbnailFileId,
    customYouTubePodcast.ChannelId

    FROM {DbTable.Podcast} AS podcast
    JOIN {DbTable.CustomPodcast} AS customPodcast ON
        customPodcast.Id = podcast.Id
    JOIN {DbTable.CustomYouTubePodcast} AS customYouTubePodcast ON
        customYouTubePodcast.Id = customPodcast.Id
        ";

        private static readonly string GetAllCustomYouTubePodcastsQuery =
        $@"
{SelectCustomYouTubePodcast}
    WHERE
        podcast.Type = '{PodcastType.Custom}'
        AND customPodcast.UserId = @userId
        AND customPodcast.Type = '{CustomPodcastType.YouTube}'
;
        ";

        private static readonly string SelectCustomFilePodcast =
        $@"
SELECT
    podcast.Id,
    customPodcast.UserId,
    podcast.Name,
    podcast.Description,
    podcast.Author,
    podcast.ThumbnailFileId

    FROM {DbTable.Podcast} AS podcast
    JOIN {DbTable.CustomPodcast} AS customPodcast ON
        customPodcast.Id = podcast.Id
    JOIN {DbTable.CustomFilePodcast} AS customFilePodcast ON
        customFilePodcast.Id = customPodcast.Id
        ";

        private static readonly string GetAllCustomFilePodcastsQuery =
        $@"
{SelectCustomFilePodcast}
    WHERE
        podcast.Type = '{PodcastType.Custom}'
        AND customPodcast.UserId = @userId
        AND customPodcast.Type = '{CustomPodcastType.File}'
;
        ";

        #endregion

        public PodcastService(MySqlConnection dbConnection, ILogger<PodcastService> logger) : base(dbConnection, logger) { }

        public async Task<IEnumerable<PodcastBase>> GetAllPodcastsAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            using var reader = await QueryMultipleAsync
            (
                $@"
{GetAllLibraryRssPodcastsQuery}
{GetAllLibraryYouTubePodcastsQuery}
{GetAllCustomRssPodcastsQuery}
{GetAllCustomYouTubePodcastsQuery}
{GetAllCustomFilePodcastsQuery}
                ",
                new { userId },
                cancellationToken
            ).ConfigureAwait(false);

            IEnumerable<PodcastBase> libraryRssPodcasts = await reader.ReadAsync<LibraryRssPodcast>().ConfigureAwait(false);
            IEnumerable<PodcastBase> libraryYouTubePodcasts = await reader.ReadAsync<LibraryYouTubePodcast>().ConfigureAwait(false);
            IEnumerable<PodcastBase> rssPodcasts = await reader.ReadAsync<CustomRssPodcast>().ConfigureAwait(false);
            IEnumerable<PodcastBase> youTubePodcasts = await reader.ReadAsync<CustomYouTubePodcast>().ConfigureAwait(false);
            IEnumerable<PodcastBase> filePodcasts = await reader.ReadAsync<CustomFilePodcast>().ConfigureAwait(false);

            return EnumerableUtils.ConcatMany
            (
                libraryRssPodcasts,
                libraryYouTubePodcasts,
                rssPodcasts,
                youTubePodcasts,
                filePodcasts
            );
        }

        public async Task<IEnumerable<LibraryPodcastBase>> GetAllLibraryPodcastsAsync(CancellationToken cancellationToken = default)
        {
            using var reader = await QueryMultipleAsync
            (
                $@"
{GetAllLibraryRssPodcastsQuery}
{GetAllLibraryYouTubePodcastsQuery}
                ",
                cancellationToken
            ).ConfigureAwait(false);

            IEnumerable<LibraryPodcastBase> libraryRssPodcasts = await reader.ReadAsync<LibraryRssPodcast>().ConfigureAwait(false);
            IEnumerable<LibraryPodcastBase> libraryYouTubePodcasts = await reader.ReadAsync<LibraryYouTubePodcast>().ConfigureAwait(false);

            return libraryRssPodcasts.Concat(libraryYouTubePodcasts);
        }

        public async Task<IEnumerable<LibraryRssPodcast>> GetAllLibraryRssPodcastsAsync(CancellationToken cancellationToken = default)
        {
            return await QueryAsync<LibraryRssPodcast>
            (
                GetAllLibraryRssPodcastsQuery,
                cancellationToken
            ).ConfigureAwait(false);
        }

        public async Task<IEnumerable<LibraryYouTubePodcast>> GetAllLibraryYouTubePodcastsAsync(CancellationToken cancellationToken = default)
        {
            return await QueryAsync<LibraryYouTubePodcast>
            (
                GetAllLibraryYouTubePodcastsQuery,
                cancellationToken
            ).ConfigureAwait(false);
        }

        public async Task<IEnumerable<CustomPodcastBase>> GetAllCustomPodcastsAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            using var reader = await QueryMultipleAsync
            (
                $@"
{GetAllCustomRssPodcastsQuery}
{GetAllCustomYouTubePodcastsQuery}
{GetAllCustomFilePodcastsQuery}
                ",
                new { userId },
                cancellationToken
            ).ConfigureAwait(false);

            IEnumerable<CustomPodcastBase> rssPodcasts = await reader.ReadAsync<CustomRssPodcast>().ConfigureAwait(false);
            IEnumerable<CustomPodcastBase> youTubePodcasts = await reader.ReadAsync<CustomYouTubePodcast>().ConfigureAwait(false);
            IEnumerable<CustomPodcastBase> filePodcasts = await reader.ReadAsync<CustomFilePodcast>().ConfigureAwait(false);

            return EnumerableUtils.ConcatMany(rssPodcasts, youTubePodcasts, filePodcasts);
        }

        public async Task<IEnumerable<CustomRssPodcast>> GetAllCustomRssPodcastsAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await QueryAsync<CustomRssPodcast>
            (
                GetAllCustomRssPodcastsQuery,
                new { userId },
                cancellationToken
            ).ConfigureAwait(false);
        }

        public async Task<IEnumerable<CustomYouTubePodcast>> GetAllCustomYouTubePodcastsAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await QueryAsync<CustomYouTubePodcast>
            (
                GetAllCustomYouTubePodcastsQuery,
                new { userId },
                cancellationToken
            ).ConfigureAwait(false);
        }

        public async Task<IEnumerable<CustomFilePodcast>> GetAllCustomFilePodcastsAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await QueryAsync<CustomFilePodcast>
            (
                GetAllCustomFilePodcastsQuery,
                new { userId },
                cancellationToken
            ).ConfigureAwait(false);
        }

        public async Task<PodcastBase?> FindPodcastByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            using var reader = await QueryMultipleAsync
            (
                $@"
SET @podcastType = (SELECT
    Type

    FROM {DbTable.Podcast}
    WHERE Id = @id
);
SELECT @podcastType;

SET @libraryPodcastType = (SELECT
    Type

    FROM {DbTable.LibraryPodcast}
    WHERE
        @podcastType = '{PodcastType.Library}'
        AND Id = @id
);
SELECT @libraryPodcastType;

SET @customPodcastType = (SELECT
    Type

    FROM {DbTable.CustomPodcast}
    WHERE
        @podcastType = '{PodcastType.Custom}'
        AND Id = @id
);
SELECT @customPodcastType;

{SelectLibraryRssPodcast}
    WHERE
        @libraryPodcastType = '{LibraryPodcastType.Rss}'
        AND podcast.Id = @id
;

{SelectLibraryYouTubePodcast}
    WHERE
        @libraryPodcastType = '{LibraryPodcastType.YouTube}'
        AND podcast.Id = @id
;

{SelectCustomRssPodcast}
    WHERE
        @customPodcastType = '{CustomPodcastType.Rss}'
        AND podcast.Id = @id
;

{SelectCustomYouTubePodcast}
    WHERE
        @customPodcastType = '{CustomPodcastType.YouTube}'
        AND podcast.Id = @id
;

{SelectCustomFilePodcast}
    WHERE
        @customPodcastType = '{CustomPodcastType.File}'
        AND podcast.Id = @id
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

            var libraryRssPodcast = await reader.ReadSingleOrDefaultAsync<LibraryRssPodcast?>().ConfigureAwait(false);
            var libraryYouTubePodcast = await reader.ReadSingleOrDefaultAsync<LibraryYouTubePodcast?>().ConfigureAwait(false);
            var customRssPodcast = await reader.ReadSingleOrDefaultAsync<CustomRssPodcast?>().ConfigureAwait(false);
            var customYouTubePodcast = await reader.ReadSingleOrDefaultAsync<CustomYouTubePodcast?>().ConfigureAwait(false);
            var customFilePodcast = await reader.ReadSingleOrDefaultAsync<CustomFilePodcast?>().ConfigureAwait(false);

            if (podcastType == PodcastType.Library)
            {
                if (libraryPodcastType == LibraryPodcastType.Rss)
                    return libraryRssPodcast;
                if (libraryPodcastType == LibraryPodcastType.YouTube)
                    return libraryYouTubePodcast;

                Debug.Fail($"Podcast {id} has unknown library podcast type: {libraryPodcastType}");
                return null;
            }

            if (podcastType == PodcastType.Custom)
            {
                if (customPodcastType == CustomPodcastType.Rss)
                    return customRssPodcast;
                if (customPodcastType == CustomPodcastType.YouTube)
                    return customYouTubePodcast;
                if (customPodcastType == CustomPodcastType.File)
                    return customFilePodcast;

                Debug.Fail($"Podcast {id} has unknown custom podcast type: {customPodcastType}");
                return null;
            }

            Debug.Fail($"Podcast {id} has unknown podcast type: {podcastType}");
            return null;
        }

        public async Task CreatePodcastAsync(PodcastBase podcast, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(podcast, nameof(podcast));

            var query = new StringBuilder(
            $@"
INSERT INTO {DbTable.Podcast}(
    Id,
    Type,
    Name,
    Author,
    Description,
    ThumbnailFileId
) SELECT
    @id,
    @podcastType,
    @name,
    @author,
    @description,
    @thumbnailFileId
;
            ");

            var parameters = new DynamicParameters(new
            {
                id = podcast.Id,
                name = podcast.Name,
                author = podcast.Author,
                description = podcast.Description,
                thumbnailFileId = podcast.ThumbnailFileId
            });

            if (podcast is LibraryPodcastBase libraryPodcast)
            {
                parameters.Add("podcastType", PodcastType.Library);

                query.Append(
                $@"
INSERT INTO {DbTable.LibraryPodcast}(
    Id,
    Type
) SELECT
    @id,
    @libraryPodcastType
;
                ");

                if (libraryPodcast is LibraryRssPodcast libraryRssPodcast)
                {
                    parameters.Add("libraryPodcastType", LibraryPodcastType.Rss);

                    query.Append(
                    $@"
INSERT INTO {DbTable.LibraryRssPodcast}(
    Id,
    FeedUrl
) SELECT
    @id,
    @feedUrl
;
                    ");

                    parameters.Add("feedUrl", libraryRssPodcast.FeedUrl);
                }
                else if (libraryPodcast is LibraryYouTubePodcast libraryYouTubePodcast)
                {
                    parameters.Add("libraryPodcastType", LibraryPodcastType.YouTube);

                    query.Append(
                    $@"
INSERT INTO {DbTable.LibraryYouTubePodcast}(
    Id,
    ChannelId
) SELECT
    @id,
    @channelId
;
                    ");

                    parameters.Add("channelId", libraryYouTubePodcast.ChannelId);
                }
                else
                {
                    ThrowForUnrecognizedPodcastType(libraryPodcast);
                }
            }
            else if (podcast is CustomPodcastBase customPodcast)
            {
                parameters.Add("podcastType", PodcastType.Custom);

                query.Append(
                $@"
INSERT INTO {DbTable.CustomPodcast}(
    Id,
    Type,
    UserId
) SELECT
    @id,
    @customPodcastType,
    @userId
;
                ");

                parameters.Add("userId", customPodcast.UserId);

                if (customPodcast is CustomRssPodcast customRssPodcast)
                {
                    parameters.Add("customPodcastType", CustomPodcastType.Rss);

                    query.Append(
                    $@"
INSERT INTO {DbTable.CustomRssPodcast}(
    Id,
    FeedUrl
) SELECT
    @id,
    @feedUrl
;
                    ");

                    parameters.Add("feedUrl", customRssPodcast.FeedUrl);
                }
                else if (customPodcast is CustomYouTubePodcast customYouTubePodcast)
                {
                    parameters.Add("customPodcastType", CustomPodcastType.YouTube);

                    query.Append(
                    $@"
INSERT INTO {DbTable.CustomYouTubePodcast}(
    Id,
    ChannelId
) SELECT
    @id,
    @channelId
;
                    ");

                    parameters.Add("channelId", customYouTubePodcast.ChannelId);
                }
                else if (customPodcast is CustomFilePodcast)
                {
                    parameters.Add("customPodcastType", CustomPodcastType.File);

                    query.Append(
                    $@"
INSERT INTO {DbTable.CustomFilePodcast}(
    Id
) SELECT
    @id
;
                    ");
                }
                else
                {
                    ThrowForUnrecognizedPodcastType(customPodcast);
                }
            }
            else
            {
                ThrowForUnrecognizedPodcastType(podcast);
            }

            await ExecuteAsync
            (
                query.ToString(),
                parameters,
                cancellationToken
            ).ConfigureAwait(false);
        }

        public async Task UpdatePodcastAsync(PodcastBase podcast, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(podcast, nameof(podcast));

            var query = new StringBuilder(
            $@"
UPDATE {DbTable.Podcast} SET
    Name = @name,
    Author = @author,
    Description = @description,
    ThumbnailFileId = @thumbnailFileId

    WHERE Id = @id
;
            ");

            var parameters = new DynamicParameters(new
            {
                id = podcast.Id,
                name = podcast.Name,
                author = podcast.Author,
                description = podcast.Description,
                thumbnailFileId = podcast.ThumbnailFileId
            });

            if (podcast is LibraryPodcastBase libraryPodcast)
            {
                if (libraryPodcast is LibraryRssPodcast libraryRssPodcast)
                {
                    query.Append(
                    $@"
UPDATE {DbTable.LibraryRssPodcast} SET
    FeedUrl = @feedUrl

    WHERE Id = @id
;
                    ");

                    parameters.Add("feedUrl", libraryRssPodcast.FeedUrl);
                }
                else if (libraryPodcast is LibraryYouTubePodcast libraryYouTubePodcast)
                {
                    query.Append(
                    $@"
UPDATE {DbTable.LibraryYouTubePodcast} SET
    ChannelId = @channelId

    WHERE Id = @id
;
                    ");

                    parameters.Add("channelId", libraryYouTubePodcast.ChannelId);
                }
                else
                {
                    ThrowForUnrecognizedPodcastType(libraryPodcast);
                }
            }
            else if (podcast is CustomPodcastBase customPodcast)
            {
                query.Append(
                $@"
UPDATE {DbTable.CustomPodcast} SET
    UserId = @userId

    WHERE Id = @id
;
                ");

                parameters.Add("userId", customPodcast.UserId);

                if (customPodcast is CustomRssPodcast customRssPodcast)
                {
                    query.Append(
                    $@"
UPDATE {DbTable.CustomRssPodcast} SET
    FeedUrl = @feedUrl

    WHERE Id = @id
;
                    ");
                    
                    parameters.Add("feedUrl", customRssPodcast.FeedUrl);
                }
                else if (customPodcast is CustomYouTubePodcast customYouTubePodcast)
                {
                    query.Append(
                    $@"
UPDATE {DbTable.CustomYouTubePodcast} SET
    ChannelId = @channelId

    WHERE Id = @id
;
                    ");

                    parameters.Add("channelId", customYouTubePodcast.ChannelId);
                }
                else if (customPodcast is CustomFilePodcast)
                {
                    // CustomFilePodcast table has no special fields
                }
                else
                {
                    ThrowForUnrecognizedPodcastType(customPodcast);
                }
            }
            else
            {
                ThrowForUnrecognizedPodcastType(podcast);
            }

            await ExecuteAsync
            (
                query.ToString(),
                parameters,
                cancellationToken
            ).ConfigureAwait(false);
        }

        public async Task DeletePodcastAsync(PodcastBase podcast, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(podcast, nameof(podcast));
            
            await ExecuteAsync
            (
                $@"
DELETE
    FROM {DbTable.Podcast}
    WHERE Id = @id
;
                ",
                new { id = podcast.Id },
                cancellationToken
            ).ConfigureAwait(false);
        }

        private static void ThrowForUnrecognizedPodcastType(PodcastBase podcast)
        {
            Debug.Assert(!(podcast is null));
            throw new ArgumentException($"Unrecognized podcast type: {podcast.GetType().FullName}", nameof(podcast));
        }
    }
}