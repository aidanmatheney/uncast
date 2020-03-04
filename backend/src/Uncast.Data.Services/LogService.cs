namespace Uncast.Data.Services
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using MySql.Data.MySqlClient;

    using Uncast.Data.Naming;
    using Uncast.Entities;
    using Uncast.Utils;

    public sealed class LogService : DbServiceBase, ILogService
    {
        public LogService(MySqlConnection dbConnection, ILogger<LogService> logger) : base(dbConnection, logger) { }

        public async Task<IEnumerable<WebApiLogEntry>> GetAllWebApiEntriesAsync(CancellationToken cancellationToken = default)
        {
            return await QueryAsync<WebApiLogEntry>
            (
                $@"
SELECT
    Id,
    TimeWritten

    FROM {DbTable.WebApiLogEntry}
    ORDER BY TimeWritten DESC
;
                ",
                cancellationToken
            ).ConfigureAwait(false);
        }

        public async Task<WebApiLogEntry?> FindWebApiEntryByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await QuerySingleOrDefaultAsync<WebApiLogEntry>
            (
                $@"
SELECT
    Id,
    TimeWritten

    FROM {DbTable.WebApiLogEntry}
    WHERE Id = @id
;
                ",
                new { id },
                cancellationToken
            ).ConfigureAwait(false);
        }

        public async Task InsertWebApiEntriesAsync(IEnumerable<WebApiLogEntry> entries, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(entries, nameof(entries));

            await using var entriesTable = await TempTableAsync(entries, table =>
            {
                table.Column("TimeWritten", "datetime NOT NULL", entry => entry.TimeWritten.DateTime);
                table.Column("ServerName", "varchar(64) NOT NULL", entry => entry.ServerName);
                table.Column("Category", "varchar(256) NOT NULL", entry => entry.Category);
                table.Column("Scope", "longtext NULL", entry => entry.Scope);
                table.Column("LogLevel", "varchar(11) NOT NULL", entry => entry.LogLevel);
                table.Column("EventId", "int NOT NULL", entry => entry.EventId);
                table.Column("EventName", "varchar(64) NULL", entry => entry.EventName);
                table.Column("Message", "longtext NOT NULL", entry => entry.Message);
                table.Column("Exception", "longtext NULL", entry => entry.Exception);
            }, cancellationToken);

            await ExecuteAsync
            (
                $@"
INSERT INTO {DbTable.WebApiLogEntry}(
    TimeWritten,
    ServerName,
    Category,
    Scope,
    LogLevel,
    EventId,
    EventName,
    Message,
    Exception
) SELECT
    TimeWritten,
    ServerName,
    Category,
    Scope,
    LogLevel,
    EventId,
    EventName,
    Message,
    Exception

    FROM {entriesTable.Name}
;
                ",
                cancellationToken
            ).ConfigureAwait(false);
        }

        public async Task DeleteWebApiEntryAsync(WebApiLogEntry entry, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(entry, nameof(entry));

            await ExecuteAsync
            (
                $@"
DELETE
    FROM {DbTable.WebApiLogEntry}
    WHERE Id = @id
;
                ",
                new { id = entry.Id },
                cancellationToken
            ).ConfigureAwait(false);
        }

        public async Task<IEnumerable<WebAppLogEntry>> GetAllWebAppEntriesAsync(CancellationToken cancellationToken = default)
        {
            return await QueryAsync<WebAppLogEntry>
            (
                $@"
SELECT
    Id,
    TimeWritten

    FROM {DbTable.WebAppLogEntry}
    ORDER BY TimeWritten DESC
;
                ",
                cancellationToken
            ).ConfigureAwait(false);
        }

        public async Task<WebAppLogEntry?> FindWebAppEntryByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await QuerySingleOrDefaultAsync<WebAppLogEntry>
            (
                $@"
SELECT
    Id,
    TimeWritten

    FROM {DbTable.WebAppLogEntry}
    WHERE Id = @id
;
                ",
                new { id },
                cancellationToken
            ).ConfigureAwait(false);
        }

        public async Task InsertWebAppEntriesAsync(IEnumerable<WebAppLogEntry> entries, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(entries, nameof(entries));

            await using var entriesTable = await TempTableAsync(entries, table =>
            {
                table.Column("TimeWritten", "datetime NOT NULL", row => row.TimeWritten);
            }, cancellationToken).ConfigureAwait(false);

            await ExecuteAsync
            (
                $@"
INSERT INTO {DbTable.WebAppLogEntry}(
    TimeWritten
) SELECT
    TimeWritten

    FROM {entriesTable.Name}
;
                ",
                cancellationToken
            ).ConfigureAwait(false);
        }

        public async Task DeleteWebAppEntryAsync(WebAppLogEntry entry, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(entry, nameof(entry));

            await ExecuteAsync
            (
                $@"
DELETE
    FROM {DbTable.WebAppLogEntry}
    WHERE Id = @id
;
                ",
                new { id = entry.Id },
                cancellationToken
            ).ConfigureAwait(false);
        }
    }
}