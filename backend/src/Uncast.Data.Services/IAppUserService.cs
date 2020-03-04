namespace Uncast.Data.Services
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using Uncast.Entities;

    public interface IAppUserService
    {
        #region IUserStore

        Task CreateUserAsync(AppUser user, CancellationToken cancellationToken = default);
        Task UpdateUserAsync(AppUser user, CancellationToken cancellationToken = default);
        Task DeleteUserAsync(AppUser user, CancellationToken cancellationToken = default);
        Task<AppUser?> FindUserByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<AppUser?> FindUserByUserNameAsync(string normalizedUserName, CancellationToken cancellationToken = default);

        #endregion

        #region IUserEmailStore

        Task<AppUser?> FindUserByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default);

        #endregion

        #region IUserClaimStore

        Task<IEnumerable<AppUserClaim>> GetUserClaimsAsync(AppUser user, CancellationToken cancellationToken = default);
        Task AddUserClaimsAsync(AppUser user, IEnumerable<AppUserClaim> claims, CancellationToken cancellationToken = default);
        Task ReplaceUserClaimAsync(AppUser user, AppUserClaim oldClaim, AppUserClaim newClaim, CancellationToken cancellationToken = default);
        Task RemoveClaimsAsync(AppUser user, IEnumerable<AppUserClaim> claims, CancellationToken cancellationToken = default);
        Task<IEnumerable<AppUser>> GetUsersForClaimAsync(AppUserClaim claim, CancellationToken cancellationToken = default);

        #endregion

        #region IUserLoginStore

        Task AddUserLoginAsync(AppUser user, AppUserLogin login, CancellationToken cancellationToken = default);
        Task RemoveUserLoginAsync(AppUser user, string loginProvider, string providerKey, CancellationToken cancellationToken = default);
        Task<IEnumerable<AppUserLogin>> GetUserLoginsAsync(AppUser user, CancellationToken cancellationToken = default);
        Task<AppUser?> FindUserByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken = default);

        #endregion

        #region IUserRoleStore

        Task AddUserToRoleAsync(AppUser user, string normalizedRoleName, CancellationToken cancellationToken = default);
        Task RemoveUserFromRoleAsync(AppUser user, string normalizedRoleName, CancellationToken cancellationToken = default);
        Task<IEnumerable<string>> GetUserRolesAsync(AppUser user, CancellationToken cancellationToken = default);
        Task<bool> GetUserIsInRoleAsync(AppUser user, string normalizedRoleName, CancellationToken cancellationToken = default);
        Task<IEnumerable<AppUser>> GetUsersInRoleAsync(string normalizedRoleName, CancellationToken cancellationToken = default);

        #endregion

        #region IUserTwoFactorRecoveryCodeStore

        Task ReplaceUserRecoveryCodesAsync(AppUser user, IEnumerable<string> codes, CancellationToken cancellationToken = default);
        Task<bool> RedeemUserRecoveryCodeAsync(AppUser user, string code, CancellationToken cancellationToken = default);
        Task<int> CountUserRecoveryCodesAsync(AppUser user, CancellationToken cancellationToken = default);

        #endregion
    }
}
