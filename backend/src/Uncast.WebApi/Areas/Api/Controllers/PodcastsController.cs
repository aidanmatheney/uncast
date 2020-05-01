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
    using System;

    [Authorize]
    public sealed class PodcastsController : ApiAreaControllerBase
    {
        private readonly IPodcastService _podcastService;

        public PodcastsController
        (
            IPodcastService podcastService,
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            ILogger<PodcastsController> logger
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

        [HttpGet]
        public async Task<IList<PodcastBase>> GetAllPodcasts(CancellationToken cancellationToken)
        {
            var appUser = await GetAuthenticatedAppUserAsync().ConfigureAwait(false);
            var podcasts = await _podcastService.GetAllPodcastsAsync(appUser.Id, cancellationToken).ConfigureAwait(false);
            return podcasts.ToList();
        }

        [HttpGet("Library")]
        public async Task<IList<LibraryPodcastBase>> GetAllLibraryPodcasts(CancellationToken cancellationToken)
        {
            var podcasts = await _podcastService.GetAllLibraryPodcastsAsync(cancellationToken).ConfigureAwait(false);
            return podcasts.ToList();
        }

        [HttpGet("Library/Rss")]
        public async Task<IList<LibraryRssPodcast>> GetAllLibraryRssPodcasts(CancellationToken cancellationToken)
        {
            var podcasts = await _podcastService.GetAllLibraryRssPodcastsAsync(cancellationToken).ConfigureAwait(false);
            return podcasts.ToList();
        }

        [HttpGet("Library/YouTube")]
        public async Task<IList<LibraryYouTubePodcast>> GetAllLibraryYouTubePodcasts(CancellationToken cancellationToken)
        {
            var podcasts = await _podcastService.GetAllLibraryYouTubePodcastsAsync(cancellationToken).ConfigureAwait(false);
            return podcasts.ToList();
        }

        [HttpGet("Custom")]
        public async Task<IList<CustomPodcastBase>> GetAllCustomPodcasts(CancellationToken cancellationToken)
        {
            var appUser = await GetAuthenticatedAppUserAsync().ConfigureAwait(false);
            var podcasts = await _podcastService.GetAllCustomPodcastsAsync(appUser.Id, cancellationToken).ConfigureAwait(false);
            return podcasts.ToList();
        }

        [HttpGet("Custom/Rss")]
        public async Task<IList<CustomRssPodcast>> GetAllCustomRssPodcasts(CancellationToken cancellationToken)
        {
            var appUser = await GetAuthenticatedAppUserAsync().ConfigureAwait(false);
            var podcasts = await _podcastService.GetAllCustomRssPodcastsAsync(appUser.Id, cancellationToken).ConfigureAwait(false);
            return podcasts.ToList();
        }

        [HttpGet("Custom/YouTube")]
        public async Task<IList<CustomYouTubePodcast>> GetAllCustomYouTubePodcasts(CancellationToken cancellationToken)
        {
            var appUser = await GetAuthenticatedAppUserAsync().ConfigureAwait(false);
            var podcasts = await _podcastService.GetAllCustomYouTubePodcastsAsync(appUser.Id, cancellationToken).ConfigureAwait(false);
            return podcasts.ToList();
        }

        [HttpGet("Custom/File")]
        public async Task<IList<CustomFilePodcast>> GetAllCustomFilePodcasts(CancellationToken cancellationToken)
        {
            var appUser = await GetAuthenticatedAppUserAsync().ConfigureAwait(false);
            var podcasts = await _podcastService.GetAllCustomFilePodcastsAsync(appUser.Id, cancellationToken).ConfigureAwait(false);
            return podcasts.ToList();
        }

        [HttpGet("{id}")]
        public async Task<PodcastBase?> FindPodcastById(Guid id, CancellationToken cancellationToken)
        {
            var appUser = await GetAuthenticatedAppUserAsync().ConfigureAwait(false);

            var podcast = await _podcastService.FindPodcastByIdAsync(id, cancellationToken).ConfigureAwait(false);
            if (podcast is CustomPodcastBase customPodcast && customPodcast.UserId != appUser.Id)
                return null;

            return podcast;
        }

        // TODO: figure out how to consolidate with FindPodcastById
        [HttpGet("Library/Rss/{id}")]
        public async Task<PodcastBase?> FindLibraryRssPodcastById(Guid id, CancellationToken cancellationToken)
        {
            var appUser = await GetAuthenticatedAppUserAsync().ConfigureAwait(false);

            var podcast = await _podcastService.FindPodcastByIdAsync(id, cancellationToken).ConfigureAwait(false);
            if (podcast is CustomPodcastBase customPodcast && customPodcast.UserId != appUser.Id)
                return null;

            return podcast;
        }

        [HttpPost("Library/Rss")]
        public async Task<Guid> CreateLibraryRssPodcast(LibraryRssPodcast podcast, CancellationToken cancellationToken)
        {
            ThrowIf.Null(podcast, nameof(podcast));

            var appUser = await GetAuthenticatedAppUserAsync().ConfigureAwait(false);
            // TODO: Ensure user has permission

            var id = Guid.NewGuid();

            podcast.Id = id;
            await _podcastService.CreatePodcastAsync(podcast, cancellationToken).ConfigureAwait(false);

            return id;
        }

        [HttpPost("Library/YouTube")]
        public async Task<Guid> CreateLibraryYouTubePodcast(LibraryYouTubePodcast podcast, CancellationToken cancellationToken)
        {
            ThrowIf.Null(podcast, nameof(podcast));

            var appUser = await GetAuthenticatedAppUserAsync().ConfigureAwait(false);
            // TODO: Ensure user has permission

            var id = Guid.NewGuid();

            podcast.Id = id;
            await _podcastService.CreatePodcastAsync(podcast, cancellationToken).ConfigureAwait(false);

            return id;
        }

        [HttpPut("Library/Rss")]
        public async Task UpdateLibraryRssPodcast(LibraryRssPodcast podcast, CancellationToken cancellationToken)
        {
            ThrowIf.Null(podcast, nameof(podcast));

            var appUser = await GetAuthenticatedAppUserAsync().ConfigureAwait(false);
            // TODO: Ensure user has permission

            await _podcastService.UpdatePodcastAsync(podcast, cancellationToken).ConfigureAwait(false);
        }

        [HttpPut("Library/YouTube")]
        public async Task UpdateLibraryYouTubePodcast(LibraryYouTubePodcast podcast, CancellationToken cancellationToken)
        {
            ThrowIf.Null(podcast, nameof(podcast));

            var appUser = await GetAuthenticatedAppUserAsync().ConfigureAwait(false);
            // TODO: Ensure user has permission

            await _podcastService.UpdatePodcastAsync(podcast, cancellationToken).ConfigureAwait(false);
        }
    }
}
