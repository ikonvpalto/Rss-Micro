using System;
using System.Threading.Tasks;
using AutoMapper;
using Downloader.Common.Contracts;
using Downloader.Common.Models;
using Gateway.Models;

namespace Gateway.Services
{
    public sealed class RssSubscriptionManager : IRssSubscriptionManager
    {
        private readonly IMapper _mapper;
        private readonly IDownloaderProvider _downloaderProvider;
        private readonly IDownloaderManager _downloaderManager;

        public RssSubscriptionManager(IMapper mapper, IDownloaderProvider downloaderProvider, IDownloaderManager downloaderManager)
        {
            _mapper = mapper;
            _downloaderProvider = downloaderProvider;
            _downloaderManager = downloaderManager;
        }

        public async Task<Guid> CreateAsync(RssSubscriptionCreateModel rssSubscription)
        {
            await EnsureSubscriptionInfoValidAsync(rssSubscription).ConfigureAwait(false);

            var guid = Guid.NewGuid();

            var rssSource = _mapper.Map<RssSourceManageModel>(rssSubscription);
            rssSource.Guid = guid;
            await _downloaderManager.CreateAsync(rssSource).ConfigureAwait(false);

            return guid;
        }

        public async Task UpdateAsync(RssSubscription rssSubscription)
        {
            await EnsureSubscriptionInfoValidAsync(rssSubscription).ConfigureAwait(false);

            var rssSource = _mapper.Map<RssSourceManageModel>(rssSubscription);
            await _downloaderManager.UpdateAsync(rssSource).ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid guid)
        {
            await _downloaderManager.DeleteAsync(guid).ConfigureAwait(false);
        }

        private async Task EnsureSubscriptionInfoValidAsync(BaseRssSubscription rssSubscription)
        {
            await _downloaderProvider.EnsureRssSourceValidAsync(rssSubscription.RssSource).ConfigureAwait(false);
        }
    }
}
