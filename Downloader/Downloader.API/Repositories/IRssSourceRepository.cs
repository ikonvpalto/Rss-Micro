using Db.Common.Repositories;
using Db.Common.Repositories.Contracts;
using Downloader.API.Models;

namespace Downloader.API.Repositories
{
    public interface IRssSourceRepository : IBaseRepository<RssSource>
    {
    }
}
