using System;
using System.Threading.Tasks;
using Gateway.Models;

namespace Gateway.Services
{
    public interface IRssSubscriptionManager
    {
        Task<Guid> Add(RssSubscriptionCreateModel rssSubscription);

        Task Update(RssSubscription rssSubscription);

        Task Delete(Guid guid);
    }
}
