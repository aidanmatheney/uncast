namespace Uncast.WebApi.Areas.Api.Controllers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using Uncast.Data.Services;
    using Uncast.Entities;
    using Uncast.Utils;

    [Authorize]
    public sealed class UserStateController : ApiAreaControllerBase
    {
        private readonly IUserStateService _userStateService;

        public UserStateController
        (
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            ILogger logger,
            IUserStateService userStateService
        ) : base
        (
            userManager,
            roleManager,
            logger
        )
        {
            ThrowIf.Null(userStateService, nameof(userStateService));

            _userStateService = userStateService;
        }

        [HttpGet("App")]
        public async Task<UserAppState?> GetAppState(CancellationToken cancellationToken)
        {
            var user = await GetAuthenticatedAppUserAsync().ConfigureAwait(false);
            var state = await _userStateService.GetAppStateAsync(user.Id, cancellationToken).ConfigureAwait(false);
            return state;
        }

        [HttpPut("App")]
        public async Task UpdateAppState(UserAppState state, CancellationToken cancellationToken)
        {
            ThrowIf.Null(state, nameof(state));

            var user = await GetAuthenticatedAppUserAsync().ConfigureAwait(false);
            state.UserId = user.Id;
            await _userStateService.UpdateAppStateAsync(state, cancellationToken).ConfigureAwait(false);
        }

        [HttpGet("Podcasts/{podcastId}")]
        public async Task<UserPodcastState?> GetPodcastState(Guid podcastId, CancellationToken cancellationToken)
        {
            var user = await GetAuthenticatedAppUserAsync().ConfigureAwait(false);
            var state = await _userStateService.GetPodcastStateAsync(user.Id, podcastId, cancellationToken).ConfigureAwait(false);
            return state;
        }

        [HttpPut("Podcasts/{podcastId}")]
        public async Task UpdatePodcastState(Guid podcastId, UserPodcastState state, CancellationToken cancellationToken)
        {
            ThrowIf.Null(state, nameof(state));

            var user = await GetAuthenticatedAppUserAsync().ConfigureAwait(false);
            state.UserId = user.Id;
            state.PodcastId = podcastId;
            await _userStateService.UpdatePodcastStateAsync(state, cancellationToken).ConfigureAwait(false);
        }

        [HttpGet("Episodes/{episodeId}")]
        public async Task<UserPodcastEpisodeState?> GetEpisodeState(Guid episodeId, CancellationToken cancellationToken)
        {
            var user = await GetAuthenticatedAppUserAsync().ConfigureAwait(false);
            var state = await _userStateService.GetEpisodeStateAsync(user.Id, episodeId, cancellationToken).ConfigureAwait(false);
            return state;
        }

        [HttpPut("Episodes/{episodeId}")]
        public async Task UpdateEpisodeState(Guid episodeId, UserPodcastEpisodeState state, CancellationToken cancellationToken)
        {
            ThrowIf.Null(state, nameof(state));

            var user = await GetAuthenticatedAppUserAsync().ConfigureAwait(false);
            state.UserId = user.Id;
            state.EpisodeId = episodeId;
            await _userStateService.UpdateEpisodeStateAsync(state, cancellationToken).ConfigureAwait(false);
        }

        [HttpGet("Queue")]
        public async Task<UserPodcastPlaybackQueue?> GetQueueAsync(CancellationToken cancellationToken)
        {
            var user = await GetAuthenticatedAppUserAsync().ConfigureAwait(false);
            var queue = await _userStateService.GetPlaybackQueueAsync(user.Id, cancellationToken).ConfigureAwait(false);
            return queue;
        }

        [HttpPost("Queue")]
        public async Task UpdateQueueAsync(UserPodcastPlaybackQueue queue, CancellationToken cancellationToken)
        {
            ThrowIf.Null(queue, nameof(queue));

            var user = await GetAuthenticatedAppUserAsync().ConfigureAwait(false);
            queue.UserId = user.Id;
            await _userStateService.UpdatePlaybackQueueAsync(queue, cancellationToken).ConfigureAwait(false);
        }
    }
}