namespace Uncast.Data.Services
{
    using System.Threading;
    using System.Threading.Tasks;

    using Uncast.Entities;

    public interface IAppRoleService
    {
        Task<AppRole?> FindRoleByIdAsync(string roleId, CancellationToken cancellationToken = default);
        Task<AppRole?> FindRoleByNameAsync(string normalizedRoleName, CancellationToken cancellationToken = default);
        Task CreateRoleAsync(AppRole role, CancellationToken cancellationToken = default);
        Task UpdateRoleAsync(AppRole role, CancellationToken cancellationToken = default);
        Task DeleteRoleAsync(AppRole role, CancellationToken cancellationToken = default);
    }
}