using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Common.Exceptions;
using Downloader.Common.Models;
using Filter.API.Repositories;
using Filter.API.Resources;
using Filter.Common.Contracts;
using Filter.Common.Models;

namespace Filter.API.Services
{
    public sealed class FilterProvider : IFilterProvider
    {
        private readonly IMapper _mapper;
        private readonly IFilterRepository _filterRepository;

        public FilterProvider(IMapper mapper, IFilterRepository filterRepository)
        {
            _mapper = mapper;
            _filterRepository = filterRepository;
        }

        public async Task<NewsFilterModel> GetAsync(Guid filterGuid)
        {
            var models = await _filterRepository.GetGroupAsync(filterGuid);
            return _mapper.Map<NewsFilterModel>(models);
        }

        public async Task<IEnumerable<NewsFilterModel>> GetAsync()
        {
            var models = await _filterRepository.GetAsync();
            return models
                .GroupBy(m => m.GroupGuid)
                .Select(m => _mapper.Map<NewsFilterModel>(m));
        }

        public async Task<IEnumerable<NewsItem>> FilterNewsAsync(Guid filterGuid, IEnumerable<NewsItem> news)
        {
            if (!await _filterRepository.IsExistsAsync(f => f.GroupGuid == filterGuid).ConfigureAwait(false))
            {
                throw new NotFoundException(Localization.FilterNotFound);
            }

            var models = await _filterRepository.GetGroupAsync(filterGuid);
            news = news.Where(n =>
                models.Any(m =>
                {
                    var regex = new Regex(m.Filter);
                    return regex.IsMatch(n.Description) || regex.IsMatch(n.Title);
                }));

            return news;
        }

        public Task EnsureFiltersIsValidAsync(IEnumerable<string> filters)
        {
            foreach (var filter in filters)
            {
                try
                {
                    new Regex(filter);
                }
                catch (RegexParseException e)
                {
                    return Task.FromException(new BadRequestException(string.Format(Localization.NotAFilter, filter), e));
                }
            }

            return Task.CompletedTask;
        }
    }
}
