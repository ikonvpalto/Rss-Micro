using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Downloader.Common.Models;
using Filter.Common.Contracts;
using Filter.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Filter.API.Controllers
{
    [ApiController]
    [Route("api/filter")]
    public sealed class FilterController : ControllerBase
    {
        private readonly IFilterProvider _filterProvider;
        private readonly IFilterManager _filterManager;

        public FilterController(IFilterProvider filterProvider, IFilterManager filterManager)
        {
            _filterProvider = filterProvider;
            _filterManager = filterManager;
        }

        [HttpGet]
        [Route("{filterGuid:guid}")]
        [ProducesResponseType(typeof(NewsFilterModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAsync([FromRoute] Guid filterGuid)
        {
            var filter = await _filterProvider.GetAsync(filterGuid).ConfigureAwait(false);
            return Ok(filter);
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(IEnumerable<NewsFilterModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAsync()
        {
            var filters = await _filterProvider.GetAsync().ConfigureAwait(false);
            return Ok(filters);
        }

        [HttpPost]
        [Route("{filterGuid:guid}/filter-news")]
        [ProducesResponseType(typeof(IEnumerable<NewsItem>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> FilterNewsAsync([FromRoute] Guid filterGuid, [FromBody] IEnumerable<NewsItem> news)
        {
            var filteredNews = await _filterProvider.FilterNewsAsync(filterGuid, news).ConfigureAwait(false);
            return Ok(filteredNews);
        }

        [HttpPost]
        [Route("ensure-valid")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> EnsureFiltersValidAsync([FromBody] IEnumerable<string> filters)
        {
            await _filterProvider.EnsureFiltersValidAsync(filters).ConfigureAwait(false);
            return Ok();
        }

        [HttpPost]
        [Route("")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateAsync([FromBody] NewsFilterModel filter)
        {
            await _filterManager.CreateAsync(filter);
            return Ok();
        }

        [HttpPut]
        [Route("")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateAsync([FromBody] NewsFilterModel filter)
        {
            await _filterManager.UpdateAsync(filter);
            return Ok();
        }

        [HttpDelete]
        [Route("{filterGuid:guid}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid filterGuid)
        {
            await _filterManager.DeleteAsync(filterGuid);
            return Ok();
        }
    }
}
