using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gateway.Models;
using Gateway.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Gateway.Controllers
{
    [ApiController]
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

        [HttpGet("")]
        public async Task<ICollection<RssSubscription>> GetAllRssSubscriptions()
        {
            return await _rssSubscriptionProvider.Get().ConfigureAwait(false);
        }

        [HttpPost("")]
        public async Task<RssSubscription> AddNewRssSubscription(RssSubscriptionCreateModel rssSubscription)
        {
            var guid = await _rssSubscriptionManager.Add(rssSubscription).ConfigureAwait(false);
            return await _rssSubscriptionProvider.Get(guid).ConfigureAwait(false);
        }

        [HttpPut("")]
        public async Task<RssSubscription> UpdateRssSubscription(RssSubscription rssSubscription)
        {
            await _rssSubscriptionManager.Update(rssSubscription).ConfigureAwait(false);
            return await _rssSubscriptionProvider.Get(rssSubscription.Guid).ConfigureAwait(false);
        }

        [HttpDelete("")]
        public async Task DeleteRssSubscription(Guid rssSubscriptionGuid)
        {
            await _rssSubscriptionManager.Delete(rssSubscriptionGuid).ConfigureAwait(false);
        }
    }
}
