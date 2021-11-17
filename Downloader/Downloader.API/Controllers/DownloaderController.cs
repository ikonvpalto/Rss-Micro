using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Common.Models;
using Downloader.Common.Contracts;
using Downloader.Common.Models;
using Downloader.Common.Models.RequestModels;
using Microsoft.AspNetCore.Mvc;

namespace Downloader.API.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/rss-sources")]
    public sealed class DownloaderController : ControllerBase
    {
        private readonly IDownloaderProvider _downloaderProvider;
        private readonly IDownloaderManager _downloaderManager;
        private readonly IMapper _mapper;

        public DownloaderController(
            IDownloaderProvider downloaderProvider,
            IDownloaderManager downloaderManager,
            IMapper mapper)
        {
            _downloaderProvider = downloaderProvider;
            _downloaderManager = downloaderManager;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("{rssSourceGuid:guid}")]
        [ProducesResponseType(typeof(RssSourceReadModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetRssSourceAsync([FromRoute] Guid rssSourceGuid)
        {
            var sources = await _downloaderProvider.GetAsync(rssSourceGuid).ConfigureAwait(false);
            return Ok(sources);
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(IEnumerable<RssSourceReadModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetRssSourcesAsync()
        {
            var sources = await _downloaderProvider.GetAsync().ConfigureAwait(false);
            return Ok(sources);
        }

        [HttpGet]
        [Route("ensure-valid")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> EnsureRssSourceValidAsync([FromQuery] string rssSourceUrl)
        {
            await _downloaderProvider.EnsureRssSourceIsValidAsync(rssSourceUrl).ConfigureAwait(false);
            return Ok();
        }

        [HttpPost]
        [Route("")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateAsync([FromBody] RssSourceManageModel rssSource)
        {
            await _downloaderManager.CreateAsync(rssSource).ConfigureAwait(false);
            return Ok();
        }

        [HttpPut]
        [Route("{rssSourceGuid:guid}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid rssSourceGuid, [FromBody] RssSourceRequestModel rssSourceRequestModel)
        {
            var rssSource = _mapper.Map(rssSourceRequestModel, new RssSourceManageModel { Guid = rssSourceGuid });
            await _downloaderManager.UpdateAsync(rssSource).ConfigureAwait(false);
            return Ok();
        }

        [HttpDelete]
        [Route("{rssSourceGuid:guid}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid rssSourceGuid)
        {
            await _downloaderManager.DeleteAsync(rssSourceGuid).ConfigureAwait(false);
            return Ok();
        }

        [HttpPost]
        [Route("{rssSourceGuid:guid}/news")]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(IEnumerable<NewsItem>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DownloadNewsAsync([FromRoute] Guid rssSourceGuid)
        {
            var news = await _downloaderManager.DownloadNewsAsync(rssSourceGuid).ConfigureAwait(false);
            return Ok(news);
        }

        [HttpPost]
        [Route("news")]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(IEnumerable<NewsItem>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DownloadAllNewsAsync()
        {
            var news = await _downloaderManager.DownloadAllNewsAsync().ConfigureAwait(false);
            return Ok(news);
        }
    }
}
