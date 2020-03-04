namespace Uncast.WebApi.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using Uncast.Entities;
    using Uncast.WebApi.Mvc;

    [ApiController]
    public abstract class HomeControllerBase : AppControllerBase
    {
        protected HomeControllerBase(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, ILogger logger) : base(userManager, roleManager, logger) { }
    }
}