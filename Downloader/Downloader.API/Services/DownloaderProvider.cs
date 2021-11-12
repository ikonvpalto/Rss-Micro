using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Downloader.API.ExternalRepositories;
using Downloader.API.ExternalServices;
using Downloader.API.Repositories;
using Downloader.Common.Contracts;
using Downloader.Common.Models;

namespace Downloader.API.Services
{
    public class DownloaderProvider : IDownloaderProvider
    {
        private readonly IRssSourceRepository _rssSourceRepository;
        private readonly IMapper _mapper;
        private readonly IRssExternalService _rssExternalService;

        public DownloaderProvider(IRssSourceRepository rssSourceRepository, IMapper mapper, IRssExternalService rssExternalService)
        {
            _rssSourceRepository = rssSourceRepository;
            _mapper = mapper;
            _rssExternalService = rssExternalService;
        }

        public async Task<RssSourceReadModel> GetAsync(Guid guid)
        {
            var rssSource = await _rssSourceRepository.GetAsync(guid).ConfigureAwait(false);
            return _mapper.Map<RssSourceReadModel>(rssSource);
        }

        public async Task<IEnumerable<RssSourceReadModel>> GetAsync()
        {
            var rssSources = await _rssSourceRepository.GetAsync().ConfigureAwait(false);
            return rssSources.Select(_mapper.Map<RssSourceReadModel>);
        }

        public async Task EnsureRssSourceIsValidAsync(string rssSourceUrl)
        {
            await _rssExternalService.EnsureCorrectRssSourceAsync(rssSourceUrl).ConfigureAwait(false);
        }
    }
}
