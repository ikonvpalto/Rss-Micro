using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Downloader.Common.Contracts;
using Filter.Common.Contracts;
using Gateway.Common.Contracts;
using Gateway.Common.Models;
using Manager.Common.Contracts;
using Microsoft.Extensions.Logging;
using Sender.Common.Contracts;

namespace Gateway.Services
{
    public sealed class RssServiceProvider : IRssServiceProvider
    {
        private readonly ILogger<RssServiceProvider> _logger;
        private readonly IMapper _mapper;
        private readonly IDownloaderProvider _downloaderProvider;
        private readonly IFilterProvider _filterProvider;
        private readonly ISenderProvider _senderProvider;
        private readonly IManagerProvider _managerProvider;

        public RssServiceProvider(
            ILogger<RssServiceProvider> logger,
            IMapper mapper,
            IDownloaderProvider downloaderProvider,
            IFilterProvider filterProvider,
            ISenderProvider senderProvider,
            IManagerProvider managerProvider)
        {
            _logger = logger;
            _mapper = mapper;
            _downloaderProvider = downloaderProvider;
            _filterProvider = filterProvider;
            _senderProvider = senderProvider;
            _managerProvider = managerProvider;
        }

        public async Task<RssSubscription> GetSubscriptionAsync(Guid guid)
        {
            // Better to have separate service with full data

            var rssSource = await _downloaderProvider.GetAsync(guid).ConfigureAwait(false);
            var model = _mapper.Map<RssSubscription>(rssSource);

            var filter = await _filterProvider.GetAsync(guid).ConfigureAwait(false);
            model = _mapper.Map(filter, model);

            var receivers = await _senderProvider.GetAsync(guid).ConfigureAwait(false);
            model = _mapper.Map(receivers, model);

            var job = await _managerProvider.GetAsync(guid).ConfigureAwait(false);
            model = _mapper.Map(job, model);

            return model;
        }

        public async Task<ICollection<RssSubscription>> GetSubscriptionsAsync()
        {
            var rssSources = (await _downloaderProvider.GetAsync().ConfigureAwait(false)).ToArray();
            var rssSourcesMap = rssSources.ToDictionary(s => s.Guid, s => s);

            var filters = (await _filterProvider.GetAsync().ConfigureAwait(false)).ToArray();
            var filterMap = filters.ToDictionary(s => s.Guid, s => s);

            var receivers = (await _senderProvider.GetAsync().ConfigureAwait(false)).ToArray();
            var receiverMap = receivers.ToDictionary(s => s.Guid, s => s);

            var jobs = (await _managerProvider.GetAsync().ConfigureAwait(false)).ToArray();
            var jobMap = jobs.ToDictionary(s => s.Guid, s => s);

            var guids = rssSources.Select(s => s.Guid);
            var rssSubscriptions = guids
                .Select(g =>
                {
                    if (!rssSourcesMap.ContainsKey(g))
                    {
                        _logger.LogError("Can't find rss source with guid {Guid:D}", g);
                        return null;
                    }
                    if (!jobMap.ContainsKey(g))
                    {
                        _logger.LogError("Can't find job with guid {Guid:D}", g);
                        return null;
                    }

                    var result = _mapper.Map<RssSubscription>(rssSourcesMap[g]);

                    if (filterMap.ContainsKey(g))
                    {
                        result = _mapper.Map(filterMap[g], result);
                    }

                    if (receiverMap.ContainsKey(g))
                    {
                        result = _mapper.Map(receiverMap[g], result);
                    }

                    result = _mapper.Map(jobMap[g], result);

                    return result;
                })
                .Where(s => s != null)
                .ToArray();

            return rssSubscriptions;
        }
    }
}
