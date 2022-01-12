using System.Collections.Generic;
using System.Threading.Tasks;
using Downloader.Common.Models;
using Gateway.Common.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/news")]
    public class NewsController : ControllerBase
    {
        private readonly IRssServiceManager _rssServiceManager;

        public NewsController(IRssServiceManager rssServiceManager)
        {
            _rssServiceManager = rssServiceManager;
        }

        /// <summary>Get all news</summary>
        [HttpGet("")]
        public async Task<ICollection<NewsItem>> DownloadNewsAsync()
        {
            return await _rssServiceManager.DownloadNewsAsync().ConfigureAwait(false);
        }
    }
}
