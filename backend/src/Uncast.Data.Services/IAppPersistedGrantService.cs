namespace Uncast.Data.Services
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using IdentityServer4.Models;

    public interface IAppPersistedGrantService
    {
        Task<IEnumerable<PersistedGrant>> GetAllGrantsAsync(string subjectId, CancellationToken cancellationToken = default);
        Task<PersistedGrant?> FindGrantByKeyAsync(string key, CancellationToken cancellationToken = default);
        Task StoreGrantAsync(PersistedGrant grant, CancellationToken cancellationToken = default);
        Task RemoveGrantAsync(string key, CancellationToken cancellationToken = default);
        Task RemoveAllGrantsAsync(string subjectId, string clientId, CancellationToken cancellationToken = default);
        Task RemoveAllGrantsAsync(string subjectId, string clientId, string type, CancellationToken cancellationToken = default);
    }
}