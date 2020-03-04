namespace Uncast.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Logging;

    using Uncast.Data.Services;
    using Uncast.Entities;
    using Uncast.Utils;

    public class AppUserStore :
        IUserStore<AppUser>,

        IUserEmailStore<AppUser>,
        IUserPasswordStore<AppUser>,
        IUserSecurityStampStore<AppUser>,
        IUserPhoneNumberStore<AppUser>,
        IUserTwoFactorStore<AppUser>,
        IUserLockoutStore<AppUser>,

        IUserAuthenticatorKeyStore<AppUser>,

        IUserClaimStore<AppUser>,
        IUserLoginStore<AppUser>,
        IUserRoleStore<AppUser>,

        IUserTwoFactorRecoveryCodeStore<AppUser>
    {
        private readonly IAppUserService _userService;
        private readonly ILogger _logger;

        public AppUserStore(IAppUserService userService, ILogger<AppUserStore> logger)
        {
            ThrowIf.Null(userService, nameof(userService));
            ThrowIf.Null(logger, nameof(logger));

            _userService = userService;
            _logger = logger;
        }

        #region IUserStore

        public Task<string> GetUserIdAsync(AppUser user, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));
            return Task.FromResult(user.Id.ToString());
        }

        public Task<string?> GetUserNameAsync(AppUser user, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));
            return Task.FromResult(user.UserName);
        }

        public Task SetUserNameAsync(AppUser user, string? userName, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public Task<string?> GetNormalizedUserNameAsync(AppUser user, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task SetNormalizedUserNameAsync(AppUser user, string? normalizedName, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }

#pragma warning disable CS8613 // Nullability of reference types in return type doesn't match implicitly implemented member.
        public async Task<AppUser?> FindByIdAsync(string id, CancellationToken cancellationToken = default)
#pragma warning restore CS8613 // Nullability of reference types in return type doesn't match implicitly implemented member.
        {
            ThrowIf.Null(id, nameof(id));
            return await _userService.FindUserByIdAsync(id, cancellationToken).ConfigureAwait(false);
        }

#pragma warning disable CS8613 // Nullability of reference types in return type doesn't match implicitly implemented member.
        public async Task<AppUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = default)
#pragma warning restore CS8613 // Nullability of reference types in return type doesn't match implicitly implemented member.
        {
            ThrowIf.Null(normalizedUserName, nameof(normalizedUserName));
            return await _userService.FindUserByUserNameAsync(normalizedUserName, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IdentityResult> CreateAsync(AppUser user, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));
            await _userService.CreateUserAsync(user, cancellationToken).ConfigureAwait(false);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateAsync(AppUser user, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));
            await _userService.UpdateUserAsync(user, cancellationToken).ConfigureAwait(false);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(AppUser user, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));
            await _userService.DeleteUserAsync(user, cancellationToken).ConfigureAwait(false);
            return IdentityResult.Success;
        }

        #endregion

        #region IUserEmailStore

        public Task<string?> GetEmailAsync(AppUser user, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));
            return Task.FromResult(user.Email);
        }

        public Task SetEmailAsync(AppUser user, string? email, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));
            user.Email = email;
            return Task.CompletedTask;
        }

        public Task<string?> GetNormalizedEmailAsync(AppUser user, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));
            return Task.FromResult(user.NormalizedEmail);
        }

        public Task SetNormalizedEmailAsync(AppUser user, string? normalizedEmail, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));
            user.NormalizedEmail = normalizedEmail;
            return Task.CompletedTask;
        }

        public Task<bool> GetEmailConfirmedAsync(AppUser user, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(AppUser user, bool confirmed, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));
            user.EmailConfirmed = confirmed;
            return Task.CompletedTask;
        }

#pragma warning disable CS8613 // Nullability of reference types in return type doesn't match implicitly implemented member.
        public async Task<AppUser?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default)
