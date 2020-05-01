namespace Uncast.WebApi.Areas.Api.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using Uncast.Entities;
    using Uncast.WebApi.Mvc;

    [ApiController]
    [Area("Api")]
    [Route("/[area]/[controller]")]
    public abstract class ApiAreaControllerBase : AppControllerBase
    {
        protected ApiAreaControllerBase(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, ILogger logger) : base(userManager, roleManager, logger) { }
    }
}