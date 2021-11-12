using System.Collections.Generic;
using System.Threading.Tasks;
using Downloader.API.Models;

namespace Downloader.API.ExternalServices
{
    public interface IRssExternalService
    {
        Task EnsureCorrectRssSourceAsync(string rssSourceUrl);

        Task<IEnumerable<RssSourceResponseItem>> RequestRssSourceAsync(string rssSourceUrl);
    }
}
