using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gateway.Models;

namespace Gateway.Services
{
    public interface IRssSubscriptionProvider
    {
        Task<RssSubscription> GetAsync(Guid guid);

        Task<ICollection<RssSubscription>> GetAsync();
    }
}
