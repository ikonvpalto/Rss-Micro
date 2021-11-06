using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Downloader.API.Database;
using Downloader.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Downloader.API.Repositories
{
    public sealed class RssSourceRepository : IRssSourceRepository
    {
        private readonly DownloaderDbContext _downloaderDbContext;

        public RssSourceRepository(DownloaderDbContext downloaderDbContext)
        {
            _downloaderDbContext = downloaderDbContext;
        }

        public async Task CreateAsync(RssSource rssSource)
        {
            _downloaderDbContext.RssSources.Add(rssSource);
            await _downloaderDbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<RssSource> GetAsync(Guid rssSourceGuid)
        {
            return await _downloaderDbContext.RssSources.SingleOrDefaultAsync(s => s.Guid == rssSourceGuid)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<RssSource>> GetAsync()
        {
            return await _downloaderDbContext.RssSources.ToListAsync().ConfigureAwait(false);
        }

        public async Task<bool> IsRssSourceExistsAsync(Guid rssSourceGuid)
        {
            return await _downloaderDbContext.RssSources.AnyAsync(s => s.Guid == rssSourceGuid).ConfigureAwait(false);
        }

        public async Task UpdateAsync(RssSource rssSource)
        {
            _downloaderDbContext.RssSources.Update(rssSource);
            await _downloaderDbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid rssSourceGuid)
        {
            var source = await _downloaderDbContext.RssSources.SingleOrDefaultAsync(s => s.Guid == rssSourceGuid);
            _downloaderDbContext.Remove(source);
            await _downloaderDbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
