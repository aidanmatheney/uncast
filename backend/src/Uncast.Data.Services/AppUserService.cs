namespace Uncast.Data.Services
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using MySql.Data.MySqlClient;

    using Uncast.Data.Naming;
    using Uncast.Entities;
    using Uncast.Utils;

    public sealed class AppUserService : DbServiceBase, IAppUserService
    {
        public AppUserService(MySqlConnection dbConnection, ILogger<AppUserService> logger) : base(dbConnection, logger) { }

        #region IUserStore

        public async Task CreateUserAsync(AppUser user, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));

            await ExecuteAsync
            (
                $@"
INSERT INTO {DbTable.User}(
    Id,
    UserName,
    NormalizedUserName,
    Email,
    NormalizedEmail,
    EmailConfirmed,
    PasswordHash,
    SecurityStamp,
    ConcurrencyStamp,
    PhoneNumber,
    PhoneNumberConfirmed,
    TwoFactorEnabled,
    LockoutEnd,
    LockoutEnabled,
    AccessFailedCount,
    AuthenticatorKey
) SELECT
    @id,
    @userName,
    @normalizedUserName,
    @email,
    @normalizedEmail,
    @emailConfirmed,
    @passwordHash,
    @securityStamp,
    @concurrencyStamp,
    @phoneNumber,
    @phoneNumberConfirmed,
    @twoFactorEnabled,
    @lockoutEnd,
    @lockoutEnabled,
    @accessFailedCount,
    @authenticatorKey
;
                ",
                new
                {
                    id = user.Id,
                    userName = user.UserName,
                    normalizedUserName = user.NormalizedUserName,
                    email = user.Email,
                    normalizedEmail = user.NormalizedEmail,
                    emailConfirmed = user.EmailConfirmed,
                    passwordHash = user.PasswordHash,
                    securityStamp = user.SecurityStamp,
                    concurrencyStamp = user.ConcurrencyStamp,
                    phoneNumber = user.PhoneNumber,
                    phoneNumberConfirmed = user.PhoneNumberConfirmed,
                    twoFactorEnabled = user.TwoFactorEnabled,
                    lockoutEnd = user.LockoutEnd,
                    lockoutEnabled = user.LockoutEnabled,
                    accessFailedCount = user.AccessFailedCount,
                    authenticatorKey = user.AuthenticatorKey
                },
                cancellationToken
            ).ConfigureAwait(false);
        }

        public async Task UpdateUserAsync(AppUser user, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));

            await ExecuteAsync
            (
                $@"
UPDATE {DbTable.User} SET
    UserName = @userName,
    NormalizedUserName = @normalizedUserName,
    Email = @email,
    NormalizedEmail = @normalizedEmail,
    EmailConfirmed = @emailConfirmed,
    PasswordHash = @passwordHash,
    SecurityStamp = @securityStamp,
    ConcurrencyStamp = @concurrencyStamp,
    PhoneNumber = @phoneNumber,
    PhoneNumberConfirmed = @phoneNumberConfirmed,
    TwoFactorEnabled = @twoFactorEnabled,
    LockoutEnd = @lockoutEnd,
    LockoutEnabled = @lockoutEnabled,
    AccessFailedCount = @accessFailedCount,
    AuthenticatorKey = @authenticatorKey

    WHERE Id = @id
;
                ",
                new
                {
                    id = user.Id,
                    userName = user.UserName,
                    normalizedUserName = user.NormalizedUserName,
                    email = user.Email,
                    normalizedEmail = user.NormalizedEmail,
                    emailConfirmed = user.EmailConfirmed,
                    passwordHash = user.PasswordHash,
                    securityStamp = user.SecurityStamp,
                    concurrencyStamp = user.ConcurrencyStamp,
                    phoneNumber = user.PhoneNumber,
                    phoneNumberConfirmed = user.PhoneNumberConfirmed,
                    twoFactorEnabled = user.TwoFactorEnabled,
                    lockoutEnd = user.LockoutEnd,
                    lockoutEnabled = user.LockoutEnabled,
                    accessFailedCount = user.AccessFailedCount,
                    authenticatorKey = user.AuthenticatorKey
                },
                cancellationToken
            ).ConfigureAwait(false);
        }

        public async Task DeleteUserAsync(AppUser user, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));

            await ExecuteAsync
            (
                $@"
DELETE
    FROM {DbTable.User}
    WHERE Id = @id
;
                ",
                new { id = user.Id },
                cancellationToken
            ).ConfigureAwait(false);
        }

        public async Task<AppUser?> FindUserByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(id, nameof(id));

            return await QuerySingleOrDefaultAsync<AppUser?>
            (
                $@"
SELECT
    Id,
    UserName,
    NormalizedUserName,
    Email,
    NormalizedEmail,
    EmailConfirmed,
    PasswordHash,
    SecurityStamp,
    ConcurrencyStamp,
    PhoneNumber,
    PhoneNumberConfirmed,
    TwoFactorEnabled,
    LockoutEnd,
    LockoutEnabled,
    AccessFailedCount,
    AuthenticatorKey

    FROM {DbTable.User}
    WHERE Id = @id
;
                ",
                new { id },
                cancellationToken
            ).ConfigureAwait(false);
        }

        public async Task<AppUser?> FindUserByUserNameAsync(string normalizedUserName, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(normalizedUserName, nameof(normalizedUserName));

            return await QuerySingleOrDefaultAsync<AppUser?>
            (
                $@"
SELECT
    Id,
    UserName,
    NormalizedUserName,
    Email,
    NormalizedEmail,
    EmailConfirmed,
    PasswordHash,
    SecurityStamp,
    ConcurrencyStamp,
    PhoneNumber,
    PhoneNumberConfirmed,
    TwoFactorEnabled,
    LockoutEnd,
    LockoutEnabled,
    AccessFailedCount,
    AuthenticatorKey

    FROM {DbTable.User}
    WHERE NormalizedUserName = @normalizedUserName
;
                ",
                new { normalizedUserName },
                cancellationToken
            ).ConfigureAwait(false);
        }

        #endregion

        #region IUserEmailStore

        public async Task<AppUser?> FindUserByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(normalizedEmail, nameof(normalizedEmail));

            return await QuerySingleOrDefaultAsync<AppUser?>
            (
                $@"
SELECT
    Id,
    UserName,
    NormalizedUserName,
    Email,
    NormalizedEmail,
    EmailConfirmed,
    PasswordHash,
    SecurityStamp,
    ConcurrencyStamp,
    PhoneNumber,
    PhoneNumberConfirmed,
    TwoFactorEnabled,
    LockoutEnd,
    LockoutEnabled,
    AccessFailedCount,
    AuthenticatorKey

    FROM {DbTable.User}
    WHERE NormalizedEmail = @normalizedEmail
;
                ",
                new { normalizedEmail },
                cancellationToken
            ).ConfigureAwait(false);
        }

        #endregion

        #region IUserClaimStore

        public async Task<IEnumerable<AppUserClaim>> GetUserClaimsAsync(AppUser user, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));

            return await QueryAsync<AppUserClaim>
            (
                $@"
SELECT
    UserId,
    ClaimType,
    ClaimValue

    FROM {DbTable.UserClaim}
    WHERE UserId = @userId
;
                ",
                new { userId = user.Id },
                cancellationToken
            ).ConfigureAwait(false);
        }

        public async Task AddUserClaimsAsync(AppUser user, IEnumerable<AppUserClaim> claims, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));
            ThrowIf.Null(claims, nameof(claims));

            await using var claimsTable = await TempTableAsync(claims, table =>
            {
                table.Column("ClaimType", "varchar(256) NOT NULL", claim => claim.ClaimType);
                table.Column("ClaimValue", "nvarchar(2048) NOT NULL", claim => claim.ClaimValue);
            }, cancellationToken).ConfigureAwait(false);

            await ExecuteAsync
            (
                $@"
INSERT INTO {DbTable.UserClaim}(
    UserId,
    ClaimType,
    ClaimValue
) SELECT
    @userId,
    ClaimType,
    ClaimValue

    FROM {claimsTable.Name}
;
                ",
                new { userId = user.Id },
                cancellationToken
            ).ConfigureAwait(false);
        }

        public async Task ReplaceUserClaimAsync(AppUser user, AppUserClaim oldClaim, AppUserClaim newClaim, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));
            ThrowIf.Null(oldClaim, nameof(oldClaim));
            ThrowIf.Null(newClaim, nameof(newClaim));

            await ExecuteAsync
            (
                $@"
DELETE
    FROM {DbTable.UserClaim}
    WHERE
        UserId = @userId
        AND ClaimType = @oldClaimType
;

INSERT INTO {DbTable.UserClaim}(
    UserId,
    ClaimType,
    ClaimValue
) SELECT
    @userId,
    @newClaimType,
    @newClaimValue
;
                ",
                parameters: new
                {
                    userId = user.Id,
                    oldClaimType = oldClaim.ClaimType,
                    newClaimType = newClaim.ClaimType,
                    newClaimValue = newClaim.ClaimValue
                },
                cancellationToken
            ).ConfigureAwait(false);
        }

        public async Task RemoveClaimsAsync(AppUser user, IEnumerable<AppUserClaim> claims, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));
            ThrowIf.Null(claims, nameof(claims));

            await using var claimsTable = await TempTableAsync(claims, table =>
            {
                table.Column("ClaimType", "varchar(256) NOT NULL", claim => claim.ClaimType);
            }, cancellationToken).ConfigureAwait(false);

            await ExecuteAsync
            (
                $@"
DELETE userClaim
    FROM {claimsTable.Name} AS claim
    JOIN {DbTable.UserClaim} AS userClaim ON
        userClaim.ClaimType = claim.ClaimType

    WHERE userClaim.UserId = @userId
;
                ",
                parameters: new { userId = user.Id },
                cancellationToken
            ).ConfigureAwait(false);
        }

        public async Task<IEnumerable<AppUser>> GetUsersForClaimAsync(AppUserClaim claim, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(claim, nameof(claim));

            // TODO: Confirm whether ClaimValue should be included in WHERE
            return await QueryAsync<AppUser>
            (
                $@"
SELECT
    user.Id,
    user.UserName,
    user.NormalizedUserName,
    user.Email,
    user.NormalizedEmail,
    user.EmailConfirmed,
    user.PasswordHash,
    user.SecurityStamp,
    user.ConcurrencyStamp,
    user.PhoneNumber,
    user.PhoneNumberConfirmed,
    user.TwoFactorEnabled,
    user.LockoutEnd,
    user.LockoutEnabled,
    user.AccessFailedCount,
    user.AuthenticatorKey

    FROM {DbTable.UserClaim} AS userClaim
    JOIN {DbTable.User} AS user ON
        user.Id = userClaim.UserId

    WHERE
        userClaim.ClaimType = @claimType
        AND userClaim.ClaimValue = @claimValue
;
                ",
                new
                {
                    claimType = claim.ClaimType,
                    claimValue = claim.ClaimValue
                },
                cancellationToken
            ).ConfigureAwait(false);
        }

        #endregion

        #region IUserLoginStore

        public async Task AddUserLoginAsync(AppUser user, AppUserLogin login, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));
            ThrowIf.Null(login, nameof(login));

            await ExecuteAsync
            (
                $@"
INSERT INTO {DbTable.UserLogin}(
    UserId,
    LoginProvider,
    ProviderKey,
    ProviderDisplayName
) SELECT
    @userId,
    @loginProvider,
    @providerKey,
    @providerDisplayName
;
                ",
                new
                {
                    userId = user.Id,
                    loginProvider = login.LoginProvider,
                    providerKey = login.ProviderKey,
                    providerDisplayName = login.ProviderDisplayName
                },
                cancellationToken
            ).ConfigureAwait(false);
        }

        public async Task RemoveUserLoginAsync(AppUser user, string loginProvider, string providerKey, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));
            ThrowIf.Null(loginProvider, nameof(loginProvider));
            ThrowIf.Null(providerKey, nameof(providerKey));

            await ExecuteAsync
            (
                $@"
DELETE
    FROM {DbTable.UserLogin}
    WHERE
        UserId = @userId
        AND LoginProvider = @loginProvider
        AND ProviderKey = @providerKey
                ",
                new
                {
                    userId = user.Id,
                    loginProvider,
                    providerKey
                },
                cancellationToken
            ).ConfigureAwait(false);
        }

        public async Task<IEnumerable<AppUserLogin>> GetUserLoginsAsync(AppUser user, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));

            return await QueryAsync<AppUserLogin>
            (
                $@"
SELECT
    UserId,
    LoginProvider,
    ProviderKey,
    ProviderDisplayName

    FROM {DbTable.UserLogin}
    WHERE UserId = @userId
;
                ",
                new { userId = user.Id },
                cancellationToken
            ).ConfigureAwait(false);
        }

        public async Task<AppUser?> FindUserByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(loginProvider, nameof(loginProvider));
            ThrowIf.Null(providerKey, nameof(providerKey));

            return await QuerySingleOrDefaultAsync<AppUser?>
            (
                $@"
SELECT
    user.Id,
    user.UserName,
    user.NormalizedUserName,
    user.Email,
    user.NormalizedEmail,
    user.EmailConfirmed,
    user.PasswordHash,
    user.SecurityStamp,
    user.ConcurrencyStamp,
    user.PhoneNumber,
    user.PhoneNumberConfirmed,
    user.TwoFactorEnabled,
    user.LockoutEnd,
    user.LockoutEnabled,
    user.AccessFailedCount,
    user.AuthenticatorKey

    FROM {DbTable.User} AS user
    JOIN {DbTable.UserLogin} AS userLogin ON
        userLogin.UserId = user.Id

    WHERE
        userLogin.LoginProvider = @loginProvider
        AND userLogin.ProviderKey = @providerKey
;
                ",
                new
                {
                    loginProvider,
                    providerKey
                },
                cancellationToken
            ).ConfigureAwait(false);
        }

        #endregion

        #region IUserRoleStore

        public async Task AddUserToRoleAsync(AppUser user, string normalizedRoleName, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));
            ThrowIf.Null(normalizedRoleName, nameof(normalizedRoleName));

            await ExecuteAsync
            (
                $@"
IF NOT EXISTS(SELECT *
    FROM {DbTable.UserRole} AS userRole
    JOIN {DbTable.Role} AS role ON
        role.Id = userRole.RoleId

    WHERE
        userRole.UserId = @userId
        AND role.NormalizedName = @normalizedRoleName
) BEGIN;
    INSERT INTO {DbTable.UserRole}(
        UserId,
        RoleId
    ) SELECT
        @userId,
        role.Id

        FROM {DbTable.Role} AS role
        WHERE role.NormalizedName = @normalizedRoleName
    ;
END;
                ",
                new
                {
                    userId = user.Id,
                    normalizedRoleName
                },
                cancellationToken
            ).ConfigureAwait(false);
        }

        public async Task RemoveUserFromRoleAsync(AppUser user, string normalizedRoleName, CancellationToken cancellationToken)
        {
            ThrowIf.Null(user, nameof(user));
            ThrowIf.Null(normalizedRoleName, nameof(normalizedRoleName));

            await ExecuteAsync
            (
                $@"
DELETE
    FROM {DbTable.UserRole} AS userRole
    JOIN {DbTable.Role} AS role ON
        role.Id = userRole.RoleId

    WHERE
        userRole.UserId = @userId
        AND role.NormalizedName = @normalizedRoleName
;
                ",
                new
                {
                    userId = user.Id,
                    normalizedRoleName
                },
                cancellationToken
            ).ConfigureAwait(false);
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(AppUser user, CancellationToken cancellationToken)
        {
            ThrowIf.Null(user, nameof(user));

            return await QueryAsync<string>
            (
                $@"
SELECT
    userRole.UserId,
    role.Name

    FROM {DbTable.UserRole} AS userRole
    JOIN {DbTable.Role} AS role ON
        role.Id = userRole.RoleId

    WHERE UserId = @userId
;
                ",
                new { userId = user.Id },
                cancellationToken
            ).ConfigureAwait(false);
        }

        public async Task<bool> GetUserIsInRoleAsync(AppUser user, string normalizedRoleName, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));
            ThrowIf.Null(normalizedRoleName, nameof(normalizedRoleName));

            return await ExecuteScalarAsync<bool>
            (
                $@"
SELECT
    COUNT(*)

    FROM {DbTable.UserRole} AS userRole
    JOIN {DbTable.Role} AS role ON
        role.Id = userRole.RoleId

    WHERE
        userRole.UserId = @userId
        AND role.NormalizedName = @normalizedRoleName
;
                ",
                new
                {
                    userId = user.Id,
                    normalizedRoleName
                },
                cancellationToken
            ).ConfigureAwait(false);
        }

        public async Task<IEnumerable<AppUser>> GetUsersInRoleAsync(string normalizedRoleName, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(normalizedRoleName, nameof(normalizedRoleName));

            return await QueryAsync<AppUser>
            (
                $@"
SELECT
    user.Id,
    user.UserName,
    user.NormalizedUserName,
    user.Email,
    user.NormalizedEmail,
    user.EmailConfirmed,
    user.PasswordHash,
    user.SecurityStamp,
    user.ConcurrencyStamp,
    user.PhoneNumber,
    user.PhoneNumberConfirmed,
    user.TwoFactorEnabled,
    user.LockoutEnd,
    user.LockoutEnabled,
    user.AccessFailedCount,
    user.AuthenticatorKey

    FROM {DbTable.User} AS user
    JOIN {DbTable.UserRole} AS userRole ON
        userRole.UserId = user.Id
    JOIN {DbTable.Role} AS role ON
        role.Id = userRole.RoleId

    WHERE role.NormalizedName = @normalizedRoleName
;
                ",
                new { normalizedRoleName },
                cancellationToken
            ).ConfigureAwait(false);
        }

        #endregion

        #region IUserTwoFactorRecoveryCodeStore

        public async Task ReplaceUserRecoveryCodesAsync(AppUser user, IEnumerable<string> codes, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));
            ThrowIf.Null(codes, nameof(codes));

            await using var codesTable = await TempTableAsync(codes, table =>
            {
                table.Column("RecoveryCode", "varchar(256) NOT NULL", code => code);
            }, cancellationToken).ConfigureAwait(false);

            await ExecuteAsync
            (
                $@"
DELETE userCode
    FROM {DbTable.UserRecoveryCode} AS userCode
    JOIN {codesTable.Name} AS searchCode ON
        searchCode.RecoveryCode = userCode.RecoveryCode

    WHERE userCode.UserId = @userId
;
                ",
                new { userId = user.Id },
                cancellationToken
            ).ConfigureAwait(false);
        }

        public async Task<bool> RedeemUserRecoveryCodeAsync(AppUser user, string code, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));
            ThrowIf.Null(code, nameof(code));

            return await ExecuteScalarAsync<bool>
            (
                $@"
SET @isValid = IIF(EXISTS(SELECT *
    FROM {DbTable.UserRecoveryCode}
    WHERE
        UserId = @userId
        AND RecoveryCode = @code
), 1, 0)

IF @isValid = 1 BEGIN;
    DELETE
        FROM {DbTable.UserRecoveryCode}
        WHERE
            UserId = @userId
            AND RecoveryCode = @code
    ;
END;

SELECT @isValid;
                ",
                new
                {
                    userId = user.Id,
                    code
                },
                cancellationToken
            ).ConfigureAwait(false);
        }

        public async Task<int> CountUserRecoveryCodesAsync(AppUser user, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(user, nameof(user));

            return await ExecuteScalarAsync<int>
            (
                commandText:
                $@"
SELECT
    COUNT(*)

    FROM {DbTable.UserRecoveryCode}
    WHERE UserId = @userId
;
                ",
                new { userId = user.Id },
                cancellationToken
            ).ConfigureAwait(false);
        }

        #endregion
    }
}
