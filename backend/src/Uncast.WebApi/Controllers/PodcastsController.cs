namespace Uncast.WebApi.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using Uncast.Data.Entities;
    using Uncast.Data.Services;
    using Uncast.Utils;

    [ApiController]
    [Route("/api/[controller]")]
    public sealed class PodcastsController : ControllerBase
    {
        private readonly IPodcastService _podcastService;

        public PodcastsController(IPodcastService podcastService)
        {
            ThrowIf.Null(podcastService, nameof(podcastService));

            _podcastService = podcastService;
        }

        /// <summary>
        ///     Get all library RSS podcasts.
        /// </summary>
        [HttpGet("LibraryRssPodcasts")]
        public async Task<IList<LibraryRssPodcast>> GetLibraryRssPodcastsAsync(CancellationToken cancellationToken)
        {
            var podcasts = await _podcastService.GetLibraryRssPodcastsAsync(cancellationToken);
            return podcasts.ToList();
        }

        /// <summary>
        ///     Get a library RSS podcast by ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>The podcast, or null.</returns>
        [HttpGet("LibraryRssPodcast")]
        public async Task<LibraryRssPodcast> GetLibraryRssPodcastAsync(int id, CancellationToken cancellationToken)
        {
            var podcast = await _podcastService.GetLibraryRssPodcastAsync(id, cancellationToken);
            return podcast;
        }
    }
}