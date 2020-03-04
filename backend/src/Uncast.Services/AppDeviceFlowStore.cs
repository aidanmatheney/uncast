namespace Uncast.Services
{
    using System.Threading;
    using System.Threading.Tasks;

    using IdentityServer4.Models;
    using IdentityServer4.Stores;

    using Microsoft.Extensions.Logging;

    using Uncast.Data.Services;
    using Uncast.Utils;

    public sealed class AppDeviceFlowStore : IDeviceFlowStore
    {
        private readonly IAppDeviceFlowService _flowService;
        private readonly ILogger _logger;

        public AppDeviceFlowStore(IAppDeviceFlowService flowService, ILogger<AppDeviceFlowStore> logger)
        {
            ThrowIf.Null(flowService, nameof(flowService));
            ThrowIf.Null(logger, nameof(logger));

            _flowService = flowService;
            _logger = logger;
        }

        public Task StoreDeviceAuthorizationAsync(string deviceCode, string userCode, DeviceCode data) => StoreDeviceAuthorizationAsync(deviceCode, userCode, data, CancellationToken.None);
        public async Task StoreDeviceAuthorizationAsync(string deviceCode, string userCode, DeviceCode data, CancellationToken cancellationToken)
        {
            ThrowIf.Null(deviceCode, nameof(deviceCode));
            ThrowIf.Null(userCode, nameof(userCode));
            ThrowIf.Null(data, nameof(data));

            await _flowService.StoreDeviceAuthorizationAsync(deviceCode, userCode, data, cancellationToken).ConfigureAwait(false);
        }

        public Task<DeviceCode> FindByUserCodeAsync(string userCode) => FindByUserCodeAsync(userCode, CancellationToken.None);
        public async Task<DeviceCode> FindByUserCodeAsync(string userCode, CancellationToken cancellationToken)
        {
            ThrowIf.Null(userCode, nameof(userCode));

#pragma warning disable CS8603 // Possible null reference return.
            return await _flowService.FindDataByUserCodeAsync(userCode, cancellationToken).ConfigureAwait(false);
#pragma warning restore CS8603 // Possible null reference return.
        }

        public Task<DeviceCode> FindByDeviceCodeAsync(string deviceCode) => FindByDeviceCodeAsync(deviceCode, CancellationToken.None);
        public async Task<DeviceCode> FindByDeviceCodeAsync(string deviceCode, CancellationToken cancellationToken)
        {
            ThrowIf.Null(deviceCode, nameof(deviceCode));

#pragma warning disable CS8603 // Possible null reference return.
            return await _flowService.FindDataByDeviceCodeAsync(deviceCode, cancellationToken).ConfigureAwait(false);
#pragma warning restore CS8603 // Possible null reference return.
        }

        public Task UpdateByUserCodeAsync(string userCode, DeviceCode data) => UpdateByUserCodeAsync(userCode, data, CancellationToken.None);
        public async Task UpdateByUserCodeAsync(string userCode, DeviceCode data, CancellationToken cancellationToken)
        {
            ThrowIf.Null(userCode, nameof(userCode));
            ThrowIf.Null(data, nameof(data));

            await _flowService.UpdateDataByUserCodeAsync(userCode, data, cancellationToken).ConfigureAwait(false);
        }

        public Task RemoveByDeviceCodeAsync(string deviceCode) => RemoveByDeviceCodeAsync(deviceCode, CancellationToken.None);
        public async Task RemoveByDeviceCodeAsync(string deviceCode, CancellationToken cancellationToken)
        {
            ThrowIf.Null(deviceCode, nameof(deviceCode));
            await _flowService.RemoveDataByDeviceCodeAsync(deviceCode, cancellationToken).ConfigureAwait(false);
        }
    }
}