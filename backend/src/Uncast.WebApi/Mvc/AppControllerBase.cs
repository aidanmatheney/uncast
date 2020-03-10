namespace Uncast.WebApi.Mvc
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using Uncast.Entities;
    using Uncast.Utils;

    public class AppControllerBase : ControllerBase
    {
        private readonly Lazy<string?> _userId;
        private readonly AsyncLazy<AppUser?> _appUser;

        protected AppControllerBase
        (
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            ILogger logger
        )
        {
            ThrowIf.Null(userManager, nameof(userManager));
            ThrowIf.Null(roleManager, nameof(roleManager));
            ThrowIf.Null(logger, nameof(logger));

            UserManager = userManager;
            RoleManager = roleManager;
            Logger = logger;

            _userId = new Lazy<string?>(LoadUserId);
            _appUser = new AsyncLazy<AppUser?>(LoadAppUserAsync);
        }

        protected UserManager<AppUser> UserManager { get; }
        protected RoleManager<AppRole> RoleManager { get; }
        protected ILogger Logger { get; }

        protected string? UserId => _userId.Value;
        protected bool UserIsAuthenticated => !(UserId is null);

        /// <summary>
        /// Gets the current user, or <see langword="null"/> if they are not authenticated.
        /// </summary>
        protected Task<AppUser?> GetAppUserAsync() => _appUser.Value;

        /// <summary>
        /// Gets the current user when the application logic should have ensured that they are authenticated.
        /// An example of this situation is when the Authorize attribute is placed on the controller.
        /// </summary>
        protected async Task<AppUser> GetAuthenticatedAppUserAsync()
        {
            var user = await GetAppUserAsync().ConfigureAwait(false);
            if (user is null)
                throw new InvalidOperationException("The user is not authenticated");

            return user;
        }

        private string? LoadUserId()
        {
            var nameClaim = User.Claims.SingleOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);
            return nameClaim?.Value;
        }

        private async Task<AppUser?> LoadAppUserAsync()
        {
            if (UserId is null)
                return null;

            var appUser = await UserManager.FindByIdAsync(UserId).ConfigureAwait(false);
            return appUser;
        }
    }
}