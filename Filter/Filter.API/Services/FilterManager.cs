using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Common.Exceptions;
using Filter.API.Models;
using Filter.API.Repositories;
using Filter.API.Resources;
using Filter.Common.Contracts;
using Filter.Common.Models;

namespace Filter.API.Services
{
    public sealed class FilterManager : IFilterManager
    {
        private readonly IMapper _mapper;
        private readonly IFilterRepository _filterRepository;
        private readonly IFilterProvider _filterProvider;

        public FilterManager(
            IMapper mapper,
            IFilterRepository filterRepository,
            IFilterProvider filterProvider)
        {
            _mapper = mapper;
            _filterRepository = filterRepository;
            _filterProvider = filterProvider;
        }

        public async Task CreateAsync(NewsFilterModel filter)
        {
            if (await _filterRepository.IsExistsAsync(f => f.GroupGuid == filter.Guid).ConfigureAwait(false))
            {
                throw new AlreadyExistsException(Localization.FilterAlreadyExists);
            }

            await _filterProvider.EnsureFiltersIsValidAsync(filter.Filters).ConfigureAwait(false);

            var models = filter.Filters.Select(f =>
            {
                var model = _mapper.Map<FilterModel>(filter);
                model.Filter = f;
                return model;
            });

            await _filterRepository.CreateAsync(models).ConfigureAwait(false);
        }

        public async Task UpdateAsync(NewsFilterModel filter)
        {
            await _filterProvider.EnsureFiltersIsValidAsync(filter.Filters).ConfigureAwait(false);

            await _filterRepository.DeleteGroupAsync(filter.Guid).ConfigureAwait(false);

            await CreateAsync(filter).ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid filterGuid)
        {
            if (!await _filterRepository.IsExistsAsync(f => f.GroupGuid == filterGuid).ConfigureAwait(false))
            {
                throw new NotFoundException(Localization.FilterNotFound);
            }

            await _filterRepository.DeleteGroupAsync(filterGuid).ConfigureAwait(false);
        }
    }
}
