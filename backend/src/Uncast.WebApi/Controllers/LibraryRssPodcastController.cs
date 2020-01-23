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

    public sealed class LibraryRssPodcastController : HomeControllerBase
    {
        private readonly IPodcastService _podcastService;

        public LibraryRssPodcastController(IPodcastService podcastService)
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
            var podcasts = await _podcastService.GetLibraryRssPodcastsAsync(cancellationToken);
            return podcasts.ToList();
        }

        /// <summary>
        ///     Get a library RSS podcast by ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<LibraryRssPodcast> GetLibraryRssPodcastById(int id, CancellationToken cancellationToken)
        {
            var podcast = await _podcastService.GetLibraryRssPodcastAsync(id, cancellationToken);
            return podcast;
        }
    }
}