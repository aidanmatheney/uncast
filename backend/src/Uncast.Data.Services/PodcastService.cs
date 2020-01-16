namespace Uncast.Data.Services
{
    using System.Collections.Generic;
    using System.Data;
    using System.Threading;
    using System.Threading.Tasks;

    using Dapper;

    using Uncast.Data.Entities;
    using Uncast.Utils;

    public sealed class PodcastService : IPodcastService
    {
        private readonly IDbConnection _dbConnection;

        public PodcastService(IDbConnection dbConnection)
        {
            ThrowIf.Null(dbConnection, nameof(dbConnection));

            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<LibraryPodcast>> GetLibraryPodcastsAsync(CancellationToken cancellationToken = default)
        {
            using var reader = await _dbConnection.QueryMultipleAsync(new CommandDefinition
            (
                commandText:
                @"
SELECT
    Id,
    Url

    FROM LibraryRssPodcast
;
                ",
                cancellationToken: cancellationToken
            )).ConfigureAwait(false);

            var rssPodcasts = await reader.ReadAsync<LibraryRssPodcast>().ConfigureAwait(false);
            
            return rssPodcasts;
        }

        public async Task<IEnumerable<LibraryRssPodcast>> GetLibraryRssPodcastsAsync(CancellationToken cancellationToken = default)
        {
            return await _dbConnection.QueryAsync<LibraryRssPodcast>(new CommandDefinition
            (
                commandText:
                @"
SELECT
    Id,
    Url

    FROM LibraryRssPodcast
;
                ",
                cancellationToken: cancellationToken
            )).ConfigureAwait(false);
        }

        public async Task<LibraryRssPodcast> GetLibraryRssPodcastAsync(int id, CancellationToken cancellationToken = default)
        { 
            return await _dbConnection.QuerySingleOrDefaultAsync<LibraryRssPodcast>(new CommandDefinition
            (
                commandText:
                @"
SELECT
    Id,
    Url

    FROM LibraryRssPodcast
    WHERE Id = @id
;
                ",
                parameters: new { id },
                cancellationToken: cancellationToken
            )).ConfigureAwait(false);
        }
    }
}