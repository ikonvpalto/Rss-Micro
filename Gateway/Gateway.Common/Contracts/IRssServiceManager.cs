using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Downloader.Common.Models;
using Gateway.Common.Models;

namespace Gateway.Common.Contracts
{
    public interface IRssServiceManager
    {
        Task<Guid> CreateOrUpdateSubscriptionAsync(RssSubscription rssSubscription);

        Task DeleteSubscriptionAsync(Guid guid);

        Task<ICollection<NewsItem>> DownloadNewsAsync();
    }
}
