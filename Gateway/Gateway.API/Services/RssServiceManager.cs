using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Downloader.Common.Contracts;
using Downloader.Common.Models;
using Filter.Common.Contracts;
using Filter.Common.Models;
using Gateway.Common.Contracts;
using Gateway.Common.Models;
using Manager.Common.Contracts;
using Manager.Common.Models;
using Sender.Common.Contracts;
using Sender.Common.Models;

namespace Gateway.Services
{
    public sealed class RssServiceManager : IRssServiceManager
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

        public RssServiceManager(
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

        public async Task<Guid> CreateOrUpdateSubscriptionAsync(RssSubscription rssSubscription)
        {
            await EnsureSubscriptionInfoValidAsync(rssSubscription).ConfigureAwait(false);

            return rssSubscription.Guid == Guid.Empty
                ? await CreateSubscriptionAsync(rssSubscription).ConfigureAwait(false)
                : await UpdateSubscriptionAsync(rssSubscription).ConfigureAwait(false);
        }

        public async Task DeleteSubscriptionAsync(Guid guid)
        {
            await Task.WhenAll(
                _downloaderManager.DeleteAsync(guid),
                _filterManager.DeleteAsync(guid),
                _senderManager.DeleteAsync(guid),
                _managerManager.DeleteAsync(guid));
        }

        public async Task<ICollection<NewsItem>> DownloadNewsAsync()
        {
            var rssSources = await _downloaderProvider.GetAsync().ConfigureAwait(false);
            var guids = rssSources.Select(s => s.Guid);

            var allNews = new List<NewsItem>();
            foreach (var guid in guids)
            {
                var news = await _downloaderManager.DownloadNewsAsync(guid).ConfigureAwait(false);
                allNews.AddRange(await _filterProvider.FilterNewsAsync(guid, news).ConfigureAwait(false));
            }

            return allNews
                .OrderBy(n => n.PublishDate)
                .ThenBy(n => n.Title)
                .ToArray();
        }

        private async Task EnsureSubscriptionInfoValidAsync(RssSubscription rssSubscription)
        {
            var validationTasks = new List<Task>
            {
                _downloaderProvider.EnsureRssSourceIsValidAsync(rssSubscription.RssSource),
                _filterProvider.EnsureFiltersIsValidAsync(rssSubscription.Filters)
            };

            if (rssSubscription.NeedToSendEmails)
            {
                validationTasks.AddRange(new[]
                {
                    _senderProvider.EnsureReceiversIsValidAsync(rssSubscription.Receivers),
                    _managerProvider.EnsureJobPeriodicityIsValidAsync(rssSubscription.Periodicity)
                });
            }

            await Task.WhenAll(validationTasks).ConfigureAwait(false);
        }

        private async Task<Guid> CreateSubscriptionAsync(RssSubscription rssSubscription)
        {
            rssSubscription.Guid = Guid.NewGuid();

            var rssSource = _mapper.Map<RssSourceManageModel>(rssSubscription);
            await _downloaderManager.CreateAsync(rssSource).ConfigureAwait(false);

            var filter = _mapper.Map<NewsFilterModel>(rssSubscription);
            await _filterManager.CreateAsync(filter).ConfigureAwait(false);

            var receivers = _mapper.Map<ReceiversModel>(rssSubscription);
            await _senderManager.CreateAsync(receivers).ConfigureAwait(false);

            var job = _mapper.Map<JobModel>(rssSubscription);
            await _managerManager.CreateAsync(job).ConfigureAwait(false);

            return rssSource.Guid;
        }

        private async Task<Guid> UpdateSubscriptionAsync(RssSubscription rssSubscription)
        {
            var rssSource = _mapper.Map<RssSourceManageModel>(rssSubscription);
            await _downloaderManager.UpdateAsync(rssSource).ConfigureAwait(false);

            var filter = _mapper.Map<NewsFilterModel>(rssSubscription);
            await _filterManager.UpdateAsync(filter).ConfigureAwait(false);

            var receivers = _mapper.Map<ReceiversModel>(rssSubscription);
            await _senderManager.UpdateAsync(receivers).ConfigureAwait(false);

            var job = _mapper.Map<JobModel>(rssSubscription);
            await _managerManager.UpdateAsync(job).ConfigureAwait(false);

            return rssSource.Guid;
        }
    }
}
