using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gateway.Models;
using Gateway.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Gateway.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/rss-subscription")]
    public class RssSubscriptionController : ControllerBase
    {
        private readonly ILogger<RssSubscriptionController> _logger;
        private readonly IRssSubscriptionProvider _rssSubscriptionProvider;
        private readonly IRssSubscriptionManager _rssSubscriptionManager;

        public RssSubscriptionController(
            ILogger<RssSubscriptionController> logger,
            IRssSubscriptionProvider rssSubscriptionProvider,
            IRssSubscriptionManager rssSubscriptionManager)
        {
            _logger = logger;
            _rssSubscriptionProvider = rssSubscriptionProvider;
            _rssSubscriptionManager = rssSubscriptionManager;
        }

        /// <summary>Get information about all stored subscriptions</summary>
        [HttpGet("")]
        public async Task<ICollection<RssSubscription>> GetAllRssSubscriptions()
        {
            return await _rssSubscriptionProvider.GetAsync().ConfigureAwait(false);
        }

        /// <summary>Add new subscription on a rss source</summary>
        [HttpPost("")]
        public async Task<RssSubscription> AddNewRssSubscription(RssSubscriptionCreateModel rssSubscription)
        {
            var guid = await _rssSubscriptionManager.CreateAsync(rssSubscription).ConfigureAwait(false);
            return await _rssSubscriptionProvider.GetAsync(guid).ConfigureAwait(false);
        }

        /// <summary>Update existing subscription on a rss source</summary>
        [HttpPut("")]
        public async Task<RssSubscription> UpdateRssSubscription(RssSubscription rssSubscription)
        {
            await _rssSubscriptionManager.UpdateAsync(rssSubscription).ConfigureAwait(false);
            return await _rssSubscriptionProvider.GetAsync(rssSubscription.Guid).ConfigureAwait(false);
        }

        /// <summary>Delete existing subscription on a rss source</summary>
        /// <param name="rssSubscriptionGuid">subscription GUID</param>
        [HttpDelete("{rssSubscriptionGuid:guid}")]
        public async Task DeleteRssSubscription([FromRoute] Guid rssSubscriptionGuid)
        {
            await _rssSubscriptionManager.DeleteAsync(rssSubscriptionGuid).ConfigureAwait(false);
        }
    }
}
