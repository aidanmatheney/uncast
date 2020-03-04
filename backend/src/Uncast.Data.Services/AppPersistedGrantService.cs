namespace Uncast.Data.Services
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using IdentityServer4.Models;

    using Microsoft.Extensions.Logging;

    using MySql.Data.MySqlClient;

    using Uncast.Data.Naming;
    using Uncast.Utils;

    public sealed class AppPersistedGrantService : DbServiceBase, IAppPersistedGrantService
    {
        public AppPersistedGrantService(MySqlConnection dbConnection, ILogger<AppPersistedGrantService> logger) : base(dbConnection, logger) { }

        public async Task<IEnumerable<PersistedGrant>> GetAllGrantsAsync(string subjectId, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(subjectId, nameof(subjectId));

            return await QueryAsync<PersistedGrant>
            (
                $@"
SELECT
    `Key`,
    Type,
    SubjectId,
    ClientId,
    CreationTime,
    Expiration,
    Data

    FROM {DbTable.PersistedGrant}
    WHERE SubjectId = @subjectId
;
                ",
                new { subjectId },
                cancellationToken
            ).ConfigureAwait(false);
        }

        public async Task<PersistedGrant?> FindGrantByKeyAsync(string key, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(key, nameof(key));

            return await QuerySingleOrDefaultAsync<PersistedGrant?>
            (
                $@"
SELECT
    `Key`,
    Type,
    SubjectId,
    ClientId,
    CreationTime,
    Expiration,
    Data

    FROM {DbTable.PersistedGrant}
    WHERE `Key` = @key
;
                ",
                new { key },
                cancellationToken
            ).ConfigureAwait(false);
        }

        public async Task StoreGrantAsync(PersistedGrant grant, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(grant, nameof(grant));

            await ExecuteAsync
            (
                $@"
INSERT INTO {DbTable.PersistedGrant}(
    `Key`,
    Type,
    SubjectId,
    ClientId,
    CreationTime,
    Expiration,
    Data
) SELECT
    @key,
    @type,
    @subjectId,
    @clientId,
    @creationTime,
    @expiration,
    @data
;
                ",
                new
                {
                    key = grant.Key,
                    type = grant.Type,
                    subjectId = grant.SubjectId,
                    clientId = grant.ClientId,
                    creationTime = grant.CreationTime,
                    expiration = grant.Expiration,
                    data = grant.Data
                },
                cancellationToken
            ).ConfigureAwait(false);
        }

        public async Task RemoveGrantAsync(string key, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(key, nameof(key));

            await ExecuteAsync
            (
                $@"
DELETE
    FROM {DbTable.PersistedGrant}
    WHERE `Key` = @key
;
                ",
                new { key },
                cancellationToken
            ).ConfigureAwait(false);
        }

        public async Task RemoveAllGrantsAsync(string subjectId, string clientId, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(subjectId, nameof(subjectId));
            ThrowIf.Null(clientId, nameof(clientId));

            await ExecuteAsync
            (
                $@"
DELETE
    FROM {DbTable.PersistedGrant}
    WHERE
        SubjectId = @subjectId
        AND ClientId = @clientId
;
                ",
                new
                {
                    subjectId,
                    clientId
                },
                cancellationToken
            ).ConfigureAwait(false);
        }

        public async Task RemoveAllGrantsAsync(string subjectId, string clientId, string type, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(subjectId, nameof(subjectId));
            ThrowIf.Null(clientId, nameof(clientId));
            ThrowIf.Null(type, nameof(type));

            await ExecuteAsync
            (
                $@"
DELETE
    FROM {DbTable.PersistedGrant}
    WHERE
        SubjectId = @subjectId
        AND ClientId = @clientId
        AND Type = @type
;
                ",
                new
                {
                    subjectId,
                    clientId,
                    type
                },
                cancellationToken
            ).ConfigureAwait(false);
        }
    }
}