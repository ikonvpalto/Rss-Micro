using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gateway.Common.Models;

namespace Gateway.Common.Contracts
{
    public interface IRssServiceProvider
    {
        Task<RssSubscription> GetSubscriptionAsync(Guid guid);

        Task<ICollection<RssSubscription>> GetSubscriptionsAsync();
    }
}
