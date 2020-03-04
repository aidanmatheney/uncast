namespace Uncast.WebApi.Areas.Api.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using Uncast.Entities;
    using Uncast.Data.Services;
    using Uncast.Utils;

    [Authorize]
    public sealed class LibraryRssPodcastController : ApiControllerBase
    {
        private readonly IPodcastService _podcastService;

        public LibraryRssPodcastController
        (
            IPodcastService podcastService,
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            ILogger<LibraryRssPodcastController> logger
        )
        : base
        (
            userManager,
            roleManager,
            logger
        )
        {
            ThrowIf.Null(podcastService, nameof(podcastService));

            _podcastService = podcastService;
        }

        /// <summary>
        ///     Get all library RSS podcasts.
        /// </summary>
        [HttpGet]
        public async Task<IList<LibraryRssPodcast>> GetLibraryRssPodcasts(CancellationToken cancellationToken)
        {
            var appUser = (await GetAppUserAsync())!; // Authorize attribute ensures the user is authenticated

            Logger.LogInformation("User {userEmail} requested library RSS podcasts", appUser.Email);

            var podcasts = await _podcastService.GetLibraryRssPodcastsAsync(cancellationToken);
            return podcasts.ToList();
        }

        /// <summary>
        ///     Get a library RSS podcast by ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<LibraryRssPodcast?> GetLibraryRssPodcastById(int id, CancellationToken cancellationToken)
        {
            var podcast = await _podcastService.FindLibraryRssPodcastByIdAsync(id, cancellationToken);
            return podcast;
        }
    }
}
