using System.Collections.Generic;
using System.Threading.Tasks;
using Downloader.API.Models;

namespace Downloader.API.ExternalRepositories
{
    public interface IRssExternalRepository
    {
        Task EnsureCorrectRssSourceAsync(string rssSourceUrl);

        Task<IEnumerable<RssSourceResponseItem>> RequestRssSourceAsync(string rssSourceUrl);
    }
}
