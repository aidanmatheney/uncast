namespace Uncast.Data.Services
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using Uncast.Entities;

    public interface ILogService
    {
        Task<IEnumerable<WebApiLogEntry>> GetAllWebApiEntriesAsync(CancellationToken cancellationToken = default);
        Task<WebApiLogEntry?> FindWebApiEntryByIdAsync(int id, CancellationToken cancellationToken = default);
        Task InsertWebApiEntriesAsync(IEnumerable<WebApiLogEntry> entries, CancellationToken cancellationToken = default);
        Task DeleteWebApiEntryAsync(WebApiLogEntry entry, CancellationToken cancellationToken = default);

        Task<IEnumerable<WebAppLogEntry>> GetAllWebAppEntriesAsync(CancellationToken cancellationToken = default);
        Task<WebAppLogEntry?> FindWebAppEntryByIdAsync(int id, CancellationToken cancellationToken = default);
        Task InsertWebAppEntriesAsync(IEnumerable<WebAppLogEntry> entries, CancellationToken cancellationToken = default);
        Task DeleteWebAppEntryAsync(WebAppLogEntry entry, CancellationToken cancellationToken = default);
    }
}