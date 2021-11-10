using Db.Common.Repositories;
using Downloader.API.Database;
using Downloader.API.Models;

namespace Downloader.API.Repositories
{
    public sealed class RssSourceRepository : BaseRepository<RssSource>, IRssSourceRepository
    {
        public RssSourceRepository(DownloaderDbContext downloaderDbContext)
            : base(downloaderDbContext, downloaderDbContext.RssSources)
        { }
    }
}
