namespace Uncast.Services
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using IdentityServer4.Models;
    using IdentityServer4.Stores;

    using Microsoft.Extensions.Logging;

    using Uncast.Data.Services;
    using Uncast.Utils;

    public sealed class AppPersistedGrantStore : IPersistedGrantStore
    {
        private readonly IAppPersistedGrantService _grantService;
        private readonly ILogger _logger;

        public AppPersistedGrantStore(IAppPersistedGrantService grantService, ILogger<AppPersistedGrantStore> logger)
        {
            ThrowIf.Null(grantService, nameof(grantService));
            ThrowIf.Null(logger, nameof(logger));

            _grantService = grantService;
            _logger = logger;
        }

        public Task StoreAsync(PersistedGrant grant) => StoreAsync(grant, CancellationToken.None);
        public async Task StoreAsync(PersistedGrant grant, CancellationToken cancellationToken)
        {
            ThrowIf.Null(grant, nameof(grant));

            await _grantService.StoreGrantAsync(grant, cancellationToken).ConfigureAwait(false);
        }

        public Task<PersistedGrant?> GetAsync(string key) => GetAsync(key, CancellationToken.None);
        public async Task<PersistedGrant?> GetAsync(string key, CancellationToken cancellationToken)
        {
            ThrowIf.Null(key, nameof(key));

            return await _grantService.FindGrantByKeyAsync(key, cancellationToken).ConfigureAwait(false);
        }

        public Task<IEnumerable<PersistedGrant>> GetAllAsync(string subjectId) => GetAllAsync(subjectId, CancellationToken.None);
        public async Task<IEnumerable<PersistedGrant>> GetAllAsync(string subjectId, CancellationToken cancellationToken)
        {
            ThrowIf.Null(subjectId, nameof(subjectId));

            return await _grantService.GetAllGrantsAsync(subjectId, cancellationToken).ConfigureAwait(false);
        }

        public Task RemoveAsync(string key) => RemoveAsync(key, CancellationToken.None);
        public async Task RemoveAsync(string key, CancellationToken cancellationToken)
        {
            ThrowIf.Null(key, nameof(key));

            await _grantService.RemoveGrantAsync(key, cancellationToken).ConfigureAwait(false);
        }

        public Task RemoveAllAsync(string subjectId, string clientId) => RemoveAllAsync(subjectId, clientId, CancellationToken.None);
        public async Task RemoveAllAsync(string subjectId, string clientId, CancellationToken cancellationToken)
        {
            ThrowIf.Null(subjectId, nameof(subjectId));
            ThrowIf.Null(clientId, nameof(clientId));

            await _grantService.RemoveAllGrantsAsync(subjectId, clientId, cancellationToken).ConfigureAwait(false);
        }

        public Task RemoveAllAsync(string subjectId, string clientId, string type) => RemoveAllAsync(subjectId, clientId, type, CancellationToken.None);
        public async Task RemoveAllAsync(string subjectId, string clientId, string type, CancellationToken cancellationToken)
        {
            ThrowIf.Null(subjectId, nameof(subjectId));
            ThrowIf.Null(clientId, nameof(clientId));
            ThrowIf.Null(type, nameof(type));

            await _grantService.RemoveAllGrantsAsync(subjectId, clientId, type, cancellationToken).ConfigureAwait(false);
        }
    }
}