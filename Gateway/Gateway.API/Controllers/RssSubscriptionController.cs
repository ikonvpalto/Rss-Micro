using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gateway.Common.Contracts;
using Gateway.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/rss-subscription")]
    public class RssSubscriptionController : ControllerBase
    {
        private readonly IRssServiceProvider _rssServiceProvider;
        private readonly IRssServiceManager _rssServiceManager;

        public RssSubscriptionController(IRssServiceProvider rssServiceProvider,
            IRssServiceManager rssServiceManager)
        {
            _rssServiceProvider = rssServiceProvider;
            _rssServiceManager = rssServiceManager;
        }

        /// <summary>Get information about specific subscription</summary>
        /// <param name="guid">subscription GUID</param>
        [HttpGet("{guid:guid}")]
        public async Task<RssSubscription> GetRssSubscriptionsAsync([FromRoute] Guid guid)
        {
            return await _rssServiceProvider.GetSubscriptionAsync(guid).ConfigureAwait(false);
        }

        /// <summary>Get information about all stored subscriptions</summary>
        [HttpGet("")]
        public async Task<ICollection<RssSubscription>> GetAllRssSubscriptionsAsync()
        {
            return await _rssServiceProvider.GetSubscriptionsAsync().ConfigureAwait(false);
        }

        /// <summary>Add new or update existing subscription on a rss source</summary>
        [HttpPost("")]
        public async Task<Guid> CreateOrUpdateSubscriptionAsync(RssSubscription rssSubscription)
        {
            return await _rssServiceManager.CreateOrUpdateSubscriptionAsync(rssSubscription).ConfigureAwait(false);
        }

        /// <summary>Delete existing subscription on a rss source</summary>
        /// <param name="rssSubscriptionGuid">subscription GUID</param>
        [HttpDelete("{rssSubscriptionGuid:guid}")]
        public async Task DeleteRssSubscriptionAsync([FromRoute] Guid rssSubscriptionGuid)
        {
            await _rssServiceManager.DeleteSubscriptionAsync(rssSubscriptionGuid).ConfigureAwait(false);
        }
    }
}
