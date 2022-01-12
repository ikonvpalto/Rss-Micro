using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Common.Extensions;
using Downloader.Common.Models;
using Gateway.Common.Contracts;
using Gateway.Common.Models;

namespace Gateway.Facade.HttpProxy
{
    public sealed class RssServiceManagerProxy : IRssServiceManager
    {
        private readonly HttpClient _httpClient;

        public RssServiceManagerProxy(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Guid> CreateOrUpdateSubscriptionAsync(RssSubscription rssSubscription)
        {
            return await _httpClient.InternalPost<Guid>("/api/rss-subscription", rssSubscription).ConfigureAwait(false);
        }

        public async Task DeleteSubscriptionAsync(Guid guid)
        {
            await _httpClient.InternalDelete($"/api/rss-subscription/{guid:D}").ConfigureAwait(false);
        }

        public async Task<ICollection<NewsItem>> DownloadNewsAsync()
        {
            return await _httpClient.InternalGet<ICollection<NewsItem>>("/api/news").ConfigureAwait(false);
        }
    }
}
