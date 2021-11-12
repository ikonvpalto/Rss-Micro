using System;
using System.Threading.Tasks;
using AutoMapper;
using Downloader.Common.Contracts;
using Downloader.Common.Models;
using Filter.Common.Contracts;
using Filter.Common.Models;
using Gateway.Models;
using Manager.Common.Contracts;
using Manager.Common.Models;
using Sender.Common.Contracts;
using Sender.Common.Models;

namespace Gateway.Services
{
    public sealed class RssSubscriptionManager : IRssSubscriptionManager
    {
        private readonly IMapper _mapper;
        private readonly IDownloaderProvider _downloaderProvider;
        private readonly IDownloaderManager _downloaderManager;
        private readonly IFilterProvider _filterProvider;
        private readonly IFilterManager _filterManager;
        private readonly ISenderProvider _senderProvider;
        private readonly ISenderManager _senderManager;
        private readonly IManagerProvider _managerProvider;
        private readonly IManagerManager _managerManager;

        public RssSubscriptionManager(
            IMapper mapper,
            IDownloaderProvider downloaderProvider,
            IDownloaderManager downloaderManager,
            IFilterProvider filterProvider,
            IFilterManager filterManager,
            ISenderProvider senderProvider,
            ISenderManager senderManager,
            IManagerProvider managerProvider,
            IManagerManager managerManager)
        {
            _mapper = mapper;
            _downloaderProvider = downloaderProvider;
            _downloaderManager = downloaderManager;
            _filterProvider = filterProvider;
            _filterManager = filterManager;
            _senderProvider = senderProvider;
            _senderManager = senderManager;
            _managerProvider = managerProvider;
            _managerManager = managerManager;
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

            var receivers = _mapper.Map<ReceiversModel>(rssSubscription);
            receivers.Guid = guid;
            await _senderManager.CreateAsync(receivers).ConfigureAwait(false);

            var job = _mapper.Map<JobModel>(rssSubscription);
            job.Guid = guid;
            await _managerManager.CreateAsync(job).ConfigureAwait(false);

            return guid;
        }

        public async Task UpdateAsync(RssSubscription rssSubscription)
        {
            await EnsureSubscriptionInfoValidAsync(rssSubscription).ConfigureAwait(false);

            var rssSource = _mapper.Map<RssSourceManageModel>(rssSubscription);
            await _downloaderManager.UpdateAsync(rssSource).ConfigureAwait(false);

            var filter = _mapper.Map<NewsFilterModel>(rssSubscription);
            await _filterManager.UpdateAsync(filter).ConfigureAwait(false);

            var receivers = _mapper.Map<ReceiversModel>(rssSubscription);
            await _senderManager.UpdateAsync(receivers).ConfigureAwait(false);

            var job = _mapper.Map<JobModel>(rssSubscription);
            await _managerManager.UpdateAsync(job).ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid guid)
        {
            await Task.WhenAll(
                _downloaderManager.DeleteAsync(guid),
                _filterManager.DeleteAsync(guid),
                _senderManager.DeleteAsync(guid),
                _managerManager.DeleteAsync(guid));
        }

        private async Task EnsureSubscriptionInfoValidAsync(BaseRssSubscription rssSubscription)
        {
            await Task.WhenAll(
                _downloaderProvider.EnsureRssSourceIsValidAsync(rssSubscription.RssSource),
                _filterProvider.EnsureFiltersIsValidAsync(rssSubscription.Filters),
                _senderProvider.EnsureReceiversIsValidAsync(rssSubscription.Receivers),
                _managerProvider.EnsureJobPeriodicityIsValidAsync(rssSubscription.Periodicity)
            ).ConfigureAwait(false);
        }
    }
}
