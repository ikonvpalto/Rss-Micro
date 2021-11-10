using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Downloader.Common.Contracts;
using Filter.Common.Contracts;
using Gateway.Models;
using Microsoft.Extensions.Logging;

namespace Gateway.Services
{
    public sealed class RssSubscriptionProvider : IRssSubscriptionProvider
    {
        private readonly ILogger<RssSubscriptionProvider> _logger;
        private readonly IMapper _mapper;
        private readonly IDownloaderProvider _downloaderProvider;
        private readonly IFilterProvider _filterProvider;

        public RssSubscriptionProvider(
            ILogger<RssSubscriptionProvider> logger,
            IMapper mapper,
            IDownloaderProvider downloaderProvider,
            IFilterProvider filterProvider)
        {
            _logger = logger;
            _mapper = mapper;
            _downloaderProvider = downloaderProvider;
            _filterProvider = filterProvider;
        }

        public async Task<RssSubscription> GetAsync(Guid guid)
        {
            // Better to have separate service with full data

            var rssSource = await _downloaderProvider.GetAsync(guid).ConfigureAwait(false);
            var filter = await _filterProvider.GetAsync(guid).ConfigureAwait(false);

            var model = _mapper.Map<RssSubscription>(rssSource);
            model = _mapper.Map(filter, model);

            return model;
        }

        public async Task<ICollection<RssSubscription>> GetAsync()
        {
            var rssSources = (await _downloaderProvider.GetAsync().ConfigureAwait(false)).ToArray();
            var rssSourcesMap = rssSources.ToDictionary(s => s.Guid, s => s);

            var filters = (await _filterProvider.GetAsync().ConfigureAwait(false)).ToArray();
            var filterMap = filters.ToDictionary(s => s.Guid, s => s);

            var guids = rssSources.Select(s => s.Guid);
            var rssSubscriptions = guids
                .Select(g =>
                {
                    if (!rssSourcesMap.ContainsKey(g))
                    {
                        _logger.LogError("Can't find rss source with guid {1:D}", g);
                        return null;
                    }
                    if (!filterMap.ContainsKey(g))
                    {
                        _logger.LogError("Can't find filter with guid {1:D}", g);
                        return null;
                    }

                    var result = _mapper.Map<RssSubscription>(rssSourcesMap[g]);
                    result = _mapper.Map(filterMap[g], result);

                    return result;
                })
                .Where(s => s != null)
                .ToArray();

            return rssSubscriptions;
        }
    }
}
