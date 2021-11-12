using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Downloader.Common.Models;
using Filter.Common.Models;

namespace Filter.Common.Contracts
{
    public interface IFilterProvider
    {
        Task<NewsFilterModel> GetAsync(Guid filterGuid);

        Task<IEnumerable<NewsFilterModel>> GetAsync();

        Task<IEnumerable<NewsItem>> FilterNewsAsync(Guid filterGuid, IEnumerable<NewsItem> news);

        Task EnsureFiltersIsValidAsync(IEnumerable<string> filters);
    }
}
