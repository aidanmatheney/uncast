namespace Uncast.Data.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using MySql.Data.MySqlClient;

    using Uncast.Data.Naming;
    using Uncast.Entities;
    using Uncast.Utils;

    public sealed class FileService : DbServiceBase, IFileService
    {
        public FileService(MySqlConnection dbConnection, ILogger<FileService> logger) : base(dbConnection, logger) { }

        public async Task<IEnumerable<AppFile>> GetAllFilesAsync(CancellationToken cancellationToken = default)
        {
            return await QueryAsync<AppFile>
            (
                $@"
SELECT
    Id,
    Path,
    OriginalName

    FROM {DbTable.File}
;
                ",
                cancellationToken
            ).ConfigureAwait(false);
        }

        public async Task<AppFile?> FindFileByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await QuerySingleOrDefaultAsync<AppFile?>
            (
                $@"
SELECT
    Id,
    Path,
    OriginalName

    FROM {DbTable.File}
    WHERE Id = @id
;
                ",
                new { id },
                cancellationToken
            ).ConfigureAwait(false);
        }

        public async Task CreateFileAsync(AppFile file, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(file, nameof(file));

            await ExecuteAsync
            (
                $@"
INSERT INTO {DbTable.File}(
    Id,
    Path,
    OriginalName
) SELECT
    @id,
    @path,
    @originalName
;
                ",
                new
                {
                    id = file.Id,
                    path = file.Path,
                    originalName = file.OriginalName
                },
                cancellationToken
            ).ConfigureAwait(false);
        }

        public async Task UpdateFileAsync(AppFile file, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(file, nameof(file));

            await ExecuteAsync
            (
                $@"
UPDATE {DbTable.File} SET
    Id = @id,
    Path = @path,
    OriginalName = @originalName
;
                ",
                new
                {
                    id = file.Id,
                    path = file.Path,
                    originalName = file.OriginalName
                },
                cancellationToken
            ).ConfigureAwait(false);
        }

        public async Task DeleteFileAsync(AppFile file, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(file, nameof(file));

            await ExecuteAsync
            (
                $@"
DELETE
    FROM {DbTable.File}
    WHERE Id = @id
;
                ",
                new { id = file.Id },
                cancellationToken
            ).ConfigureAwait(false);
        }
    }
}