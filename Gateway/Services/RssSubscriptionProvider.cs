using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gateway.Models;

namespace Gateway.Services
{
    public sealed class RssSubscriptionProvider : IRssSubscriptionProvider
    {
        public Task<RssSubscription> Get(Guid guid)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<RssSubscription>> Get()
        {
            throw new NotImplementedException();
        }
    }
}