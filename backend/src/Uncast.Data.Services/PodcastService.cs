namespace Uncast.Data.Services
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using MySql.Data.MySqlClient;

    using Uncast.Data.Naming;
    using Uncast.Entities;

    public sealed class PodcastService : DbServiceBase, IPodcastService
    {
        public PodcastService(MySqlConnection dbConnection, ILogger<PodcastService> logger) : base(dbConnection, logger) { }

        public async Task<IEnumerable<LibraryPodcast>> GetLibraryPodcastsAsync(CancellationToken cancellationToken = default)
        {
            using var reader = await QueryMultipleAsync
            (
                $@"
SELECT
    Id,
    Url

    FROM {DbTable.LibraryRssPodcast}
;
                ",
                cancellationToken
            ).ConfigureAwait(false);

            var rssPodcasts = await reader.ReadAsync<LibraryRssPodcast>().ConfigureAwait(false);

            return rssPodcasts;
        }

        public async Task<IEnumerable<LibraryRssPodcast>> GetLibraryRssPodcastsAsync(CancellationToken cancellationToken = default)
        {
            return await QueryAsync<LibraryRssPodcast>
            (
                $@"
SELECT
    Id,
    Url

    FROM {DbTable.LibraryRssPodcast}
;
                ",
                cancellationToken
            ).ConfigureAwait(false);
        }

        public async Task<LibraryRssPodcast?> FindLibraryRssPodcastByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await QuerySingleOrDefaultAsync<LibraryRssPodcast?>
            (
                $@"
SELECT
    Id,
    Url

    FROM {DbTable.LibraryRssPodcast}
    WHERE Id = @id
;
                ",
                new { id },
                cancellationToken
            ).ConfigureAwait(false);
        }
    }
}
