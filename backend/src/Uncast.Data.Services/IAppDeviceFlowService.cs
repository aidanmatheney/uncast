namespace Uncast.Data.Services
{
    using System.Threading;
    using System.Threading.Tasks;

    using IdentityServer4.Models;

    public interface IAppDeviceFlowService
    {
        Task<DeviceCode?> FindDataByUserCodeAsync(string userCode, CancellationToken cancellationToken = default);
        Task<DeviceCode?> FindDataByDeviceCodeAsync(string deviceCode, CancellationToken cancellationToken = default);
        Task StoreDeviceAuthorizationAsync(string deviceCode, string userCode, DeviceCode data, CancellationToken cancellationToken = default);
        Task UpdateDataByUserCodeAsync(string userCode, DeviceCode data, CancellationToken cancellationToken = default);
        Task RemoveDataByDeviceCodeAsync(string deviceCode, CancellationToken cancellationToken = default);
    }
}