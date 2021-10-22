using System;
using System.Threading.Tasks;
using Gateway.Models;

namespace Gateway.Services
{
    public sealed class RssSubscriptionManager : IRssSubscriptionManager
    {
        public Task<Guid> Add(RssSubscriptionCreateModel rssSubscription)
        {
            throw new NotImplementedException();
        }

        public Task Update(RssSubscription rssSubscription)
        {
            throw new NotImplementedException();
        }

        public Task Delete(Guid guid)
        {
            throw new NotImplementedException();
        }
    }
}
