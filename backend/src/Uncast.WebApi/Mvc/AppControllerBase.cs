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
        protected Task<AppUser?> GetAppUserAsync() => _appUser.Value;

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