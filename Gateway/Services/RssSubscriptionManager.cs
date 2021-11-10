using System;
using System.Threading.Tasks;
using AutoMapper;
using Downloader.Common.Contracts;
using Downloader.Common.Models;
using Filter.Common.Contracts;
using Filter.Common.Models;
using Gateway.Models;

namespace Gateway.Services
{
    public sealed class RssSubscriptionManager : IRssSubscriptionManager
    {
        private readonly IMapper _mapper;
        private readonly IDownloaderProvider _downloaderProvider;
        private readonly IDownloaderManager _downloaderManager;
        private readonly IFilterProvider _filterProvider;
        private readonly IFilterManager _filterManager;

        public RssSubscriptionManager(
            IMapper mapper,
            IDownloaderProvider downloaderProvider,
            IDownloaderManager downloaderManager,
            IFilterProvider filterProvider,
            IFilterManager filterManager)
        {
            _mapper = mapper;
            _downloaderProvider = downloaderProvider;
            _downloaderManager = downloaderManager;
            _filterProvider = filterProvider;
            _filterManager = filterManager;
        }

        public async Task<Guid> CreateAsync(RssSubscriptionCreateModel rssSubscription)
        {
            await EnsureSubscriptionInfoValidAsync(rssSubscription).ConfigureAwait(false);

            var guid = Guid.NewGuid();

            var rssSource = _mapper.Map<RssSourceManageModel>(rssSubscription);
            rssSource.Guid = guid;
            await _downloaderManager.CreateAsync(rssSource).ConfigureAwait(false);

            var filter = _mapper.Map<NewsFilterModel>(rssSubscription);
            filter.Guid = guid;
            await _filterManager.CreateAsync(filter).ConfigureAwait(false);

            return guid;
        }

        public async Task UpdateAsync(RssSubscription rssSubscription)
        {
            await EnsureSubscriptionInfoValidAsync(rssSubscription).ConfigureAwait(false);

            var rssSource = _mapper.Map<RssSourceManageModel>(rssSubscription);
            await _downloaderManager.UpdateAsync(rssSource).ConfigureAwait(false);

            var filter = _mapper.Map<NewsFilterModel>(rssSubscription);
            await _filterManager.UpdateAsync(filter).ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid guid)
        {
            await _downloaderManager.DeleteAsync(guid).ConfigureAwait(false);
            await _filterManager.DeleteAsync(guid).ConfigureAwait(false);
        }

        private async Task EnsureSubscriptionInfoValidAsync(BaseRssSubscription rssSubscription)
        {
            await Task.WhenAll(
                _downloaderProvider.EnsureRssSourceValidAsync(rssSubscription.RssSource),
                _filterProvider.EnsureFiltersValidAsync(rssSubscription.Filters)
            ).ConfigureAwait(false);
        }
    }
}
