using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gateway.Models;

namespace Gateway.Services
{
    public sealed class RssSubscriptionProvider : IRssSubscriptionProvider
    {
        public Task<RssSubscription> GetAsync(Guid guid)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<RssSubscription>> GetAsync()
        {
            throw new NotImplementedException();
        }
    }
}
