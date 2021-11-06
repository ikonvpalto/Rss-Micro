using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Downloader.API.Models;

namespace Downloader.API.Repositories
{
    public interface IRssSourceRepository
    {
        Task CreateAsync(RssSource rssSource);

        Task<RssSource> GetAsync(Guid rssSourceGuid);

        Task<IEnumerable<RssSource>> GetAsync();

        Task<bool> IsRssSourceExistsAsync(Guid rssSourceGuid);

        Task UpdateAsync(RssSource rssSource);

        Task DeleteAsync(Guid rssSourceGuid);
    }
}
