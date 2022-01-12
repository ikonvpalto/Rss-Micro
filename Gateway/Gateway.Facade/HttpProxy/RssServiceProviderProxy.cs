using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Common.Extensions;
using Gateway.Common.Contracts;
using Gateway.Common.Models;

namespace Gateway.Facade.HttpProxy
{
    public sealed class RssServiceProviderProxy : IRssServiceProvider
    {
        private readonly HttpClient _httpClient;

        public RssServiceProviderProxy(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<RssSubscription> GetSubscriptionAsync(Guid guid)
        {
            return await _httpClient.InternalGet<RssSubscription>($"/api/rss-subscription/{guid:D}").ConfigureAwait(false);
        }

        public async Task<ICollection<RssSubscription>> GetSubscriptionsAsync()
        {
            return await _httpClient.InternalGet<ICollection<RssSubscription>>("/api/rss-subscription").ConfigureAwait(false);
        }
    }
}
