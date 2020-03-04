namespace Uncast.Data.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using MySql.Data.MySqlClient;

    using Uncast.Entities;
    using Uncast.Utils;

    public sealed class AppRoleService : DbServiceBase, IAppRoleService
    {
        public AppRoleService(MySqlConnection dbConnection, ILogger<AppRoleService> logger) : base(dbConnection, logger) { }

        public Task<AppRole?> FindRoleByIdAsync(string roleId, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(roleId, nameof(roleId));

            throw new NotImplementedException();
        }

        public Task<AppRole?> FindRoleByNameAsync(string normalizedRoleName, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(normalizedRoleName, nameof(normalizedRoleName));

            throw new NotImplementedException();
        }

        public Task CreateRoleAsync(AppRole role, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(role, nameof(role));

            throw new NotImplementedException();
        }

        public Task UpdateRoleAsync(AppRole role, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(role, nameof(role));

            throw new NotImplementedException();
        }

        public Task DeleteRoleAsync(AppRole role, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(role, nameof(role));

            throw new NotImplementedException();
        }
    }
}