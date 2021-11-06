using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Common.Exceptions;
using Downloader.API.ExternalRepositories;
using Downloader.API.Models;
using Downloader.API.Repositories;
using Downloader.API.Resources;
using Downloader.Common.Contracts;
using Downloader.Common.Models;

namespace Downloader.API.Services
{
    public class DownloaderManager : IDownloaderManager
    {
        private readonly IRssSourceRepository _rssSourceRepository;
        private readonly IMapper _mapper;
        private readonly IRssExternalRepository _rssExternalRepository;

        public DownloaderManager(
            IRssSourceRepository rssSourceRepository,
            IMapper mapper,
            IRssExternalRepository rssExternalRepository)
        {
            _rssSourceRepository = rssSourceRepository;
            _mapper = mapper;
            _rssExternalRepository = rssExternalRepository;
        }

        public async Task CreateAsync(RssSourceManageModel rssSource)
        {
            if (await _rssSourceRepository.IsRssSourceExistsAsync(rssSource.Guid).ConfigureAwait(false))
            {
                throw new AlreadyExistsException(Localization.RssSourceAlreadyExists);
            }

            await _rssExternalRepository.EnsureCorrectRssSourceAsync(rssSource.Url).ConfigureAwait(false);

            var model = _mapper.Map<RssSource>(rssSource);
            model.LastSuccessDownloading = DateTime.MinValue;

            await _rssSourceRepository.CreateAsync(model).ConfigureAwait(false);
        }

        public async Task UpdateAsync(RssSourceManageModel rssSource)
        {
            if (!await _rssSourceRepository.IsRssSourceExistsAsync(rssSource.Guid).ConfigureAwait(false))
            {
                throw new NotFoundException(Localization.RssSourceNotFound);
            }

            await _rssExternalRepository.EnsureCorrectRssSourceAsync(rssSource.Url).ConfigureAwait(false);

            var model = await _rssSourceRepository.GetAsync(rssSource.Guid).ConfigureAwait(false);
            model = _mapper.Map(rssSource, model);

            await _rssSourceRepository.UpdateAsync(model).ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid rssSourceGuid)
        {
            if (!await _rssSourceRepository.IsRssSourceExistsAsync(rssSourceGuid).ConfigureAwait(false))
            {
                throw new NotFoundException(Localization.RssSourceNotFound);
            }

            await _rssSourceRepository.DeleteAsync(rssSourceGuid).ConfigureAwait(false);
        }

        public async Task<IEnumerable<NewsItem>> DownloadNewsAsync(Guid rssSourceGuid)
        {
            if (!await _rssSourceRepository.IsRssSourceExistsAsync(rssSourceGuid).ConfigureAwait(false))
            {
                throw new NotFoundException(Localization.RssSourceNotFound);
            }

            var rssSource = await _rssSourceRepository.GetAsync(rssSourceGuid).ConfigureAwait(false);

            var news = await _rssExternalRepository.RequestRssSourceAsync(rssSource.Url).ConfigureAwait(false);
            var result = news.Select(_mapper.Map<NewsItem>);

            rssSource.LastSuccessDownloading = DateTime.Today;
            await _rssSourceRepository.UpdateAsync(rssSource).ConfigureAwait(false);

            return result;
        }
    }
}
