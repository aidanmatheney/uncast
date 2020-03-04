namespace Uncast.Data.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using IdentityServer4.Models;

    using Microsoft.Extensions.Logging;

    using MySql.Data.MySqlClient;

    using Uncast.Utils;

    public sealed class AppDeviceFlowService : DbServiceBase, IAppDeviceFlowService
    {
        public AppDeviceFlowService(MySqlConnection dbConnection, ILogger<AppDeviceFlowService> logger) : base(dbConnection, logger) { }

        public Task<DeviceCode?> FindDataByUserCodeAsync(string userCode, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(userCode, nameof(userCode));

            throw new NotImplementedException();
        }

        public Task<DeviceCode?> FindDataByDeviceCodeAsync(string deviceCode, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(deviceCode, nameof(deviceCode));

            throw new NotImplementedException();
        }

        public Task StoreDeviceAuthorizationAsync(string deviceCode, string userCode, DeviceCode data, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(deviceCode, nameof(deviceCode));
            ThrowIf.Null(userCode, nameof(userCode));
            ThrowIf.Null(data, nameof(data));

            throw new NotImplementedException();
        }

        public Task UpdateDataByUserCodeAsync(string userCode, DeviceCode data, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(userCode, nameof(userCode));
            ThrowIf.Null(data, nameof(data));

            throw new NotImplementedException();
        }

        public Task RemoveDataByDeviceCodeAsync(string deviceCode, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(deviceCode, nameof(deviceCode));

            throw new NotImplementedException();
        }
    }
}