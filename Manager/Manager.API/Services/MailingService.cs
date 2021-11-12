using System;
using System.Threading.Tasks;
using Downloader.Common.Contracts;
using Filter.Common.Contracts;
using Sender.Common.Contracts;

namespace Manager.API.Services
{
    public sealed class MailingService : IMailingService
    {
        private readonly IDownloaderManager _downloaderManager;
        private readonly IFilterProvider _filterProvider;
        private readonly ISenderManager _senderManager;

        public MailingService(
            IDownloaderManager downloaderManager,
            IFilterProvider filterProvider,
            ISenderManager senderManager)
        {
            _downloaderManager = downloaderManager;
            _filterProvider = filterProvider;
            _senderManager = senderManager;
        }

        public async Task SendNewsAsync(Guid jobId)
        {
            var news = await _downloaderManager.DownloadNewsAsync(jobId).ConfigureAwait(false);
            news = await _filterProvider.FilterNewsAsync(jobId, news).ConfigureAwait(false);
            await _senderManager.SendNewsAsync(jobId, news).ConfigureAwait(false);
        }
    }
}
