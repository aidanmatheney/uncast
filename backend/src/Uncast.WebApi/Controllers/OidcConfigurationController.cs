namespace Uncast.WebApi.Controllers
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using Uncast.Entities;
    using Uncast.Utils;

    public sealed class OidcConfigurationController : HomeControllerBase
    {
        private readonly IClientRequestParametersProvider _parametersProvider;

        public OidcConfigurationController
        (
            IClientRequestParametersProvider parametersProvider,
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            ILogger<OidcConfigurationController> logger
        ) : base
        (
            userManager,
            roleManager,
            logger
        )
        {
            ThrowIf.Null(parametersProvider, nameof(parametersProvider));

            _parametersProvider = parametersProvider;
        }

        [HttpGet("_configuration/{clientId}")]
        public IDictionary<string, string> GetOidcConfigurationClientRequestParameters([FromRoute] string clientId)
        {
            var parameters = _parametersProvider.GetClientParameters(HttpContext, clientId);
            return parameters;
        }
    }
}