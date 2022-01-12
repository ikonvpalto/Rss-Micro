using System;
using System.Threading.Tasks;
using Gateway.Models;

namespace Gateway.Services
{
    public interface IRssSubscriptionManager
    {
        Task<Guid> CreateAsync(RssSubscriptionCreateModel rssSubscription);

        Task UpdateAsync(RssSubscription rssSubscription);

        Task DeleteAsync(Guid guid);
    }
}