#pragma warning restore CS8613 // Nullability of reference types in return type doesn't match implicitly implemented member.
        {
            ThrowIf.Null(normalizedEmail, nameof(normalizedEmail));
            return await _userService.FindUserByEmailAsync(normalizedEmail, cancellationToken).ConfigureAwait(false);
        }

        #endregion

        #region IUserPasswordStore

        public Task<string?> GetPasswordHashAsync(AppUser user, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));
            return Task.FromResult(user.PasswordHash);
        }

        public Task SetPasswordHashAsync(AppUser user, string passwordHash, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public Task<bool> HasPasswordAsync(AppUser user, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));
            return Task.FromResult(user.HasPassword);
        }

        #endregion

        #region IUserSecurityStampStore

        public Task<string?> GetSecurityStampAsync(AppUser user, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));
            return Task.FromResult(user.SecurityStamp);
        }

        public Task SetSecurityStampAsync(AppUser user, string stamp, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));
            user.SecurityStamp = stamp;
            return Task.CompletedTask;
        }

        #endregion

        #region IUserPhoneNumberStore

        public Task<string?> GetPhoneNumberAsync(AppUser user, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));
            return Task.FromResult(user.PhoneNumber);
        }

        public Task SetPhoneNumberAsync(AppUser user, string phoneNumber, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));
            user.PhoneNumber = phoneNumber;
            return Task.CompletedTask;
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(AppUser user, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public Task SetPhoneNumberConfirmedAsync(AppUser user, bool confirmed, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));
            user.PhoneNumberConfirmed = confirmed;
            return Task.CompletedTask;
        }

        #endregion

        #region IUserTwoFactorStore

        public Task<bool> GetTwoFactorEnabledAsync(AppUser user, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));
            return Task.FromResult(user.TwoFactorEnabled);
        }

        public Task SetTwoFactorEnabledAsync(AppUser user, bool enabled, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));
            user.TwoFactorEnabled = enabled;
            return Task.CompletedTask;
        }

        #endregion

        #region IUserLockoutStore

        public Task<DateTimeOffset?> GetLockoutEndDateAsync(AppUser user, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));
            return Task.FromResult(user.LockoutEnd);
        }

        public Task SetLockoutEndDateAsync(AppUser user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));
            user.LockoutEnd = lockoutEnd;
            return Task.CompletedTask;
        }

        public Task<bool> GetLockoutEnabledAsync(AppUser user, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));
            return Task.FromResult(user.LockoutEnabled);
        }

        public Task SetLockoutEnabledAsync(AppUser user, bool enabled, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));
            user.LockoutEnabled = enabled;
            return Task.CompletedTask;
        }

        public Task<int> GetAccessFailedCountAsync(AppUser user, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<int> IncrementAccessFailedCountAsync(AppUser user, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));

            var newCount = user.AccessFailedCount + 1;
            user.AccessFailedCount = newCount;

            return Task.FromResult(newCount);
        }

        public Task ResetAccessFailedCountAsync(AppUser user, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));
            user.AccessFailedCount = 0;
            return Task.CompletedTask;
        }

        #endregion

        #region IUserAuthenticatorKeyStore

        public Task<string?> GetAuthenticatorKeyAsync(AppUser user, CancellationToken cancellationToken)
        {
            ThrowIf.Null(user, nameof(user));
            return Task.FromResult(user.AuthenticatorKey);
        }

        public Task SetAuthenticatorKeyAsync(AppUser user, string key, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));
            user.AuthenticatorKey = key;
            return Task.CompletedTask;
        }

        #endregion

        #region IUserClaimStore

        public async Task<IList<Claim>> GetClaimsAsync(AppUser user, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));
            var appClaims = await _userService.GetUserClaimsAsync(user, cancellationToken).ConfigureAwait(false);
            return appClaims.Select(appClaim => appClaim.ToClaim()).ToList();
        }

        public async Task AddClaimsAsync(AppUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));
            ThrowIf.Null(claims, nameof(claims));
            await _userService.AddUserClaimsAsync(user, claims.Select(claim =>
            {
                if (claim is null)
                    throw new ArgumentException("Null claims not allowed", nameof(claims));

                return new AppUserClaim(claim);
            }), cancellationToken).ConfigureAwait(false);
        }

        public async Task ReplaceClaimAsync(AppUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));
            ThrowIf.Null(claim, nameof(claim));
            ThrowIf.Null(newClaim, nameof(newClaim));
            await _userService.ReplaceUserClaimAsync(user, new AppUserClaim(claim), new AppUserClaim(newClaim), cancellationToken).ConfigureAwait(false);
        }

        public async Task RemoveClaimsAsync(AppUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));
            ThrowIf.Null(claims, nameof(claims));
            await _userService.RemoveClaimsAsync(user, claims.Select(claim =>
            {
                if (claim is null)
                    throw new ArgumentException("Null claims not allowed", nameof(claims));

                return new AppUserClaim(claim);
            }), cancellationToken).ConfigureAwait(false);
        }

        public async Task<IList<AppUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(claim, nameof(claim));
            var users = await _userService.GetUsersForClaimAsync(new AppUserClaim(claim), cancellationToken).ConfigureAwait(false);
            return users.ToList();
        }

        #endregion

        #region IUserLoginStore

        public async Task AddLoginAsync(AppUser user, UserLoginInfo login, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));
            ThrowIf.Null(login, nameof(login));
            await _userService.AddUserLoginAsync(user, new AppUserLogin(login), cancellationToken).ConfigureAwait(false);
        }

        public async Task RemoveLoginAsync(AppUser user, string loginProvider, string providerKey, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));
            ThrowIf.Null(loginProvider, nameof(loginProvider));
            ThrowIf.Null(providerKey, nameof(providerKey));
            await _userService.RemoveUserLoginAsync(user, loginProvider, providerKey, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(AppUser user, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));
            var logins = await _userService.GetUserLoginsAsync(user, cancellationToken).ConfigureAwait(false);
            return logins.Select(login => login.ToLoginInfo()).ToList();
        }

