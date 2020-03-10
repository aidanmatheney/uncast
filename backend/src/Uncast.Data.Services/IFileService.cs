namespace Uncast.Data.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using Uncast.Entities;

    public interface IFileService
    {
        Task<IEnumerable<AppFile>> GetAllFilesAsync(CancellationToken cancellationToken = default);
        Task<AppFile?> FindFileByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task CreateFileAsync(AppFile file, CancellationToken cancellationToken = default);
        Task UpdateFileAsync(AppFile file, CancellationToken cancellationToken = default);
        Task DeleteFileAsync(AppFile file, CancellationToken cancellationToken = default);
    }
}