#pragma warning disable CS8613 // Nullability of reference types in return type doesn't match implicitly implemented member.
        public async Task<AppUser?> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken = default)
#pragma warning restore CS8613 // Nullability of reference types in return type doesn't match implicitly implemented member.
        {
            ThrowIf.Null(loginProvider, nameof(loginProvider));
            ThrowIf.Null(providerKey, nameof(providerKey));
            return await _userService.FindUserByLoginAsync(loginProvider, providerKey, cancellationToken).ConfigureAwait(false);
        }

        #endregion

        #region IUserRoleStore

        public async Task AddToRoleAsync(AppUser user, string roleName, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));
            ThrowIf.Null(roleName, nameof(roleName));
            await _userService.AddUserToRoleAsync(user, roleName, cancellationToken).ConfigureAwait(false);
        }

        public async Task RemoveFromRoleAsync(AppUser user, string roleName, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));
            ThrowIf.Null(roleName, nameof(roleName));
            await _userService.RemoveUserFromRoleAsync(user, roleName, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IList<string>> GetRolesAsync(AppUser user, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));
            var roles = await _userService.GetUserRolesAsync(user, cancellationToken).ConfigureAwait(false);
            return roles.ToList();
        }

        public async Task<bool> IsInRoleAsync(AppUser user, string roleName, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));
            ThrowIf.Null(roleName, nameof(roleName));
            return await _userService.GetUserIsInRoleAsync(user, roleName, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IList<AppUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(roleName, nameof(roleName));
            var users = await _userService.GetUsersInRoleAsync(roleName, cancellationToken).ConfigureAwait(false);
            return users.ToList();
        }

        #endregion

        #region IUserTwoFactorRecoveryCodeStore

        public async Task ReplaceCodesAsync(AppUser user, IEnumerable<string> recoveryCodes, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));
            ThrowIf.Null(recoveryCodes, nameof(recoveryCodes));
            await _userService.ReplaceUserRecoveryCodesAsync(user, recoveryCodes, cancellationToken).ConfigureAwait(false);
        }

        public async Task<bool> RedeemCodeAsync(AppUser user, string recoveryCode, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));
            ThrowIf.Null(recoveryCode, nameof(recoveryCode));
            return await _userService.RedeemUserRecoveryCodeAsync(user, recoveryCode, cancellationToken).ConfigureAwait(false);
        }

        public async Task<int> CountCodesAsync(AppUser user, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));
            return await _userService.CountUserRecoveryCodesAsync(user, cancellationToken).ConfigureAwait(false);
        }

        #endregion

        public void Dispose() { }
    }
}
